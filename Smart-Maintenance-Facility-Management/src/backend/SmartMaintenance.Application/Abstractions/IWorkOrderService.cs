using SmartMaintenance.Application.WorkOrders;

namespace SmartMaintenance.Application.Abstractions;

public interface IWorkOrderService
{
    Task<WorkOrderResponse> CreateAsync(CreateWorkOrderPayload request, int facilityManagerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<WorkOrderResponse>> ListAsync(int userId, string role, CancellationToken cancellationToken = default);
    Task<WorkOrderDetailResponse> GetByIdAsync(int id, int userId, string role, CancellationToken cancellationToken = default);
    Task<WorkOrderResponse> PatchAsync(int id, PatchWorkOrderPayload payload, int userId, string role, CancellationToken cancellationToken = default);
}
