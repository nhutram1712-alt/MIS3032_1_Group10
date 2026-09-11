namespace SmartMaintenance.Domain.Entities;

public class IotData
{
    public int DataId { get; set; }
    public int DeviceId { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public double ReadingValue { get; set; }
    public DateTime Timestamp { get; set; }
}
