using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Predictions;

namespace SmartMaintenance.Tests.Fakes;

public sealed class FakeAiPredictionClient : IAiPredictionClient
{
    public string? RiskToReturn { get; set; } = "Low";
    public bool SimulateTimeout { get; set; }
    public bool SimulateInvalidRisk { get; set; }
    public int CallCount { get; private set; }

    public void Reset()
    {
        RiskToReturn = "Low";
        SimulateTimeout = false;
        SimulateInvalidRisk = false;
        CallCount = 0;
    }

    public Task<AiPredictResult?> PredictAsync(AiPredictRequest request, CancellationToken cancellationToken = default)
    {
        CallCount++;
        if (SimulateTimeout)
            throw new TaskCanceledException("AI timeout");
        if (SimulateInvalidRisk)
            return Task.FromResult<AiPredictResult?>(new AiPredictResult { Risk = "Critical" });
        if (RiskToReturn is null)
            return Task.FromResult<AiPredictResult?>(null);
        return Task.FromResult<AiPredictResult?>(new AiPredictResult
        {
            Risk = RiskToReturn,
            BasedOnSampleData = true
        });
    }
}
