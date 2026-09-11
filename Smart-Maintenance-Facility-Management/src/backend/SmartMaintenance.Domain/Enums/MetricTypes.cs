namespace SmartMaintenance.Domain.Enums;

public static class MetricTypes
{
    public const string Temperature = "temperature";
    public const string Humidity = "humidity";
    public const string PowerStatus = "power_status";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        Temperature, Humidity, PowerStatus
    };

    public static bool IsValid(string? type) =>
        !string.IsNullOrWhiteSpace(type) && Allowed.Contains(type.Trim());
}
