namespace SmartMaintenance.Domain.Entities;

public class Asset
{
    public int AssetId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    /// <summary>Latest AI Maintenance Risk (Low/Medium/High). Independent of operational Status (BR-10).</summary>
    public string? MaintenanceRisk { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CreatedByUserId { get; set; }
}
