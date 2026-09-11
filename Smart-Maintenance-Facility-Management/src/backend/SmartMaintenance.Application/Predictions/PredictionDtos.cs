namespace SmartMaintenance.Application.Predictions;

public sealed class AiPredictRequest
{
    public int AssetId { get; set; }
    public int HorizonDays { get; set; } = 7;
    public IReadOnlyList<AiIotReading> IotReadings { get; set; } = [];
    public int HistoryCount { get; set; }
}

public sealed class AiIotReading
{
    public string MetricType { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Timestamp { get; set; }
}

public sealed class AiPredictResult
{
    public string? Risk { get; set; }
    public bool BasedOnSampleData { get; set; }
}

public sealed class PredictionResponse
{
    public int AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public string AssetLocation { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Risk { get; set; } = string.Empty;
    public DateTime PredictedAt { get; set; }
    public int HorizonDays { get; set; } = 7;
    public bool Stale { get; set; }
    public bool BasedOnSampleData { get; set; }
}
