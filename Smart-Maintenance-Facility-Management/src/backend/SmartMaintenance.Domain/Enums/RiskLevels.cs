namespace SmartMaintenance.Domain.Enums;

public static class RiskLevels
{
    public const string Low = "Low";
    public const string Medium = "Medium";
    public const string High = "High";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Low, Medium, High
    };

    public static bool IsValid(string? risk) =>
        !string.IsNullOrWhiteSpace(risk) && Allowed.Contains(risk.Trim());
}
