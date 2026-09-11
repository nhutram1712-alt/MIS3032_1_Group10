namespace SmartMaintenance.Domain.Enums;

/// <summary>BR-03: MVP asset types only.</summary>
public static class AssetTypes
{
    public const string WiFi = "Wi-Fi";
    public const string AirConditioner = "Air Conditioner";
    public const string Projector = "Projector";
    public const string Light = "Light";
    public const string Fan = "Fan";

    public static readonly HashSet<string> Allowed = new(StringComparer.Ordinal)
    {
        WiFi, AirConditioner, Projector, Light, Fan
    };

    public static bool IsValid(string? type) =>
        !string.IsNullOrWhiteSpace(type) && Allowed.Contains(type.Trim());
}
