using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.Assets;

public sealed class AssetService : IAssetService
{
    private readonly IAppDbContext _db;
    private readonly ILogger<AssetService> _logger;

    public AssetService(IAppDbContext db, ILogger<AssetService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<AssetResponse> CreateAsync(CreateAssetRequest request, int facilityManagerId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new AppException("Name is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Type))
            throw new AppException("Type is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Location))
            throw new AppException("Location is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Status))
            throw new AppException("Status is required.", 400);

        var name = request.Name.Trim();
        var type = request.Type.Trim();
        var location = request.Location.Trim();
        var status = request.Status.Trim();

        if (!AssetTypes.IsValid(type))
            throw new AppException($"Asset Type is invalid. Allowed: {string.Join(", ", AssetTypes.Allowed)}.", 400);

        if (!AssetStatuses.IsValid(status))
            throw new AppException($"Status is invalid. Allowed: {string.Join(", ", AssetStatuses.Allowed)}.", 400);

        var asset = new Asset
        {
            Name = name,
            Type = type,
            Location = location,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = facilityManagerId
        };

        _db.AddAsset(asset);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-02-01 CreateAsset: FacilityManagerId={FacilityManagerId}, AssetId={AssetId}, Timestamp={Timestamp:o}",
            facilityManagerId,
            asset.AssetId,
            DateTime.UtcNow);

        return ToResponse(asset);
    }

    public Task<IReadOnlyList<AssetResponse>> ListAsync(string? location, CancellationToken cancellationToken = default)
    {
        var query = _db.Assets.AsQueryable();
        if (!string.IsNullOrWhiteSpace(location))
        {
            var loc = location.Trim();
            query = query.Where(a => a.Location.Contains(loc));
        }

        var items = query
            .OrderBy(a => a.AssetId)
            .ToList()
            .Select(ToResponse)
            .ToList();

        return Task.FromResult<IReadOnlyList<AssetResponse>>(items);
    }

    public Task<AssetResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var asset = FindAsset(id);
        return Task.FromResult(ToResponse(asset));
    }

    public async Task<AssetResponse> UpdateAsync(int id, UpdateAssetRequest request, CancellationToken cancellationToken = default)
    {
        var asset = FindAsset(id);

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new AppException("Name is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Type))
            throw new AppException("Type is required.", 400);
        if (string.IsNullOrWhiteSpace(request.Location))
            throw new AppException("Location is required.", 400);

        var type = request.Type.Trim();
        if (!AssetTypes.IsValid(type))
            throw new AppException($"Asset Type is invalid. Allowed: {string.Join(", ", AssetTypes.Allowed)}.", 400);

        asset.Name = request.Name.Trim();
        asset.Type = type;
        asset.Location = request.Location.Trim();
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("US-02-03 UpdateAsset: AssetId={AssetId}", id);
        return ToResponse(asset);
    }

    public async Task<AssetResponse> UpdateStatusAsync(int id, UpdateAssetStatusRequest request, CancellationToken cancellationToken = default)
    {
        var asset = FindAsset(id);

        if (string.IsNullOrWhiteSpace(request.Status))
            throw new AppException("Status is required.", 400);

        var status = request.Status.Trim();
        if (!AssetStatuses.IsValid(status))
            throw new AppException($"Status is invalid. Allowed: {string.Join(", ", AssetStatuses.Allowed)}.", 400);

        asset.Status = status;
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("US-02-04 UpdateAssetStatus: AssetId={AssetId}, Status={Status}", id, status);
        return ToResponse(asset);
    }

    private Asset FindAsset(int id)
    {
        if (id <= 0)
            throw new AppException("Asset id is invalid.", 400);

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == id);
        if (asset is null)
            throw new AppException("Asset not found.", 404);
        return asset;
    }

    private static AssetResponse ToResponse(Asset asset) => new()
    {
        AssetId = asset.AssetId,
        Name = asset.Name,
        Type = asset.Type,
        Location = asset.Location,
        Status = asset.Status,
        MaintenanceRisk = asset.MaintenanceRisk,
        CreatedAt = asset.CreatedAt
    };
}
