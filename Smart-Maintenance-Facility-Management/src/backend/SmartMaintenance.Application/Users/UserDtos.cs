namespace SmartMaintenance.Application.Users;

public sealed class UserSummary
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public sealed class CreateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public string? FullName { get; set; }
}

public sealed class UpdateUserRequest
{
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}
