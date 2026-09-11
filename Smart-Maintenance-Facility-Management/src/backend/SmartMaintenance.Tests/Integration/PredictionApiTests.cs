using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Auth;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Application.Predictions;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Tests.Integration;

[Collection("Integration")]
public class PredictionApiTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public PredictionApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        factory.AiClient.Reset();
    }

    private async Task<string> LoginAsync(string username, string password)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest { Username = username, Password = password });
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts))!.Token;
    }

    private async Task<int> SeedAssetWithIotAsync()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "AI-AC-" + Guid.NewGuid().ToString("N")[..6],
            Type = AssetTypes.AirConditioner,
            Location = "A301",
            Status = AssetStatuses.Operational
        });
        var res = await _client.SendAsync(req);
        var asset = await res.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);
        var externalId = "SENSOR_" + Guid.NewGuid().ToString("N")[..8];

        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var device = new IotDevice { ExternalId = externalId };
            db.AddIotDevice(device);
            await db.SaveChangesAsync();
            db.AddIotMapping(new IotMapping { AssetId = asset!.AssetId, DeviceId = device.DeviceId, CreatedAt = DateTime.UtcNow });
            await db.SaveChangesAsync();
        }

        using var ingest = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest");
        ingest.Headers.TryAddWithoutValidation("X-Api-Key", CustomWebApplicationFactory.GatewayApiKey);
        ingest.Content = JsonContent.Create(new IotIngestRequest
        {
            DeviceId = externalId,
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 36, Humidity = 70, PowerStatus = 1 }
        });
        (await _client.SendAsync(ingest)).StatusCode.Should().Be(HttpStatusCode.Created);
        return asset!.AssetId;
    }

    [Fact]
    public async Task GenerateAndGet_ValidAi_Returns200_DoesNotCreateWorkOrder()
    {
        var assetId = await SeedAssetWithIotAsync();
        _factory.AiClient.RiskToReturn = RiskLevels.High;

        using (var scope = _factory.Services.CreateScope())
        {
            var svc = scope.ServiceProvider.GetRequiredService<IPredictionService>();
            await svc.GenerateForAssetAsync(assetId);
        }

        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{assetId}/prediction");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await res.Content.ReadFromJsonAsync<PredictionResponse>(JsonOpts);
        body!.Risk.Should().Be(RiskLevels.High);
        body.PredictedAt.Should().BeAfter(DateTime.UtcNow.AddMinutes(-5));

        using var verify = _factory.Services.CreateScope();
        var db = verify.ServiceProvider.GetRequiredService<AppDbContext>();
        var asset = await db.AssetsSet.SingleAsync(a => a.AssetId == assetId);
        asset.MaintenanceRisk.Should().Be(RiskLevels.High);
        asset.Status.Should().Be(AssetStatuses.Operational);
        (await db.WorkOrdersSet.CountAsync(w => w.AssetId == assetId)).Should().Be(0);
    }

    [Fact]
    public async Task Generate_Timeout_KeepsOldRisk()
    {
        var assetId = await SeedAssetWithIotAsync();
        _factory.AiClient.RiskToReturn = RiskLevels.Medium;
        using (var scope = _factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IPredictionService>().GenerateForAssetAsync(assetId);
        }

        _factory.AiClient.SimulateTimeout = true;
        using (var scope = _factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IPredictionService>().GenerateForAssetAsync(assetId);
        }

        using var verify = _factory.Services.CreateScope();
        var db = verify.ServiceProvider.GetRequiredService<AppDbContext>();
        var asset = await db.AssetsSet.SingleAsync(a => a.AssetId == assetId);
        asset.MaintenanceRisk.Should().Be(RiskLevels.Medium);
        (await db.AiPredictionsSet.CountAsync(p => p.AssetId == assetId)).Should().Be(1);
    }

    [Fact]
    public async Task GetPrediction_InsufficientData_Returns404()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var create = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        create.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        create.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "NoData",
            Type = AssetTypes.Light,
            Location = "L1",
            Status = AssetStatuses.Operational
        });
        var created = await _client.SendAsync(create);
        var asset = await created.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);

        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{asset!.AssetId}/prediction");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPrediction_Technician_Allowed()
    {
        var assetId = await SeedAssetWithIotAsync();
        _factory.AiClient.Reset();
        _factory.AiClient.RiskToReturn = RiskLevels.Low;
        using (var scope = _factory.Services.CreateScope())
        {
            await scope.ServiceProvider.GetRequiredService<IPredictionService>().GenerateForAssetAsync(assetId);
        }

        var token = await LoginAsync("tech1", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{assetId}/prediction");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetPrediction_Requester_Returns403()
    {
        var assetId = await SeedAssetWithIotAsync();
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{assetId}/prediction");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetPrediction_UnknownAsset_Returns404()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, "/api/assets/99999/prediction");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
