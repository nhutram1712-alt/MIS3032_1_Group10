using System.Text.Json.Serialization;

namespace SmartMaintenance.Application.Iot;

public sealed class IotIngestRequest
{
    public string? DeviceId { get; set; }
    public DateTime? Timestamp { get; set; }
    public IotMetricsDto? Metrics { get; set; }
}

public sealed class IotMetricsDto
{
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }

    [JsonPropertyName("power_status")]
    public double? PowerStatus { get; set; }
}

public sealed class IotIngestResponse
{
    public int DeviceId { get; set; }
    public int AssetId { get; set; }
    public int SavedReadings { get; set; }
}

public sealed class IotDataItem
{
    public string MetricType { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}

public sealed class CreateIotMappingRequest
{
    public int? AssetId { get; set; }
    public string? DeviceId { get; set; }
}

public sealed class UpdateIotMappingRequest
{
    public string? DeviceId { get; set; }
}

public sealed class IotMappingResponse
{
    public int MappingId { get; set; }
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public sealed class IotAlertResponse
{
    public int AlertId { get; set; }
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string MetricType { get; set; } = string.Empty;
    public double ReadingValue { get; set; }
    public double Threshold { get; set; }
    public string Severity { get; set; } = string.Empty;
    public DateTime DetectedAt { get; set; }
}
