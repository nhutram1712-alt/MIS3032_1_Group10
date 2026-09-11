using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Auth;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Domain.Entities;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Tests.Integration;

[Collection("Integration")]
public class IotApiTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public IotApiTests(CustomWebApplicationFactory factory)
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

    private async Task<(int AssetId, string ExternalId)> SeedMappedAssetAsync()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "IOT-AC-" + Guid.NewGuid().ToString("N")[..6],
            Type = AssetTypes.AirConditioner,
            Location = "A301",
            Status = AssetStatuses.Operational
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var asset = await res.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);
        var externalId = "SENSOR_" + Guid.NewGuid().ToString("N")[..8];

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var device = new IotDevice { ExternalId = externalId };
        db.AddIotDevice(device);
        await db.SaveChangesAsync();
        db.AddIotMapping(new IotMapping { AssetId = asset!.AssetId, DeviceId = device.DeviceId, CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();
        return (asset.AssetId, externalId);
    }

    [Fact]
    public async Task Ingest_MappedDevice_Returns201_AndPersists()
    {
        var (assetId, externalId) = await SeedMappedAssetAsync();
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest");
        req.Headers.TryAddWithoutValidation("X-Api-Key", CustomWebApplicationFactory.GatewayApiKey);
        req.Content = JsonContent.Create(new IotIngestRequest
        {
            DeviceId = externalId,
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 28.5, Humidity = 60.2, PowerStatus = 1 }
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var mapping = await db.IotMappingsSet.SingleAsync(m => m.AssetId == assetId);
        (await db.IotDataSet.CountAsync(d => d.DeviceId == mapping.DeviceId)).Should().Be(3);
    }

    [Fact]
    public async Task Ingest_UnmappedDevice_DoesNotPersist()
    {
        var externalId = "ORPHAN_" + Guid.NewGuid().ToString("N")[..6];
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.AddIotDevice(new IotDevice { ExternalId = externalId });
            await db.SaveChangesAsync();
        }

        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest");
        req.Headers.TryAddWithoutValidation("X-Api-Key", CustomWebApplicationFactory.GatewayApiKey);
        req.Content = JsonContent.Create(new IotIngestRequest
        {
            DeviceId = externalId,
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 28.5 }
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);

        using var verify = _factory.Services.CreateScope();
        var vdb = verify.ServiceProvider.GetRequiredService<AppDbContext>();
        var device = await vdb.IotDevicesSet.SingleAsync(d => d.ExternalId == externalId);
        (await vdb.IotDataSet.CountAsync(d => d.DeviceId == device.DeviceId)).Should().Be(0);
    }

    [Fact]
    public async Task Ingest_InvalidPayload_Returns400()
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest");
        req.Headers.TryAddWithoutValidation("X-Api-Key", CustomWebApplicationFactory.GatewayApiKey);
        req.Content = JsonContent.Create(new IotIngestRequest { DeviceId = "X" });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Ingest_BadApiKey_Returns401()
    {
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest");
        req.Headers.TryAddWithoutValidation("X-Api-Key", "wrong");
        req.Content = JsonContent.Create(new IotIngestRequest
        {
            DeviceId = "SENSOR_001",
            Timestamp = DateTime.UtcNow,
            Metrics = new IotMetricsDto { Temperature = 1 }
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetIotData_Manager_Returns200()
    {
        var (assetId, externalId) = await SeedMappedAssetAsync();
        using (var ingest = new HttpRequestMessage(HttpMethod.Post, "/api/iot/ingest"))
        {
            ingest.Headers.TryAddWithoutValidation("X-Api-Key", CustomWebApplicationFactory.GatewayApiKey);
            ingest.Content = JsonContent.Create(new IotIngestRequest
            {
                DeviceId = externalId,
                Timestamp = DateTime.UtcNow,
                Metrics = new IotMetricsDto { Temperature = 28.5 }
            });
            (await _client.SendAsync(ingest)).StatusCode.Should().Be(HttpStatusCode.Created);
        }

        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{assetId}/iot-data");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await res.Content.ReadFromJsonAsync<List<IotDataItem>>(JsonOpts);
        data.Should().ContainSingle(d => d.MetricType == MetricTypes.Temperature && d.Value == 28.5);
    }

    [Fact]
    public async Task GetIotData_UnmappedAsset_Returns200Empty()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var create = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        create.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        create.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "NoMap",
            Type = AssetTypes.Fan,
            Location = "C1",
            Status = AssetStatuses.Operational
        });
        var created = await _client.SendAsync(create);
        var asset = await created.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);

        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{asset!.AssetId}/iot-data");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await res.Content.ReadFromJsonAsync<List<IotDataItem>>(JsonOpts);
        data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetIotData_UnknownAsset_Returns404()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, "/api/assets/99999/iot-data");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetIotData_Requester_Returns403()
    {
        var (assetId, _) = await SeedMappedAssetAsync();
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Get, $"/api/assets/{assetId}/iot-data");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
