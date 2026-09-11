using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Application.Requests;
using SmartMaintenance.Application.WorkOrders;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.WorkOrders;

public class WorkOrderServiceTests
{
    private static async Task<(WorkOrderService Sut, FakeAppDbContext Db, int AssetId, int RequestId, int TechId)> ArrangeAsync()
    {
        var db = new FakeAppDbContext();
        var tech = db.SeedUser(new User
        {
            Username = "tech",
            PasswordHash = "x",
            Role = UserRoles.Technician,
            FullName = "Tech",
            IsActive = true
        });
        db.SeedUser(new User
        {
            Username = "req",
            PasswordHash = "x",
            Role = UserRoles.Requester,
            FullName = "Req",
            IsActive = true
        });

        var asset = await new AssetService(db, NullLogger<AssetService>.Instance).CreateAsync(
            new CreateAssetRequest
            {
                Name = "AC",
                Type = AssetTypes.AirConditioner,
                Location = "A301",
                Status = AssetStatuses.Operational
            }, 1);

        var request = await new RequestService(db, NullLogger<RequestService>.Instance).CreateAsync(
            new CreateRequestPayload { AssetId = asset.AssetId, Description = "Hong" }, 9);

        var sut = new WorkOrderService(db, NullLogger<WorkOrderService>.Instance);
        return (sut, db, asset.AssetId, request.RequestId, tech.UserId);
    }

    [Fact]
    public async Task Create_MissingTechnician_Throws400()
    {
        var (sut, _, assetId, requestId, _) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = null,
            AssetId = assetId
        }, 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Create_TechnicianIsRequester_Throws400()
    {
        var (sut, db, assetId, requestId, _) = await ArrangeAsync();
        var requester = db.UserList.Single(u => u.Role == UserRoles.Requester);

        var act = async () => await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = requester.UserId,
            AssetId = assetId
        }, 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        db.WorkOrderList.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_UnknownRequest_Throws404()
    {
        var (sut, _, assetId, _, techId) = await ArrangeAsync();

        var act = async () => await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = 999,
            TechnicianId = techId,
            AssetId = assetId
        }, 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Create_UnknownAsset_Throws404()
    {
        var (sut, db, _, requestId, techId) = await ArrangeAsync();
        db.RequestList[0].AssetId = 888;

        var act = async () => await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = techId,
            AssetId = 888
        }, 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Create_DuplicateWorkOrder_Throws409()
    {
        var (sut, _, assetId, requestId, techId) = await ArrangeAsync();
        var payload = new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = techId,
            AssetId = assetId
        };
        await sut.CreateAsync(payload, 1);

        var act = async () => await sut.CreateAsync(payload, 1);
        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(409);
    }

    [Fact]
    public async Task Create_RequestWithoutAsset_Throws422()
    {
        var (sut, db, assetId, _, techId) = await ArrangeAsync();
        db.AddMaintenanceRequest(new MaintenanceRequest
        {
            RequesterId = 9,
            AssetId = null,
            Description = "Khu vuc A",
            Status = RequestStatuses.Submitted,
            CreatedAt = DateTime.UtcNow
        });
        var areaRequestId = db.RequestList.Last().RequestId;

        var act = async () => await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = areaRequestId,
            TechnicianId = techId,
            AssetId = assetId
        }, 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(422);
    }

    [Fact]
    public async Task Create_Valid_PersistsAssigned()
    {
        var (sut, db, assetId, requestId, techId) = await ArrangeAsync();

        var result = await sut.CreateAsync(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = techId,
            AssetId = assetId
        }, facilityManagerId: 3);

        result.Status.Should().Be(WorkOrderStatuses.Assigned);
        result.TechnicianId.Should().Be(techId);
        result.AssetId.Should().Be(assetId);
        db.WorkOrderList.Should().ContainSingle();
    }
}
