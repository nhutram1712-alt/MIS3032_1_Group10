namespace SmartMaintenance.Domain.Entities;

public class IotAlert
{
    public int AlertId { get; set; }
    public int DataId { get; set; }
    public int AssetId { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public double ReadingValue { get; set; }
    public double Threshold { get; set; }
    public string Severity { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
}
