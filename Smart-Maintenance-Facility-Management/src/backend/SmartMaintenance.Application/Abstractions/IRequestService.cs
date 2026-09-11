using SmartMaintenance.Application.Requests;

namespace SmartMaintenance.Application.Abstractions;

public interface IRequestService
{
    Task<RequestResponse> CreateAsync(CreateRequestPayload request, int requesterId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RequestResponse>> ListAsync(int userId, string role, CancellationToken cancellationToken = default);
    Task<RequestResponse> GetByIdAsync(int id, int userId, string role, CancellationToken cancellationToken = default);
    Task<RequestResponse> UpdateStatusAsync(int id, UpdateRequestStatusPayload request, CancellationToken cancellationToken = default);
    Task<RequestHistoryResponse> GetHistoryAsync(int id, CancellationToken cancellationToken = default);
}
