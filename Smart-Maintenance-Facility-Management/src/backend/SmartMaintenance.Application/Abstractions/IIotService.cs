using SmartMaintenance.Application.Iot;

namespace SmartMaintenance.Application.Abstractions;

public interface IIotService
{
    Task<IotIngestResponse> IngestAsync(IotIngestRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IotDataItem>> GetByAssetAsync(int assetId, CancellationToken cancellationToken = default);
    Task<IotMappingResponse> CreateMappingAsync(CreateIotMappingRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IotMappingResponse>> ListMappingsAsync(CancellationToken cancellationToken = default);
    Task<IotMappingResponse> UpdateMappingAsync(int id, UpdateIotMappingRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<IotAlertResponse>> ListAlertsAsync(int? assetId, CancellationToken cancellationToken = default);
}
