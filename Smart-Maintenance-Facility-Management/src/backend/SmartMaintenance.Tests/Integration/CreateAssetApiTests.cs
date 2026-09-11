using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Auth;
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Tests.Integration;

[Collection("Integration")]
public class CreateAssetApiTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public CreateAssetApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> LoginAsync(string username, string password)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
        {
            Username = username,
            Password = password
        });
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await res.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrWhiteSpace();
        return body.Token;
    }

    [Fact]
    public async Task PostAssets_ValidFacilityManager_Returns201_AndPersists()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "Dieu hoa Panasonic",
            Type = AssetTypes.AirConditioner,
            Location = "Phong A301",
            Status = AssetStatuses.Operational
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await res.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);
        created.Should().NotBeNull();
        created!.AssetId.Should().BeGreaterThan(0);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var row = await db.AssetsSet.SingleAsync(a => a.AssetId == created.AssetId);
        row.Name.Should().Be("Dieu hoa Panasonic");
    }

    [Fact]
    public async Task PostAssets_RequesterToken_Returns403()
    {
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "AP",
            Type = AssetTypes.WiFi,
            Location = "B201",
            Status = AssetStatuses.Operational
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostAssets_InvalidType_Returns400()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "TV",
            Type = "Tivi",
            Location = "A101",
            Status = AssetStatuses.Operational
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostAssets_Anonymous_Returns401()
    {
        var res = await _client.PostAsJsonAsync("/api/assets", new CreateAssetRequest
        {
            Name = "AP",
            Type = AssetTypes.WiFi,
            Location = "B201",
            Status = AssetStatuses.Operational
        });
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
