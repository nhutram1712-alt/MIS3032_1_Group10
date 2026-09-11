using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Application.Predictions;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Predictions;

public class PredictionServiceTests
{
    private static async Task<(PredictionService Sut, FakeAppDbContext Db, FakeAiPredictionClient Ai, int AssetId)> ArrangeWithIotAsync()
    {
        var db = new FakeAppDbContext();
        var ai = new FakeAiPredictionClient { RiskToReturn = RiskLevels.High };
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
        await new IotService(db, NullLogger<IotService>.Instance).IngestAsync(new IotIngestRequest
        {
            DeviceId = "SENSOR_001",
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 36, PowerStatus = 1 }
        });
        var sut = new PredictionService(db, ai, NullLogger<PredictionService>.Instance);
        return (sut, db, ai, asset.AssetId);
    }

    [Fact]
    public async Task Generate_InvalidRisk_DoesNotPersist()
    {
        var (sut, db, ai, assetId) = await ArrangeWithIotAsync();
        ai.SimulateInvalidRisk = true;

        await sut.GenerateForAssetAsync(assetId);

        db.PredictionList.Should().BeEmpty();
        db.AssetList[0].MaintenanceRisk.Should().BeNull();
        db.AssetList[0].Status.Should().Be(AssetStatuses.Operational);
        db.WorkOrderList.Should().BeEmpty();
    }

    [Fact]
    public async Task Generate_Timeout_DoesNotOverwriteExistingRisk()
    {
        var (sut, db, ai, assetId) = await ArrangeWithIotAsync();
        await sut.GenerateForAssetAsync(assetId);
        db.AssetList[0].MaintenanceRisk.Should().Be(RiskLevels.High);

        ai.SimulateTimeout = true;
        ai.RiskToReturn = RiskLevels.Low;
        await sut.GenerateForAssetAsync(assetId);

        db.AssetList[0].MaintenanceRisk.Should().Be(RiskLevels.High);
        db.PredictionList.Should().ContainSingle();
        db.WorkOrderList.Should().BeEmpty();
    }

    [Fact]
    public async Task Generate_Valid_SavesPredictionLinkedToAsset_DoesNotCreateWorkOrder_DoesNotChangeStatus()
    {
        var (sut, db, _, assetId) = await ArrangeWithIotAsync();
        var statusBefore = db.AssetList[0].Status;

        await sut.GenerateForAssetAsync(assetId);

        db.PredictionList.Should().ContainSingle();
        db.PredictionList[0].AssetId.Should().Be(assetId);
        db.PredictionList[0].RiskLevel.Should().Be(RiskLevels.High);
        db.PredictionList[0].PredictedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        db.AssetList[0].MaintenanceRisk.Should().Be(RiskLevels.High);
        db.AssetList[0].Status.Should().Be(statusBefore);
        db.WorkOrderList.Should().BeEmpty();

        var latest = await sut.GetLatestAsync(assetId);
        latest.Risk.Should().Be(RiskLevels.High);
        latest.HorizonDays.Should().Be(7);
    }

    [Fact]
    public async Task Generate_NoIotData_DoesNotSave()
    {
        var db = new FakeAppDbContext();
        var ai = new FakeAiPredictionClient { RiskToReturn = RiskLevels.Low };
        var asset = await new AssetService(db, NullLogger<AssetService>.Instance).CreateAsync(
            new CreateAssetRequest
            {
                Name = "Fan",
                Type = AssetTypes.Fan,
                Location = "B1",
                Status = AssetStatuses.Operational
            }, 1);
        var sut = new PredictionService(db, ai, NullLogger<PredictionService>.Instance);

        await sut.GenerateForAssetAsync(asset.AssetId);

        ai.CallCount.Should().Be(0);
        db.PredictionList.Should().BeEmpty();

        var act = async () => await sut.GetLatestAsync(asset.AssetId);
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
        ex.Which.Message.Should().Contain("Chưa đủ dữ liệu");
    }
}
