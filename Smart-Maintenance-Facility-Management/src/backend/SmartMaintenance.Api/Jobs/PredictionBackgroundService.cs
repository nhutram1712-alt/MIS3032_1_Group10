using SmartMaintenance.Application.Abstractions;

namespace SmartMaintenance.Api.Jobs;

public sealed class PredictionBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PredictionBackgroundService> _logger;

    public PredictionBackgroundService(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<PredictionBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_configuration.GetValue("Ai:BackgroundJobEnabled", false))
        {
            _logger.LogInformation("AI prediction background job is disabled.");
            return;
        }

        var intervalHours = _configuration.GetValue("Ai:JobIntervalHours", 24);
        if (intervalHours <= 0)
            intervalHours = 24;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IPredictionService>();
                await service.GenerateForAllAssetsAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AI prediction background job failed");
            }

            try
            {
                await Task.Delay(TimeSpan.FromHours(intervalHours), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
