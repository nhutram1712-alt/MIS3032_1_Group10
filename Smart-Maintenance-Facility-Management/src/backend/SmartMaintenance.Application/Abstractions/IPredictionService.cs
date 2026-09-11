using SmartMaintenance.Application.Predictions;

namespace SmartMaintenance.Application.Abstractions;

public interface IPredictionService
{
    Task<PredictionResponse> GetLatestAsync(int assetId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PredictionResponse>> ListAllAsync(string? sort = null, CancellationToken cancellationToken = default);
    Task GenerateForAssetAsync(int assetId, CancellationToken cancellationToken = default);
    Task GenerateForAllAssetsAsync(CancellationToken cancellationToken = default);
}
