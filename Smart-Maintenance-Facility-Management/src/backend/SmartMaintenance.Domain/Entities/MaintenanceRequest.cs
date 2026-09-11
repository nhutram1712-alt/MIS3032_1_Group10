namespace SmartMaintenance.Domain.Entities;

public class MaintenanceRequest
{
    public int RequestId { get; set; }
    public int RequesterId { get; set; }
    public int? AssetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
