using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Iot;

public class IotServiceTests
{
    private static async Task<(IotService Sut, FakeAppDbContext Db, int AssetId, string ExternalId)> ArrangeMappedAsync()
    {
        var db = new FakeAppDbContext();
        var asset = await new AssetService(db, NullLogger<AssetService>.Instance).CreateAsync(
            new CreateAssetRequest
            {
                Name = "AC",
                Type = AssetTypes.AirConditioner,
                Location = "A301",
                Status = AssetStatuses.Operational
            }, 1);
        db.AddIotDevice(new IotDevice { ExternalId = "SENSOR_001" });
        db.AddIotMapping(new IotMapping { AssetId = asset.AssetId, DeviceId = db.DeviceList[0].DeviceId, CreatedAt = DateTime.UtcNow });
        var sut = new IotService(db, NullLogger<IotService>.Instance);
        return (sut, db, asset.AssetId, "SENSOR_001");
    }

    [Fact]
    public async Task Ingest_MissingDeviceId_Throws400()
    {
        var (sut, db, _, _) = await ArrangeMappedAsync();
        var act = async () => await sut.IngestAsync(new IotIngestRequest
        {
            DeviceId = "",
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 28.5 }
        });
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        db.DataList.Should().BeEmpty();
    }

    [Fact]
    public async Task Ingest_MissingMetrics_Throws400()
    {
        var (sut, _, _, ext) = await ArrangeMappedAsync();
        var act = async () => await sut.IngestAsync(new IotIngestRequest
        {
            DeviceId = ext,
            Timestamp = DateTime.UtcNow,
            Metrics = null
        });
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Ingest_UnknownDevice_Throws404_DoesNotPersist()
    {
        var (sut, db, _, _) = await ArrangeMappedAsync();
        var act = async () => await sut.IngestAsync(new IotIngestRequest
        {
            DeviceId = "UNKNOWN",
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 28.5 }
        });
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
        db.DataList.Should().BeEmpty();
    }

    [Fact]
    public async Task Ingest_UnmappedDevice_Throws422_DoesNotPersist()
    {
        var db = new FakeAppDbContext();
        db.AddIotDevice(new IotDevice { ExternalId = "SENSOR_ORPHAN" });
        var sut = new IotService(db, NullLogger<IotService>.Instance);

        var act = async () => await sut.IngestAsync(new IotIngestRequest
        {
            DeviceId = "SENSOR_ORPHAN",
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 28.5 }
        });
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(422);
        db.DataList.Should().BeEmpty();
    }

    [Fact]
    public async Task Ingest_ValidMapped_PersistsOneRowPerMetric()
    {
        var (sut, db, assetId, ext) = await ArrangeMappedAsync();
        var ts = DateTime.UtcNow;

        var result = await sut.IngestAsync(new IotIngestRequest
        {
            DeviceId = ext,
            Timestamp = ts,
            Metrics = new IotMetricsDto { Temperature = 28.5, Humidity = 60.2, PowerStatus = 1 }
        });

        result.AssetId.Should().Be(assetId);
        result.SavedReadings.Should().Be(3);
        db.DataList.Should().HaveCount(3);
        db.DataList.Select(d => d.MetricType).Should().BeEquivalentTo(
            [MetricTypes.Temperature, MetricTypes.Humidity, MetricTypes.PowerStatus]);
    }

    [Fact]
    public async Task GetByAsset_Unknown_Throws404()
    {
        var (sut, _, _, _) = await ArrangeMappedAsync();
        var act = async () => await sut.GetByAssetAsync(999);
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetByAsset_NoMapping_ReturnsEmpty()
    {
        var db = new FakeAppDbContext();
        var asset = await new AssetService(db, NullLogger<AssetService>.Instance).CreateAsync(
            new CreateAssetRequest
            {
                Name = "Fan",
                Type = AssetTypes.Fan,
                Location = "B1",
                Status = AssetStatuses.Operational
            }, 1);
        var sut = new IotService(db, NullLogger<IotService>.Instance);

        var data = await sut.GetByAssetAsync(asset.AssetId);
        data.Should().BeEmpty();
    }
}
