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
using SmartMaintenance.Domain.Enums;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Tests.Integration;

[Collection("Integration")]
public class CreateRequestApiTests
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

    public CreateRequestApiTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<string> LoginAsync(string username, string password)
    {
        var res = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest { Username = username, Password = password });
        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        return body!.Token;
    }

    private async Task<int> CreateAssetAsManagerAsync()
    {
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/assets");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateAssetRequest
        {
            Name = "AC-" + Guid.NewGuid().ToString("N")[..6],
            Type = AssetTypes.AirConditioner,
            Location = "A301",
            Status = AssetStatuses.Operational
        });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await res.Content.ReadFromJsonAsync<AssetResponse>(JsonOpts);
        return created!.AssetId;
    }

    [Fact]
    public async Task PostRequest_ValidRequester_Returns201_Submitted()
    {
        var assetId = await CreateAssetAsManagerAsync();
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/requests");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateRequestPayload
        {
            AssetId = assetId,
            Description = "Dieu hoa khong mat"
        });

        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await res.Content.ReadFromJsonAsync<RequestResponse>(JsonOpts);
        body!.Status.Should().Be(RequestStatuses.Submitted);
        body.AssetId.Should().Be(assetId);

        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var row = await db.MaintenanceRequestsSet.SingleAsync(r => r.RequestId == body.RequestId);
        row.Description.Should().Be("Dieu hoa khong mat");
        row.RequesterId.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task PostRequest_UnknownAsset_Returns404()
    {
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/requests");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateRequestPayload { AssetId = 99999, Description = "Loi" });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PostRequest_EmptyDescription_Returns400()
    {
        var assetId = await CreateAssetAsManagerAsync();
        var token = await LoginAsync("requester", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/requests");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateRequestPayload { AssetId = assetId, Description = "" });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PostRequest_FacilityManager_Returns403()
    {
        var assetId = await CreateAssetAsManagerAsync();
        var token = await LoginAsync("manager", "Due@2026");
        using var req = new HttpRequestMessage(HttpMethod.Post, "/api/requests");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        req.Content = JsonContent.Create(new CreateRequestPayload { AssetId = assetId, Description = "Loi" });
        var res = await _client.SendAsync(req);
        res.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task PostRequest_Anonymous_Returns401()
    {
        var res = await _client.PostAsJsonAsync("/api/requests", new CreateRequestPayload { AssetId = 1, Description = "Loi" });
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
