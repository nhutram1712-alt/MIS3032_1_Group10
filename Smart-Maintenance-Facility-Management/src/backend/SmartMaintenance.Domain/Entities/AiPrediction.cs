namespace SmartMaintenance.Domain.Entities;

public class AiPrediction
{
    public int PredictionId { get; set; }
    public int AssetId { get; set; }
    public string RiskLevel { get; set; } = string.Empty;
    public DateTime PredictedAt { get; set; }
    public bool BasedOnSampleData { get; set; }
}
