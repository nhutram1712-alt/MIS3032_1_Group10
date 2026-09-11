using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Auth;
using SmartMaintenance.Application.Requests;
using SmartMaintenance.Application.WorkOrders;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Tests.Integration;

[Collection("Integration")]
public class CreateWorkOrderApiTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public CreateWorkOrderApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> LoginAsync(string username, string password)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest { Username = username, Password = password });
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts))!.Token;
    }

    private async Task<(int AssetId, int RequestId, int TechId)> SeedHappyPathAsync()
    {
        var managerToken = await LoginAsync("manager", "Due@2026");
        using var assetReq = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        assetReq.Headers.Authorization = new AuthenticationHeaderValue("Bearer", managerToken);
        assetReq.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "WO-AC-" + Guid.NewGuid().ToString("N")[..6],
            Type = AssetTypes.AirConditioner,
            Location = "A301",
            Status = AssetStatuses.Operational
        });
        var assetRes = await _client.SendAsync(assetReq);
        assetRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var asset = await assetRes.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);

        var requesterToken = await LoginAsync("requester", "Due@2026");
        using var reqMsg = new HttpRequestMessage(HttpMethod.Post, "/api/requests");
        reqMsg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", requesterToken);
        reqMsg.Content = JsonContent.Create(new CreateRequestPayload
        {
            AssetId = asset!.AssetId,
            Description = "Dieu hoa keu to"
        });
        var reqRes = await _client.SendAsync(reqMsg);
        reqRes.StatusCode.Should().Be(HttpStatusCode.Created);
        var request = await reqRes.Content.ReadFromJsonAsync<RequestResponse>(JsonOpts);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var techId = await db.Users.Where(u => u.Username == "tech1").Select(u => u.UserId).SingleAsync();
        return (asset.AssetId, request!.RequestId, techId);
    }

    [Fact]
    public async Task PostWorkOrder_ValidManager_Returns201_Assigned()
    {
        var (assetId, requestId, techId) = await SeedHappyPathAsync();
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = techId,
            AssetId = assetId
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await res.Content.ReadFromJsonAsync<WorkOrderResponse>(JsonOpts);
        body!.Status.Should().Be(WorkOrderStatuses.Assigned);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        (await db.WorkOrdersSet.CountAsync(w => w.RequestId == requestId)).Should().Be(1);
    }

    [Fact]
    public async Task PostWorkOrder_SecondTime_Returns409()
    {
        var (assetId, requestId, techId) = await SeedHappyPathAsync();
        var token = await LoginAsync("manager", "Due@2026");
        async Task<HttpResponseMessage> PostAsync()
        {
            using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            req.Content = JsonContent.Create(new CreateWorkOrderPayload
            {
                RequestId = requestId,
                TechnicianId = techId,
                AssetId = assetId
            });
            return await _client.SendAsync(req);
        }

        (await PostAsync()).StatusCode.Should().Be(HttpStatusCode.Created);
        (await PostAsync()).StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task PostWorkOrder_RequestWithoutAsset_Returns422()
    {
        var (_, _, techId) = await SeedHappyPathAsync();
        int areaRequestId;
        int dummyAssetId;
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dummyAssetId = await db.AssetsSet.Select(a => a.AssetId).FirstAsync();
            var entity = new MaintenanceRequest
            {
                RequesterId = await db.Users.Where(u => u.Username == "requester").Select(u => u.UserId).SingleAsync(),
                AssetId = null,
                Description = "Khu vuc tang 3",
                Status = RequestStatuses.Submitted,
                CreatedAt = DateTime.UtcNow
            };
            db.AddMaintenanceRequest(entity);
            await db.SaveChangesAsync();
            areaRequestId = entity.RequestId;
        }

        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateWorkOrderPayload
        {
            RequestId = areaRequestId,
            TechnicianId = techId,
            AssetId = dummyAssetId
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task PostWorkOrder_UnknownRequest_Returns404()
    {
        var (assetId, _, techId) = await SeedHappyPathAsync();
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateWorkOrderPayload
        {
            RequestId = 99999,
            TechnicianId = techId,
            AssetId = assetId
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostWorkOrder_Requester_Returns403()
    {
        var (assetId, requestId, techId) = await SeedHappyPathAsync();
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateWorkOrderPayload
        {
            RequestId = requestId,
            TechnicianId = techId,
            AssetId = assetId
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostWorkOrder_Anonymous_Returns401()
    {
        var res = await _client.PostAsJsonAsync("/api/work-orders", new CreateWorkOrderPayload
        {
            RequestId = 1,
            TechnicianId = 1,
            AssetId = 1
        });
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task PostWorkOrder_MissingFields_Returns400()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/work-orders");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateWorkOrderPayload());
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
