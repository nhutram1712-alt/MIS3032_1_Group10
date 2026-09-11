namespace SmartMaintenance.Domain.Enums;

/// <summary>BR-14: Maintenance Request lifecycle statuses.</summary>
public static class RequestStatuses
{
    public const string Submitted = "Submitted";
    public const string Pending = "Pending";
    public const string InProgress = "In Progress";
    public const string Resolved = "Resolved";
    public const string Closed = "Closed";
    public const string Rejected = "Rejected";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Submitted, Pending, InProgress, Resolved, Closed, Rejected
    };

    public static bool IsValid(string? status) =>
        !string.IsNullOrWhiteSpace(status) && Allowed.Contains(status.Trim());
}
