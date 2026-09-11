namespace SmartMaintenance.Domain.Enums;

public static class UserRoles
{
    public const string Requester = "Requester";
    public const string Technician = "Technician";
    public const string FacilityManager = "FacilityManager";
    public const string Admin = "Admin";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Requester, Technician, FacilityManager, Admin
    };

    public static bool IsValid(string? role) =>
        !string.IsNullOrWhiteSpace(role) && Allowed.Contains(role.Trim());
}
