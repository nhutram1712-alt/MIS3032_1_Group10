using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.WorkOrders;

public sealed class WorkOrderService : IWorkOrderService
{
    private readonly IAppDbContext _db;
    private readonly ILogger<WorkOrderService> _logger;

    public WorkOrderService(IAppDbContext db, ILogger<WorkOrderService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<WorkOrderResponse> CreateAsync(
        CreateWorkOrderPayload request,
        int facilityManagerId,
        CancellationToken cancellationToken = default)
    {
        if (request.RequestId is null or <= 0)
            throw new AppException("requestId is required.", 400);
        if (request.TechnicianId is null or <= 0)
            throw new AppException("technicianId is required.", 400);
        if (request.AssetId is null or <= 0)
            throw new AppException("assetId is required.", 400);

        var requestId = request.RequestId.Value;
        var technicianId = request.TechnicianId.Value;
        var assetId = request.AssetId.Value;

        var maintenanceRequest = _db.MaintenanceRequests.FirstOrDefault(r => r.RequestId == requestId);
        if (maintenanceRequest is null)
            throw new AppException("Maintenance Request not found.", 404);

        if (_db.WorkOrders.Any(w => w.RequestId == requestId))
        {
            _logger.LogWarning(
                "US-04-01 rejected BR-06: FacilityManagerId={FacilityManagerId}, RequestId={RequestId} already has a Work Order",
                facilityManagerId,
                requestId);
            throw new AppException("This Maintenance Request already has a Work Order.", 409);
        }

        if (maintenanceRequest.AssetId is null or <= 0)
        {
            _logger.LogWarning(
                "US-04-01 rejected BR-05: FacilityManagerId={FacilityManagerId}, RequestId={RequestId} has no Asset",
                facilityManagerId,
                requestId);
            throw new AppException("Asset must be identified on the Request before creating a Work Order.", 422);
        }

        if (!string.Equals(maintenanceRequest.Status, RequestStatuses.Submitted, StringComparison.Ordinal))
            throw new AppException("Only Submitted maintenance requests can be assigned a Work Order.", 400);

        if (maintenanceRequest.AssetId != assetId)
            throw new AppException("assetId does not match the Asset on the Maintenance Request.", 400);

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
            throw new AppException("Asset not found.", 404);

        var technician = _db.Users.FirstOrDefault(u => u.UserId == technicianId && u.IsActive);
        if (technician is null || !string.Equals(technician.Role, UserRoles.Technician, StringComparison.Ordinal))
            throw new AppException("technicianId must belong to an active user with Role Technician.", 400);

        var workOrder = new WorkOrder
        {
            RequestId = requestId,
            TechnicianId = technicianId,
            AssetId = assetId,
            Status = WorkOrderStatuses.Assigned,
            CreatedAt = DateTime.UtcNow
        };

        _db.AddWorkOrder(workOrder);
        SyncRequestStatus(maintenanceRequest, RequestStatuses.Pending);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-04-01 CreateWorkOrder: FacilityManagerId={FacilityManagerId}, RequestId={RequestId}, AssetId={AssetId}, TechnicianId={TechnicianId}, OrderId={OrderId}, Timestamp={Timestamp:o}",
            facilityManagerId,
            requestId,
            assetId,
            technicianId,
            workOrder.OrderId,
            DateTime.UtcNow);

        return ToResponse(workOrder);
    }

    public Task<IReadOnlyList<WorkOrderResponse>> ListAsync(int userId, string role, CancellationToken cancellationToken = default)
    {
        var query = _db.WorkOrders.AsQueryable();
        if (string.Equals(role, UserRoles.Technician, StringComparison.Ordinal))
            query = query.Where(w => w.TechnicianId == userId);

        var items = query
            .OrderByDescending(w => w.CreatedAt)
            .ToList()
            .Select(ToResponse)
            .ToList();

        return Task.FromResult<IReadOnlyList<WorkOrderResponse>>(items);
    }

    public Task<WorkOrderDetailResponse> GetByIdAsync(int id, int userId, string role, CancellationToken cancellationToken = default)
    {
        var workOrder = FindWorkOrder(id);
        EnsureTechnicianAccess(workOrder, userId, role);

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == workOrder.AssetId);
        var history = _db.MaintenanceHistories
            .Where(h => h.WorkOrderId == workOrder.OrderId)
            .OrderByDescending(h => h.CompletedAt)
            .ToList()
            .Select(h => new WorkOrderHistoryItem
            {
                HistoryId = h.HistoryId,
                AssetId = h.AssetId,
                WorkOrderId = h.WorkOrderId,
                Result = h.Result,
                CompletedAt = h.CompletedAt
            })
            .ToList();

        return Task.FromResult(new WorkOrderDetailResponse
        {
            OrderId = workOrder.OrderId,
            RequestId = workOrder.RequestId,
            TechnicianId = workOrder.TechnicianId,
            AssetId = workOrder.AssetId,
            Status = workOrder.Status,
            RejectionReason = workOrder.RejectionReason,
            CreatedAt = workOrder.CreatedAt,
            Asset = asset is null
                ? null
                : new AssetResponse
                {
                    AssetId = asset.AssetId,
                    Name = asset.Name,
                    Type = asset.Type,
                    Location = asset.Location,
                    Status = asset.Status,
                    MaintenanceRisk = asset.MaintenanceRisk,
                    CreatedAt = asset.CreatedAt
                },
            MaintenanceHistory = history
        });
    }

    public async Task<WorkOrderResponse> PatchAsync(
        int id,
        PatchWorkOrderPayload payload,
        int userId,
        string role,
        CancellationToken cancellationToken = default)
    {
        var workOrder = FindWorkOrder(id);
        var isFm = string.Equals(role, UserRoles.FacilityManager, StringComparison.Ordinal);
        var isTech = string.Equals(role, UserRoles.Technician, StringComparison.Ordinal);

        if (!isFm && !isTech)
            throw new AppException("Forbidden.", 403);

        if (isTech)
            EnsureTechnicianAccess(workOrder, userId, role);

        if (string.Equals(workOrder.Status, WorkOrderStatuses.Completed, StringComparison.Ordinal)
            || string.Equals(workOrder.Status, WorkOrderStatuses.Cancelled, StringComparison.Ordinal))
            throw new AppException($"Work Order status '{workOrder.Status}' is terminal and cannot be changed.", 400);

        var hasTechnicianId = payload.TechnicianId is not null;
        var hasStatus = !string.IsNullOrWhiteSpace(payload.Status);
        var hasRejection = !string.IsNullOrWhiteSpace(payload.RejectionReason);
        var hasResult = !string.IsNullOrWhiteSpace(payload.Result);

        if (isFm)
        {
            if (hasRejection || hasResult)
                throw new AppException("FacilityManager cannot set rejectionReason or result.", 403);

            if (hasTechnicianId)
            {
                if (!string.Equals(workOrder.Status, WorkOrderStatuses.Assigned, StringComparison.Ordinal))
                    throw new AppException("Technician can only be reassigned while status is Assigned.", 400);

                var technicianId = payload.TechnicianId!.Value;
                if (technicianId <= 0)
                    throw new AppException("technicianId is invalid.", 400);

                var technician = _db.Users.FirstOrDefault(u => u.UserId == technicianId && u.IsActive);
                if (technician is null || !string.Equals(technician.Role, UserRoles.Technician, StringComparison.Ordinal))
                    throw new AppException("technicianId must belong to an active user with Role Technician.", 400);

                workOrder.TechnicianId = technicianId;
            }

            if (hasStatus)
            {
                var status = payload.Status!.Trim();
                if (!string.Equals(status, WorkOrderStatuses.Cancelled, StringComparison.Ordinal))
                    throw new AppException("FacilityManager may only set status to Cancelled.", 400);
                workOrder.Status = WorkOrderStatuses.Cancelled;
                SyncRequestFromWorkOrder(workOrder);
            }

            if (!hasTechnicianId && !hasStatus)
                throw new AppException("No valid patch fields provided.", 400);
        }
        else
        {
            if (hasTechnicianId)
                throw new AppException("Technician cannot reassign Work Orders.", 403);

            if (hasRejection)
            {
                if (!string.Equals(workOrder.Status, WorkOrderStatuses.Assigned, StringComparison.Ordinal)
                    && !string.Equals(workOrder.Status, WorkOrderStatuses.InProgress, StringComparison.Ordinal))
                    throw new AppException("Work Order cannot be rejected in the current status.", 400);

                workOrder.RejectionReason = payload.RejectionReason!.Trim();
                workOrder.Status = WorkOrderStatuses.Cancelled;
                SyncRequestFromWorkOrder(workOrder);
            }
            else if (hasStatus)
            {
                var status = payload.Status!.Trim();
                if (string.Equals(status, WorkOrderStatuses.InProgress, StringComparison.Ordinal))
                {
                    if (!string.Equals(workOrder.Status, WorkOrderStatuses.Assigned, StringComparison.Ordinal))
                        throw new AppException("Status can only move to In Progress from Assigned.", 400);
                    workOrder.Status = WorkOrderStatuses.InProgress;
                    SyncRequestFromWorkOrder(workOrder);
                }
                else if (string.Equals(status, WorkOrderStatuses.Completed, StringComparison.Ordinal))
                {
                    if (!string.Equals(workOrder.Status, WorkOrderStatuses.InProgress, StringComparison.Ordinal)
                        && !string.Equals(workOrder.Status, WorkOrderStatuses.Assigned, StringComparison.Ordinal))
                        throw new AppException("Status can only move to Completed from Assigned or In Progress.", 400);

                    if (!hasResult)
                        throw new AppException("result is required when completing a Work Order (BR-09).", 400);

                    var result = payload.Result!.Trim();
                    workOrder.Status = WorkOrderStatuses.Completed;
                    _db.AddMaintenanceHistory(new MaintenanceHistory
                    {
                        WorkOrderId = workOrder.OrderId,
                        AssetId = workOrder.AssetId,
                        Result = result,
                        CompletedAt = DateTime.UtcNow
                    });
                    SyncRequestFromWorkOrder(workOrder);
                }
                else
                {
                    throw new AppException("Technician may only set status to In Progress or Completed.", 400);
                }
            }
            else
            {
                throw new AppException("No valid patch fields provided.", 400);
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-04 PatchWorkOrder: OrderId={OrderId}, UserId={UserId}, Role={Role}, Status={Status}",
            workOrder.OrderId,
            userId,
            role,
            workOrder.Status);

        return ToResponse(workOrder);
    }

    private WorkOrder FindWorkOrder(int id)
    {
        if (id <= 0)
            throw new AppException("Work Order id is invalid.", 400);

        var workOrder = _db.WorkOrders.FirstOrDefault(w => w.OrderId == id);
        if (workOrder is null)
            throw new AppException("Work Order not found.", 404);
        return workOrder;
    }

    /// <summary>
    /// Keep Maintenance Request in sync with WO lifecycle so FM does not manage both manually.
    /// Assigned → Pending; In Progress → In Progress; Completed → Closed; Cancelled → Rejected.
    /// </summary>
    private void SyncRequestFromWorkOrder(WorkOrder workOrder)
    {
        var related = _db.MaintenanceRequests.FirstOrDefault(r => r.RequestId == workOrder.RequestId);
        if (related is null) return;

        var next = workOrder.Status switch
        {
            WorkOrderStatuses.Assigned => RequestStatuses.Pending,
            WorkOrderStatuses.InProgress => RequestStatuses.InProgress,
            WorkOrderStatuses.Completed => RequestStatuses.Closed,
            WorkOrderStatuses.Cancelled => RequestStatuses.Rejected,
            _ => null
        };

        if (next is null) return;
        SyncRequestStatus(related, next);
    }

    private static void SyncRequestStatus(MaintenanceRequest request, string nextStatus)
    {
        if (string.Equals(request.Status, RequestStatuses.Closed, StringComparison.Ordinal)
            || string.Equals(request.Status, RequestStatuses.Rejected, StringComparison.Ordinal))
            return;

        request.Status = nextStatus;
    }

    private static void EnsureTechnicianAccess(WorkOrder workOrder, int userId, string role)
    {
        if (string.Equals(role, UserRoles.Technician, StringComparison.Ordinal)
            && workOrder.TechnicianId != userId)
            throw new AppException("Technician is not assigned to this Work Order (BR-07).", 403);
    }

    private static WorkOrderResponse ToResponse(WorkOrder workOrder) => new()
    {
        OrderId = workOrder.OrderId,
        RequestId = workOrder.RequestId,
        TechnicianId = workOrder.TechnicianId,
        AssetId = workOrder.AssetId,
        Status = workOrder.Status,
        RejectionReason = workOrder.RejectionReason,
        CreatedAt = workOrder.CreatedAt
    };
}
