using SmartMaintenance.Application.Assets;

namespace SmartMaintenance.Application.Abstractions;

public interface IAssetService
{
    Task<AssetResponse> CreateAsync(CreateAssetRequest request, int facilityManagerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AssetResponse>> ListAsync(string? location, CancellationToken cancellationToken = default);
    Task<AssetResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AssetResponse> UpdateAsync(int id, UpdateAssetRequest request, CancellationToken cancellationToken = default);
    Task<AssetResponse> UpdateStatusAsync(int id, UpdateAssetStatusRequest request, CancellationToken cancellationToken = default);
}
