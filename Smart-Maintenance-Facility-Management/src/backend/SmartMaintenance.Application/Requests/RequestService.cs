using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.Requests;

public sealed class RequestService : IRequestService
{
    public const int DescriptionMaxLength = 500;

    private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new(StringComparer.Ordinal)
    {
        [RequestStatuses.Submitted] = new(StringComparer.Ordinal) { RequestStatuses.Pending, RequestStatuses.Rejected },
        [RequestStatuses.Pending] = new(StringComparer.Ordinal) { RequestStatuses.InProgress, RequestStatuses.Rejected },
        [RequestStatuses.InProgress] = new(StringComparer.Ordinal) { RequestStatuses.Resolved, RequestStatuses.Rejected },
        [RequestStatuses.Resolved] = new(StringComparer.Ordinal) { RequestStatuses.Closed }
    };

    private readonly IAppDbContext _db;
    private readonly ILogger<RequestService> _logger;

    public RequestService(IAppDbContext db, ILogger<RequestService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<RequestResponse> CreateAsync(
        CreateRequestPayload request,
        int requesterId,
        CancellationToken cancellationToken = default)
    {
        if (request.AssetId is null or <= 0)
            throw new AppException("AssetID is required and must be greater than 0.", 400);

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new AppException("Description is required.", 400);

        var description = request.Description.Trim();
        if (description.Length > DescriptionMaxLength)
            throw new AppException($"Description must be at most {DescriptionMaxLength} characters.", 400);

        var assetId = request.AssetId.Value;
        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
            throw new AppException("Asset not found.", 404);

        var entity = new MaintenanceRequest
        {
            RequesterId = requesterId,
            AssetId = assetId,
            Description = description,
            Status = RequestStatuses.Submitted,
            CreatedAt = DateTime.UtcNow
        };

        _db.AddMaintenanceRequest(entity);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-03-01 CreateRequest: RequesterId={RequesterId}, AssetId={AssetId}, RequestId={RequestId}, Timestamp={Timestamp:o}",
            requesterId,
            assetId,
            entity.RequestId,
            DateTime.UtcNow);

        return ToResponse(entity);
    }

    public Task<IReadOnlyList<RequestResponse>> ListAsync(int userId, string role, CancellationToken cancellationToken = default)
    {
        var query = _db.MaintenanceRequests.AsQueryable();
        if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal))
            query = query.Where(r => r.RequesterId == userId);

        var items = query
            .OrderByDescending(r => r.CreatedAt)
            .ToList()
            .Select(ToResponse)
            .ToList();

        return Task.FromResult<IReadOnlyList<RequestResponse>>(items);
    }

    public Task<RequestResponse> GetByIdAsync(int id, int userId, string role, CancellationToken cancellationToken = default)
    {
        var entity = FindRequest(id);

        if (string.Equals(role, UserRoles.Requester, StringComparison.Ordinal) && entity.RequesterId != userId)
            throw new AppException("You do not have access to this request.", 403);

        return Task.FromResult(ToResponse(entity));
    }

    public async Task<RequestResponse> UpdateStatusAsync(
        int id,
        UpdateRequestStatusPayload request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
            throw new AppException("Status is required.", 400);

        var next = request.Status.Trim();
        if (!RequestStatuses.IsValid(next))
            throw new AppException(
                "Status must be one of BR-14: Submitted, Pending, In Progress, Resolved, Closed, Rejected.",
                400);

        var entity = FindRequest(id);
        var current = entity.Status;

        if (string.Equals(current, RequestStatuses.Rejected, StringComparison.Ordinal)
            || string.Equals(current, RequestStatuses.Closed, StringComparison.Ordinal))
            throw new AppException($"Request status '{current}' is terminal and cannot be changed.", 400);

        if (!AllowedTransitions.TryGetValue(current, out var allowed) || !allowed.Contains(next))
            throw new AppException($"Invalid status transition from '{current}' to '{next}'.", 400);

        if (string.Equals(next, RequestStatuses.Closed, StringComparison.Ordinal))
        {
            var workOrder = _db.WorkOrders.FirstOrDefault(w => w.RequestId == id);
            if (workOrder is null || !string.Equals(workOrder.Status, WorkOrderStatuses.Completed, StringComparison.Ordinal))
                throw new AppException("Related Work Order must be Completed before closing the Request (BR-18).", 400);
        }

        entity.Status = next;
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-03-03 UpdateRequestStatus: RequestId={RequestId}, From={From}, To={To}",
            id,
            current,
            next);

        return ToResponse(entity);
    }

    public Task<RequestHistoryResponse> GetHistoryAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = FindRequest(id);

        var workOrder = _db.WorkOrders.FirstOrDefault(w => w.RequestId == id);
        if (workOrder is null)
        {
            return Task.FromResult(new RequestHistoryResponse { RequestId = id });
        }

        var history = _db.MaintenanceHistories
            .Where(h => h.WorkOrderId == workOrder.OrderId)
            .OrderByDescending(h => h.CompletedAt)
            .FirstOrDefault();

        return Task.FromResult(new RequestHistoryResponse
        {
            RequestId = id,
            WorkOrderId = workOrder.OrderId,
            TechnicianId = workOrder.TechnicianId,
            Result = history?.Result,
            CompletedAt = history?.CompletedAt
        });
    }

    private MaintenanceRequest FindRequest(int id)
    {
        if (id <= 0)
            throw new AppException("Request id is invalid.", 400);

        var entity = _db.MaintenanceRequests.FirstOrDefault(r => r.RequestId == id);
        if (entity is null)
            throw new AppException("Maintenance Request not found.", 404);
        return entity;
    }

    private static RequestResponse ToResponse(MaintenanceRequest entity) => new()
    {
        RequestId = entity.RequestId,
        RequesterId = entity.RequesterId,
        AssetId = entity.AssetId,
        Description = entity.Description,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt
    };
}
