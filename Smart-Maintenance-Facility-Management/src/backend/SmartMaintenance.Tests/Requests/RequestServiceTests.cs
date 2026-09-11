using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Application.Requests;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Requests;

public class RequestServiceTests
{
    private static async Task<(RequestService Sut, FakeAppDbContext Db, Asset Asset)> ArrangeAsync()
    {
        var db = new FakeAppDbContext();
        var assetService = new AssetService(db, NullLogger<AssetService>.Instance);
        var asset = await assetService.CreateAsync(new CreateAssetRequest
        {
            Name = "AC A301",
            Type = AssetTypes.AirConditioner,
            Location = "A301",
            Status = AssetStatuses.Operational
        }, facilityManagerId: 1);
        var sut = new RequestService(db, NullLogger<RequestService>.Instance);
        return (sut, db, new Asset
        {
            AssetId = asset.AssetId,
            Name = asset.Name,
            Type = asset.Type,
            Location = asset.Location,
            Status = asset.Status
        });
    }

    [Fact]
    public async Task Create_MissingAssetId_Throws400()
    {
        var (sut, db, _) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateRequestPayload
        {
            AssetId = 0,
            Description = "Loi"
        }, requesterId: 9);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        db.RequestList.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_EmptyDescription_Throws400()
    {
        var (sut, db, asset) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateRequestPayload
        {
            AssetId = asset.AssetId,
            Description = "  "
        }, requesterId: 9);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        db.RequestList.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_DescriptionTooLong_Throws400()
    {
        var (sut, _, asset) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateRequestPayload
        {
            AssetId = asset.AssetId,
            Description = new string('x', 501)
        }, requesterId: 9);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Create_UnknownAsset_Throws404()
    {
        var (sut, db, _) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateRequestPayload
        {
            AssetId = 999,
            Description = "Dieu hoa khong mat"
        }, requesterId: 9);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
        db.RequestList.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_Valid_PersistsSubmitted_WithJwtRequesterId()
    {
        var (sut, db, asset) = await ArrangeAsync();

        var result = await sut.CreateAsync(new CreateRequestPayload
        {
            AssetId = asset.AssetId,
            Description = "Dieu hoa khong mat"
        }, requesterId: 42);

        result.Status.Should().Be(RequestStatuses.Submitted);
        result.RequesterId.Should().Be(42);
        result.AssetId.Should().Be(asset.AssetId);
        db.RequestList.Should().ContainSingle();
    }
}
