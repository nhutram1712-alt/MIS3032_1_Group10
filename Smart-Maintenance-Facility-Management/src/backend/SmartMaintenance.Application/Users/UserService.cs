using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.Users;

public sealed class UserService : IUserService
{
    private static readonly HashSet<string> RotatableRoles = new(StringComparer.Ordinal)
    {
        UserRoles.Technician,
        UserRoles.FacilityManager
    };

    private readonly IAppDbContext _db;
    private readonly ILogger<UserService> _logger;

    public UserService(IAppDbContext db, ILogger<UserService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public Task<IReadOnlyList<UserSummary>> ListAsync(string? role = null, CancellationToken cancellationToken = default)
    {
        // Admin listing (no role filter) includes inactive users for soft-disable management.
        var query = _db.Users.AsQueryable();
        if (!string.IsNullOrWhiteSpace(role))
        {
            var normalized = role.Trim();
            if (!UserRoles.IsValid(normalized))
                throw new AppException($"Role is invalid. Allowed: {string.Join(", ", UserRoles.Allowed)}.", 400);
            query = query.Where(u => u.IsActive && u.Role == normalized);
        }

        var items = query
            .OrderBy(u => u.FullName)
            .Select(u => new UserSummary
            {
                UserId = u.UserId,
                Username = u.Username,
                FullName = u.FullName,
                Role = u.Role,
                IsActive = u.IsActive
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<UserSummary>>(items);
    }

    public async Task<UserSummary> CreateAsync(CreateUserRequest request, int actorUserId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            throw new AppException("Username is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new AppException("Password is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Role))
            throw new AppException("Role is required.", 400);

        var username = request.Username.Trim();
        var role = request.Role.Trim();
        if (!UserRoles.IsValid(role))
            throw new AppException($"Role is invalid. Allowed: {string.Join(", ", UserRoles.Allowed)}.", 400);

        if (_db.Users.Any(u => u.Username == username))
            throw new AppException("Username already exists.", 400);

        var fullName = string.IsNullOrWhiteSpace(request.FullName) ? username : request.FullName.Trim();
        var user = new User
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role,
            FullName = fullName,
            IsActive = true
        };

        _db.AddUser(user);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "UserCreated AdminUserID={AdminUserId} TargetUserID={TargetUserId} Username={Username} Role={Role}",
            actorUserId, user.UserId, user.Username, user.Role);

        return ToSummary(user);
    }

    public async Task<UserSummary> UpdateAsync(int id, UpdateUserRequest request, int actorUserId, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new AppException("User id is invalid.", 400);

        var user = _db.Users.FirstOrDefault(u => u.UserId == id);
        if (user is null)
            throw new AppException("User not found.", 404);

        // QT-4: Admin accounts are immutable via PUT.
        if (user.Role == UserRoles.Admin)
            throw new AppException("Admin accounts cannot be modified via this endpoint.", 403);

        if (request.Role is not null)
        {
            if (string.IsNullOrWhiteSpace(request.Role))
                throw new AppException("Role cannot be empty.", 400);

            var newRole = request.Role.Trim();

            // QT-3: Admin may only be assigned at create time.
            if (newRole == UserRoles.Admin)
            {
                throw new AppException(
                    "Cannot assign Admin role via this endpoint. Use POST /api/users to create an Admin account.",
                    400);
            }

            // QT-2: Requester accounts cannot change role.
            if (user.Role == UserRoles.Requester)
                throw new AppException("Cannot change role of a Requester account.", 400);

            if (!UserRoles.IsValid(newRole))
                throw new AppException($"Role is invalid. Allowed: {string.Join(", ", UserRoles.Allowed)}.", 400);

            // QT-1: PUT only rotates Technician ↔ FacilityManager.
            if (!RotatableRoles.Contains(user.Role) || !RotatableRoles.Contains(newRole))
            {
                throw new AppException(
                    "This endpoint only allows rotating roles between Technician and FacilityManager.",
                    400);
            }

            var oldRole = user.Role;
            user.Role = newRole;

            _logger.LogInformation(
                "UserRoleUpdated AdminUserID={AdminUserId} TargetUserID={TargetUserId} OldRole={OldRole} NewRole={NewRole}",
                actorUserId, user.UserId, oldRole, newRole);
        }

        if (request.IsActive is not null)
            user.IsActive = request.IsActive.Value;

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "UserUpdated AdminUserID={AdminUserId} TargetUserID={TargetUserId} Username={Username} Role={Role} IsActive={IsActive}",
            actorUserId, user.UserId, user.Username, user.Role, user.IsActive);

        return ToSummary(user);
    }

    private static UserSummary ToSummary(User user) => new()
    {
        UserId = user.UserId,
        Username = user.Username,
        FullName = user.FullName,
        Role = user.Role,
        IsActive = user.IsActive
    };
}
