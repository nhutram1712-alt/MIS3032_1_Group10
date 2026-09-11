using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SmartMaintenance.Api.Jobs;
using SmartMaintenance.Api.Middleware;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Infrastructure.DependencyInjection;
using SmartMaintenance.Infrastructure.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);

    var jwtSecret = builder.Configuration["Jwt:Secret"]
        ?? throw new InvalidOperationException("Jwt:Secret must be configured via appsettings or environment.");
    if (jwtSecret.Length < 32)
        throw new InvalidOperationException("Jwt:Secret must be at least 32 characters.");

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var blacklist = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklist>();
                    var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                    string? key = jti;
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        var raw = context.Request.Headers.Authorization.ToString();
                        if (raw.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            var token = raw["Bearer ".Length..].Trim();
                            key = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(key) && blacklist.IsBlacklisted(key))
                    {
                        context.Fail("Token has been revoked.");
                    }

                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddAuthorization();
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
    builder.Services.AddHostedService<PredictionBackgroundService>();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (string.Equals(db.Database.ProviderName, "Microsoft.EntityFrameworkCore.InMemory", StringComparison.Ordinal))
            await db.Database.EnsureCreatedAsync();
        else
            await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db);
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program;
