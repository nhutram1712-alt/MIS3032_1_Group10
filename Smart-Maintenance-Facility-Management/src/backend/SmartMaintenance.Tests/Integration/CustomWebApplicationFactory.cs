using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Tests.Fakes;

namespace SmartMaintenance.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestJwtSecret = "TEST_ONLY_SmartMaintenance_Jwt_Secret_Key_32chars!";
    public const string GatewayApiKey = "TEST_IOT_GATEWAY_KEY";
    public FakeAiPredictionClient AiClient { get; } = new();
    private readonly string _dbName = $"Phase8_{Guid.NewGuid():N}";

    public CustomWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("Jwt__Secret", TestJwtSecret);
        Environment.SetEnvironmentVariable("Jwt__Issuer", "SmartMaintenance");
        Environment.SetEnvironmentVariable("Jwt__Audience", "SmartMaintenance");
        Environment.SetEnvironmentVariable("Database__Provider", "InMemory");
        Environment.SetEnvironmentVariable("Iot__GatewayApiKey", GatewayApiKey);
        Environment.SetEnvironmentVariable("Ai__BackgroundJobEnabled", "false");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Database:Provider"] = "InMemory",
                ["Database:InMemoryName"] = _dbName,
                ["Jwt:Secret"] = TestJwtSecret,
                ["Jwt:Issuer"] = "SmartMaintenance",
                ["Jwt:Audience"] = "SmartMaintenance",
                ["Jwt:ExpiresMinutes"] = "60",
                ["Iot:GatewayApiKey"] = GatewayApiKey,
                ["Ai:BackgroundJobEnabled"] = "false",
                ["Ai:TimeoutSeconds"] = "2"
            });
        });

        builder.ConfigureServices(services =>
        {
            var existing = services.Where(d => d.ServiceType == typeof(IAiPredictionClient)).ToList();
            foreach (var d in existing)
                services.Remove(d);
            services.AddSingleton<IAiPredictionClient>(AiClient);

            services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "SmartMaintenance",
                    ValidAudience = "SmartMaintenance",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestJwtSecret)),
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    NameClaimType = System.Security.Claims.ClaimTypes.Name
                };
            });
        });
    }
}
