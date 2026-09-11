using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Application.Iot;

public sealed class IotService : IIotService
{
    private readonly IAppDbContext _db;
    private readonly ILogger<IotService> _logger;

    public IotService(IAppDbContext db, ILogger<IotService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IotIngestResponse> IngestAsync(IotIngestRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceId))
            throw new AppException("deviceId is required.", 400);
        if (request.Metrics is null)
            throw new AppException("metrics is required.", 400);

        var readings = ExtractReadings(request.Metrics);
        if (readings.Count == 0)
            throw new AppException("metrics must include at least one of: temperature, humidity, power_status.", 400);

        var externalId = request.DeviceId.Trim();
        var device = _db.IotDevices.FirstOrDefault(d => d.ExternalId == externalId);
        if (device is null)
        {
            _logger.LogWarning("US-05-03 ingest rejected: unknown deviceId={DeviceId}", externalId);
            throw new AppException("IoT Device not found.", 404);
        }

        var mapping = _db.IotMappings.FirstOrDefault(m => m.DeviceId == device.DeviceId);
        if (mapping is null)
        {
            _logger.LogWarning(
                "US-05-03 ingest rejected BR-13: deviceId={DeviceId} has no Asset mapping",
                externalId);
            throw new AppException("IoT Device is not mapped to an Asset.", 422);
        }

        // NFR-04: prefer server time; client timestamp is optional fallback only.
        var timestamp = DateTime.UtcNow;
        var saved = new List<IotData>();
        foreach (var (metric, value) in readings)
        {
            var row = new IotData
            {
                DeviceId = device.DeviceId,
                MetricType = metric,
                ReadingValue = value,
                Timestamp = timestamp
            };
            _db.AddIotData(row);
            saved.Add(row);
        }

        await _db.SaveChangesAsync(cancellationToken);

        foreach (var row in saved)
            TryCreateAlert(row, mapping.AssetId);

        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "US-05-03 ingest ok: DeviceId={DeviceId}, AssetId={AssetId}, Readings={Count}, Timestamp={Timestamp:o}",
            device.DeviceId,
            mapping.AssetId,
            readings.Count,
            timestamp);

        return new IotIngestResponse
        {
            DeviceId = device.DeviceId,
            AssetId = mapping.AssetId,
            SavedReadings = readings.Count
        };
    }

    public Task<IReadOnlyList<IotDataItem>> GetByAssetAsync(int assetId, CancellationToken cancellationToken = default)
    {
        if (assetId <= 0)
            throw new AppException("Asset id is invalid.", 400);

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
            throw new AppException("Asset not found.", 404);

        var mapping = _db.IotMappings.FirstOrDefault(m => m.AssetId == assetId);
        if (mapping is null)
        {
            _logger.LogInformation(
                "US-05-03 get iot-data: AssetId={AssetId} has no IoT mapping",
                assetId);
            return Task.FromResult<IReadOnlyList<IotDataItem>>([]);
        }

        var items = _db.IotData
            .Where(d => d.DeviceId == mapping.DeviceId)
            .OrderByDescending(d => d.Timestamp)
            .ToList()
            .Select(d => new IotDataItem
            {
                MetricType = d.MetricType,
                Value = d.ReadingValue,
                Timestamp = d.Timestamp
            })
            .ToList();

        return Task.FromResult<IReadOnlyList<IotDataItem>>(items);
    }

    public async Task<IotMappingResponse> CreateMappingAsync(
        CreateIotMappingRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.AssetId is null or <= 0)
            throw new AppException("assetId is required.", 400);

        var assetId = request.AssetId.Value;

        var asset = _db.Assets.FirstOrDefault(a => a.AssetId == assetId);
        if (asset is null)
            throw new AppException("Asset not found.", 404);

        if (_db.IotMappings.Any(m => m.AssetId == assetId))
            throw new AppException("Asset already has an IoT mapping (BR-13).", 400);

        // Device ID do hệ thống cấp (MVP) — client không nhập tay.
        var externalId = string.IsNullOrWhiteSpace(request.DeviceId)
            ? $"SENSOR_{assetId}"
            : request.DeviceId.Trim();

        var device = _db.IotDevices.FirstOrDefault(d => d.ExternalId == externalId);
        if (device is null)
        {
            device = new IotDevice
            {
                ExternalId = externalId,
                DeviceName = $"Sensor {asset.Name}",
                DeviceType = asset.Type switch
                {
                    AssetTypes.AirConditioner => "temperature_sensor",
                    AssetTypes.WiFi => "wifi_analyzer",
                    _ => "multi_sensor"
                }
            };
            _db.AddIotDevice(device);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (_db.IotMappings.Any(m => m.DeviceId == device.DeviceId))
        {
            // Trùng SENSOR_{id} hiếm khi remap — sinh mã duy nhất.
            externalId = $"SENSOR_{assetId}_{DateTime.UtcNow:yyyyMMddHHmmss}";
            device = new IotDevice
            {
                ExternalId = externalId,
                DeviceName = $"Sensor {asset.Name}",
                DeviceType = "multi_sensor"
            };
            _db.AddIotDevice(device);
            await _db.SaveChangesAsync(cancellationToken);
        }

        var mapping = new IotMapping
        {
            AssetId = assetId,
            DeviceId = device.DeviceId,
            CreatedAt = DateTime.UtcNow
        };
        _db.AddIotMapping(mapping);
        await _db.SaveChangesAsync(cancellationToken);

        return new IotMappingResponse
        {
            MappingId = mapping.MappingId,
            AssetId = asset.AssetId,
            AssetName = asset.Name,
            DeviceId = device.ExternalId,
            CreatedAt = mapping.CreatedAt
        };
    }

    public Task<IReadOnlyList<IotMappingResponse>> ListMappingsAsync(CancellationToken cancellationToken = default)
    {
        var items = (
            from m in _db.IotMappings
            join a in _db.Assets on m.AssetId equals a.AssetId
            join d in _db.IotDevices on m.DeviceId equals d.DeviceId
            orderby m.MappingId
            select new IotMappingResponse
            {
                MappingId = m.MappingId,
                AssetId = m.AssetId,
                AssetName = a.Name,
                DeviceId = d.ExternalId,
                CreatedAt = m.CreatedAt
            }).ToList();

        return Task.FromResult<IReadOnlyList<IotMappingResponse>>(items);
    }

    public async Task<IotMappingResponse> UpdateMappingAsync(
        int id,
        UpdateIotMappingRequest request,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new AppException("Mapping id is invalid.", 400);
        if (string.IsNullOrWhiteSpace(request.DeviceId))
            throw new AppException("deviceId is required.", 400);

        var mapping = _db.IotMappings.FirstOrDefault(m => m.MappingId == id);
        if (mapping is null)
            throw new AppException("IoT mapping not found.", 404);

        var externalId = request.DeviceId.Trim();
        var device = _db.IotDevices.FirstOrDefault(d => d.ExternalId == externalId);
        if (device is null)
        {
            device = new IotDevice { ExternalId = externalId };
            _db.AddIotDevice(device);
            await _db.SaveChangesAsync(cancellationToken);
        }

        if (_db.IotMappings.Any(m => m.DeviceId == device.DeviceId && m.MappingId != id))
            throw new AppException("Device is already mapped to another Asset (BR-13).", 400);

        mapping.DeviceId = device.DeviceId;
        await _db.SaveChangesAsync(cancellationToken);

        var asset = _db.Assets.First(a => a.AssetId == mapping.AssetId);
        return new IotMappingResponse
        {
            MappingId = mapping.MappingId,
            AssetId = mapping.AssetId,
            AssetName = asset.Name,
            DeviceId = device.ExternalId,
            CreatedAt = mapping.CreatedAt
        };
    }

    public Task<IReadOnlyList<IotAlertResponse>> ListAlertsAsync(int? assetId, CancellationToken cancellationToken = default)
    {
        var query = _db.IotAlerts.AsQueryable();
        if (assetId is > 0)
            query = query.Where(a => a.AssetId == assetId.Value);

        var items = (
            from alert in query
            join asset in _db.Assets on alert.AssetId equals asset.AssetId
            orderby alert.DetectedAt descending
            select new IotAlertResponse
            {
                AlertId = alert.AlertId,
                AssetId = alert.AssetId,
                AssetName = asset.Name,
                MetricType = alert.MetricType,
                ReadingValue = alert.ReadingValue,
                Threshold = alert.Threshold,
                Severity = alert.Severity,
                DetectedAt = alert.DetectedAt
            }).ToList();

        return Task.FromResult<IReadOnlyList<IotAlertResponse>>(items);
    }

    private void TryCreateAlert(IotData data, int assetId)
    {
        double? threshold = null;
        string? severity = null;

        if (string.Equals(data.MetricType, MetricTypes.Temperature, StringComparison.Ordinal) && data.ReadingValue > 40)
        {
            threshold = 40;
            severity = AlertSeverities.High;
        }
        else if (string.Equals(data.MetricType, MetricTypes.Humidity, StringComparison.Ordinal) && data.ReadingValue > 80)
        {
            threshold = 80;
            severity = AlertSeverities.Medium;
        }
        else if (string.Equals(data.MetricType, MetricTypes.PowerStatus, StringComparison.Ordinal) && data.ReadingValue == 0)
        {
            threshold = 0;
            severity = AlertSeverities.High;
        }

        if (threshold is null || severity is null)
            return;

        _db.AddIotAlert(new IotAlert
        {
            DataId = data.DataId,
            AssetId = assetId,
            MetricType = data.MetricType,
            ReadingValue = data.ReadingValue,
            Threshold = threshold.Value,
            Severity = severity,
            DetectedAt = DateTime.UtcNow
        });
    }

    private static List<(string Metric, double Value)> ExtractReadings(IotMetricsDto metrics)
    {
        var list = new List<(string, double)>();
        if (metrics.Temperature is not null)
            list.Add((MetricTypes.Temperature, metrics.Temperature.Value));
        if (metrics.Humidity is not null)
            list.Add((MetricTypes.Humidity, metrics.Humidity.Value));
        if (metrics.PowerStatus is not null)
            list.Add((MetricTypes.PowerStatus, metrics.PowerStatus.Value));
        return list;
    }
}
