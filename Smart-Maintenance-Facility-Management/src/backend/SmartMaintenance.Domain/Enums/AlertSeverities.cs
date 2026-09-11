namespace SmartMaintenance.Domain.Enums;

public static class AlertSeverities
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Low, Medium, High
    };

    public static bool IsValid(string? severity) =>
        !string.IsNullOrWhiteSpace(severity) && Allowed.Contains(severity.Trim());
}
