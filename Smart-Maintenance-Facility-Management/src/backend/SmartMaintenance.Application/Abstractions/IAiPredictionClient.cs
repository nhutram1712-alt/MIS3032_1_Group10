using SmartMaintenance.Application.Predictions;

namespace SmartMaintenance.Application.Abstractions;

public interface IAiPredictionClient
{
    Task<AiPredictResult?> PredictAsync(AiPredictRequest request, CancellationToken cancellationToken = default);
}
