using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using SmartMaintenance.Application.Common;
using SmartMaintenance.Application.Users;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Users;

public class UserServiceTests
{
    private static UserService CreateService(FakeAppDbContext db) =>
        new(db, NullLogger<UserService>.Instance);

    private static User Seed(FakeAppDbContext db, string username, string role) =>
        db.SeedUser(new User
        {
            Username = username,
            PasswordHash = "hash",
            Role = role,
            FullName = username,
            IsActive = true
        });

    [Fact]
    public async Task Update_Requester_Throws400_QT2()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "req1", UserRoles.Requester);
        var sut = CreateService(db);

        var act = async () => await sut.UpdateAsync(target.UserId, new UpdateUserRequest { Role = UserRoles.Technician }, actorUserId: 99);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        ex.Which.Message.Should().Be("Cannot change role of a Requester account.");
    }

    [Fact]
    public async Task Update_AssignAdmin_Throws400_QT3()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "tech1", UserRoles.Technician);
        var sut = CreateService(db);

        var act = async () => await sut.UpdateAsync(target.UserId, new UpdateUserRequest { Role = UserRoles.Admin }, actorUserId: 99);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
        ex.Which.Message.Should().Contain("Cannot assign Admin role via this endpoint");
    }

    [Fact]
    public async Task Update_AdminTarget_Throws403_QT4()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "admin1", UserRoles.Admin);
        var sut = CreateService(db);

        var act = async () => await sut.UpdateAsync(target.UserId, new UpdateUserRequest { Role = UserRoles.Technician }, actorUserId: 99);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(403);
        ex.Which.Message.Should().Be("Admin accounts cannot be modified via this endpoint.");
    }

    [Fact]
    public async Task Update_AdminSelfDeactivate_Throws403_QT4()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "admin1", UserRoles.Admin);
        var sut = CreateService(db);

        var act = async () => await sut.UpdateAsync(target.UserId, new UpdateUserRequest { IsActive = false }, actorUserId: target.UserId);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task Update_TechnicianToFacilityManager_Ok_QT1()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "tech1", UserRoles.Technician);
        var sut = CreateService(db);

        var result = await sut.UpdateAsync(target.UserId, new UpdateUserRequest { Role = UserRoles.FacilityManager }, actorUserId: 99);

        result.Role.Should().Be(UserRoles.FacilityManager);
        target.Role.Should().Be(UserRoles.FacilityManager);
    }

    [Fact]
    public async Task Update_FacilityManagerToTechnician_Ok_QT1()
    {
        var db = new FakeAppDbContext();
        var target = Seed(db, "fm1", UserRoles.FacilityManager);
        var sut = CreateService(db);

        var result = await sut.UpdateAsync(target.UserId, new UpdateUserRequest { Role = UserRoles.Technician }, actorUserId: 99);

        result.Role.Should().Be(UserRoles.Technician);
    }

    [Fact]
    public async Task Update_MissingUser_Throws404()
    {
        var db = new FakeAppDbContext();
        var sut = CreateService(db);

        var act = async () => await sut.UpdateAsync(999, new UpdateUserRequest { Role = UserRoles.Technician }, actorUserId: 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task Create_DuplicateUsername_Throws400()
    {
        var db = new FakeAppDbContext();
        Seed(db, "tech1", UserRoles.Technician);
        var sut = CreateService(db);

        var act = async () => await sut.CreateAsync(new CreateUserRequest
        {
            Username = "tech1",
            Password = "P@ssw0rd",
            Role = UserRoles.Technician
        }, actorUserId: 1);

        var ex = await act.Should().ThrowAsync<AppException>();
        ex.Which.StatusCode.Should().Be(400);
    }
}
