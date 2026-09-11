using SmartMaintenance.Application.Users;

namespace SmartMaintenance.Application.Abstractions;

public interface IUserService
{
    Task<IReadOnlyList<UserSummary>> ListAsync(string? role = null, CancellationToken cancellationToken = default);
    Task<UserSummary> CreateAsync(CreateUserRequest request, int actorUserId, CancellationToken cancellationToken = default);
    Task<UserSummary> UpdateAsync(int id, UpdateUserRequest request, int actorUserId, CancellationToken cancellationToken = default);
}
