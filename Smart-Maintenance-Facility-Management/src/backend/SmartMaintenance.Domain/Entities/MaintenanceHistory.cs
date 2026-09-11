namespace SmartMaintenance.Domain.Entities;

public class MaintenanceHistory
{
    public int HistoryId { get; set; }
    public int AssetId { get; set; }
    public int? WorkOrderId { get; set; }
    public string Result { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
}
