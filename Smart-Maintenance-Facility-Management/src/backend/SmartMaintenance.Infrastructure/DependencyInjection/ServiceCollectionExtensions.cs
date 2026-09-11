using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Application.Predictions;
using SmartMaintenance.Application.Requests;
using SmartMaintenance.Application.Users;
using SmartMaintenance.Application.WorkOrders;
using SmartMaintenance.Infrastructure.Ai;
using SmartMaintenance.Infrastructure.Auth;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemory = string.Equals(configuration["Database:Provider"], "InMemory", StringComparison.OrdinalIgnoreCase);
        if (useInMemory)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(configuration["Database:InMemoryName"] ?? "SmartMaintenanceTests"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }

        services.AddSingleton<ITokenBlacklist, TokenBlacklist>();
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRequestService, RequestService>();
        services.AddScoped<IWorkOrderService, WorkOrderService>();
        services.AddScoped<IIotService, IotService>();
        services.AddScoped<IPredictionService, PredictionService>();
        services.AddScoped<IUserService, UserService>();

        var aiBaseUrl = configuration["Ai:BaseUrl"] ?? "http://localhost:8000";
        var timeoutSeconds = int.TryParse(configuration["Ai:TimeoutSeconds"], out var t) ? t : 5;
        services.AddHttpClient<IAiPredictionClient, AiPredictionClient>(client =>
        {
            client.BaseAddress = new Uri(aiBaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        });

        return services;
    }
}
