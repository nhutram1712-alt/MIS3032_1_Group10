using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.Predictions;

public sealed class PredictionService : IPredictionService
{
    public const int HorizonDays = 7;
    public static readonly TimeSpan StaleAfter = TimeSpan.FromHours(24);

    private readonly IAppDbContext _db;
    private readonly IAiPredictionClient _aiClient;
    private readonly ILogger<PredictionService> _logger;

    public PredictionService(IAppDbContext db, IAiPredictionClient aiClient, ILogger<PredictionService> logger)
    {
        _db = db;
        _aiClient = aiClient;
        _logger = logger;
    }

    public Task<PredictionResponse> GetLatestAsync(int assetId, CancellationToken cancellationToken = default)
    {
        if (assetId <= 0)
            throw new AppException("Asset id is invalid.", 400);

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
            throw new AppException("Asset not found.", 404);

        var latest = _db.AiPredictions
            .Where(p => p.AssetId == assetId)
            .OrderByDescending(p => p.PredictedAt)
            .FirstOrDefault();

        if (latest is null)
        {
            var mapping = _db.IotMappings.FirstOrDefault(m => m.AssetId == assetId);
            var hasIot = mapping is not null && _db.IotData.Any(d => d.DeviceId == mapping.DeviceId);
            var message = hasIot
                ? "No prediction found."
                : "Chưa đủ dữ liệu để dự đoán";
            throw new AppException(message, 404);
        }

        return Task.FromResult(ToResponse(latest, asset));
    }

    public Task<IReadOnlyList<PredictionResponse>> ListAllAsync(string? sort = null, CancellationToken cancellationToken = default)
    {
        var latestByAsset = _db.AiPredictions
            .ToList()
            .GroupBy(p => p.AssetId)
            .Select(g => g.OrderByDescending(p => p.PredictedAt).First())
            .ToList();

        var assets = _db.Assets.ToList().ToDictionary(a => a.AssetId);
        var items = latestByAsset
            .Where(p => assets.ContainsKey(p.AssetId))
            .Select(p => ToResponse(p, assets[p.AssetId]))
            .ToList();

        if (string.Equals(sort, "risk_desc", StringComparison.OrdinalIgnoreCase))
        {
            items = items
                .OrderByDescending(p => RiskRank(p.Risk))
                .ThenBy(p => p.AssetId)
                .ToList();
        }
        else
        {
            items = items.OrderBy(p => p.AssetId).ToList();
        }

        return Task.FromResult<IReadOnlyList<PredictionResponse>>(items);
    }

    public async Task GenerateForAllAssetsAsync(CancellationToken cancellationToken = default)
    {
        var mappedAssetIds = _db.IotMappings.Select(m => m.AssetId).Distinct().ToList();
        foreach (var assetId in mappedAssetIds)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await GenerateForAssetAsync(assetId, cancellationToken);
        }
    }

    public async Task GenerateForAssetAsync(int assetId, CancellationToken cancellationToken = default)
    {
        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
        {
            _logger.LogWarning("US-06-01 skip: AssetId={AssetId} not found", assetId);
            return;
        }

        var mapping = _db.IotMappings.FirstOrDefault(m => m.AssetId == assetId);
        if (mapping is null)
        {
            _logger.LogWarning("US-06-01 insufficient data: AssetId={AssetId} has no IoT mapping", assetId);
            return;
        }

        var readings = _db.IotData
            .Where(d => d.DeviceId == mapping.DeviceId)
            .OrderByDescending(d => d.Timestamp)
            .Take(288)
            .ToList()
            .Select(d => new AiIotReading
            {
                MetricType = d.MetricType,
                Value = d.ReadingValue,
                Timestamp = d.Timestamp
            })
            .ToList();

        if (readings.Count == 0)
        {
            _logger.LogWarning("US-06-01 insufficient data: AssetId={AssetId} has no IoT readings", assetId);
            return;
        }

        var historyCount = _db.MaintenanceHistories.Count(h => h.AssetId == assetId);

        AiPredictResult? result;
        try
        {
            result = await _aiClient.PredictAsync(new AiPredictRequest
            {
                AssetId = assetId,
                HorizonDays = HorizonDays,
                IotReadings = readings,
                HistoryCount = historyCount
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "US-06-01 AI service error: AssetId={AssetId}", assetId);
            return;
        }

        if (result is null || string.IsNullOrWhiteSpace(result.Risk))
        {
            _logger.LogError("US-06-01 AI service returned empty result: AssetId={AssetId}", assetId);
            return;
        }

        var risk = result.Risk.Trim();
        if (!RiskLevels.IsValid(risk))
        {
            _logger.LogError(
                "US-06-01 rejected invalid risk from AI: AssetId={AssetId}, Risk={Risk}",
                assetId,
                risk);
            return;
        }

        var predictedAt = DateTime.UtcNow;
        _db.AddAiPrediction(new AiPrediction
        {
            AssetId = assetId,
            RiskLevel = risk,
            PredictedAt = predictedAt,
            BasedOnSampleData = result.BasedOnSampleData
        });
        asset.MaintenanceRisk = risk;
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-06-01 prediction saved: AssetId={AssetId}, Risk={Risk}, PredictedAt={PredictedAt:o}, HorizonDays={HorizonDays}, BasedOnSampleData={BasedOnSampleData}",
            assetId,
            risk,
            predictedAt,
            HorizonDays,
            result.BasedOnSampleData);

        if (string.Equals(risk, RiskLevels.High, StringComparison.Ordinal))
        {
            _logger.LogWarning(
                "Notification: Facility Manager — AssetId={AssetId} Maintenance Risk=High (decision support only; no Work Order auto-created)",
                assetId);
        }
    }

    private static PredictionResponse ToResponse(AiPrediction prediction, Asset asset)
    {
        var location = asset.Location;
        return new PredictionResponse
        {
            AssetId = prediction.AssetId,
            AssetName = asset.Name,
            AssetType = asset.Type,
            AssetLocation = location,
            Location = location,
            Risk = prediction.RiskLevel,
            PredictedAt = prediction.PredictedAt,
            HorizonDays = HorizonDays,
            Stale = DateTime.UtcNow - prediction.PredictedAt > StaleAfter,
            BasedOnSampleData = prediction.BasedOnSampleData
        };
    }

    private static int RiskRank(string risk) => risk switch
    {
        RiskLevels.High => 3,
        RiskLevels.Medium => 2,
        RiskLevels.Low => 1,
        _ => 0
    };
}
