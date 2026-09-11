using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Predictions;

namespace SmartMaintenance.Infrastructure.Ai;

public sealed class AiPredictionClient : IAiPredictionClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AiPredictionClient> _logger;

    public AiPredictionClient(HttpClient http, IConfiguration configuration, ILogger<AiPredictionClient> logger)
    {
        _http = http;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AiPredictResult?> PredictAsync(AiPredictRequest request, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["Ai:ApiKey"] ?? string.Empty;

        for (var attempt = 1; attempt <= 2; attempt++)
        {
            try
            {
                using var message = new HttpRequestMessage(HttpMethod.Post, "predict");
                if (!string.IsNullOrWhiteSpace(apiKey))
                    message.Headers.TryAddWithoutValidation("X-Api-Key", apiKey);
                message.Content = JsonContent.Create(request);

                using var response = await _http.SendAsync(message, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        "AI service HTTP {Status} on attempt {Attempt} for AssetId={AssetId}",
                        (int)response.StatusCode,
                        attempt,
                        request.AssetId);
                    if (attempt == 1)
                        continue;
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<AiPredictResult>(cancellationToken);
            }
            catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogError("AI service timeout on attempt {Attempt} for AssetId={AssetId}", attempt, request.AssetId);
                if (attempt == 1)
                    continue;
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "AI service connection failure on attempt {Attempt} for AssetId={AssetId}", attempt, request.AssetId);
                if (attempt == 1)
                    continue;
                return null;
            }
        }

        return null;
    }
}
