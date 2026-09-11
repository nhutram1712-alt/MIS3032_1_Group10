namespace SmartMaintenance.Domain.Enums;

/// <summary>Asset status values from data-requirements.md.</summary>
public static class AssetStatuses
{
    public const string Operational = "Operational";
    public const string Warning = "Warning";
    public const string Maintenance = "Maintenance";
    public const string OutOfService = "Out of Service";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Operational, Warning, Maintenance, OutOfService
    };

    public static bool IsValid(string? status) =>
        !string.IsNullOrWhiteSpace(status) && Allowed.Contains(status.Trim());
}
