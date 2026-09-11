namespace SmartMaintenance.Domain.Entities;

public class IotMapping
{
    public int MappingId { get; set; }
    public int AssetId { get; set; }
    public int DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
}
