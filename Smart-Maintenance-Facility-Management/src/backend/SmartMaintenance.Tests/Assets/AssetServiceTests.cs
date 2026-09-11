using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Assets;

public class AssetServiceTests
{
    private static AssetService CreateService(FakeAppDbContext db) =>
        new(db, NullLogger<AssetService>.Instance);

    [Fact]
    public async Task Create_InvalidType_Tivi_Throws400()
    {
        var db = new FakeAppDbContext();
        var sut = CreateService(db);

        var act = async () => await sut.CreateAsync(new CreateAssetRequest
        {
            Name = "TV",
            Type = "Tivi",
            Location = "A101",
            Status = AssetStatuses.Operational
        }, facilityManagerId: 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        db.AssetList.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_MissingLocation_Throws400()
    {
        var db = new FakeAppDbContext();
        var sut = CreateService(db);

        var act = async () => await sut.CreateAsync(new CreateAssetRequest
        {
            Name = "AC",
            Type = AssetTypes.AirConditioner,
            Location = "  ",
            Status = AssetStatuses.Operational
        }, facilityManagerId: 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Create_MissingStatus_Throws400()
    {
        var db = new FakeAppDbContext();
        var sut = CreateService(db);

        var act = async () => await sut.CreateAsync(new CreateAssetRequest
        {
            Name = "AC",
            Type = AssetTypes.AirConditioner,
            Location = "A101",
            Status = null
        }, facilityManagerId: 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Create_ValidPayload_PersistsAndReturnsAsset()
    {
        var db = new FakeAppDbContext();
        var sut = CreateService(db);

        var result = await sut.CreateAsync(new CreateAssetRequest
        {
            Name = "Dieu hoa Panasonic",
            Type = AssetTypes.AirConditioner,
            Location = "Phong A301",
            Status = AssetStatuses.Operational
        }, facilityManagerId: 7);

        result.AssetId.Should().BeGreaterThan(0);
        result.Type.Should().Be(AssetTypes.AirConditioner);
        result.Status.Should().Be(AssetStatuses.Operational);
        db.AssetList.Should().ContainSingle();
        db.AssetList[0].CreatedByUserId.Should().Be(7);
    }
}
