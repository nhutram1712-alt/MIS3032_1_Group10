namespace SmartMaintenance.Domain.Entities;

public class WorkOrder
{
    public int OrderId { get; set; }
    public int RequestId { get; set; }
    public int TechnicianId { get; set; }
    public int AssetId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
}
