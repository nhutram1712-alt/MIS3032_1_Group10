namespace SmartMaintenance.Domain.Entities;

public class IotDevice
{
    public int DeviceId { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string? DeviceName { get; set; }
    public string? DeviceType { get; set; }
}
