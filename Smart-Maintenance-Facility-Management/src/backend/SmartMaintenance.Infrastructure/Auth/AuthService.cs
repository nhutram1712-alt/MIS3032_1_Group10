using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Auth;
using SmartMaintenance.Infrastructure.Persistence;

namespace SmartMaintenance.Infrastructure.Auth;

public sealed class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ITokenBlacklist _tokenBlacklist;

    public AuthService(AppDbContext db, IConfiguration configuration, ITokenBlacklist tokenBlacklist)
    {
        _db = db;
        _configuration = configuration;
        _tokenBlacklist = tokenBlacklist;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.Username == request.Username.Trim() && u.IsActive,
            cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var token = CreateToken(user.UserId, user.Username, user.Role);
        return new LoginResponse
        {
            Token = token,
            Role = user.Role,
            UserId = user.UserId,
            Username = user.Username
        };
    }

    public Task LogoutAsync(string token, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Task.CompletedTask;

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            return Task.CompletedTask;

        var jwt = handler.ReadJwtToken(token);
        var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var key = string.IsNullOrWhiteSpace(jti) ? HashToken(token) : jti;
        var expires = jwt.ValidTo.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc)
            : jwt.ValidTo.ToUniversalTime();

        if (expires > DateTime.UtcNow)
            _tokenBlacklist.Blacklist(key, expires);

        return Task.CompletedTask;
    }

    private string CreateToken(int userId, string username, string role)
    {
        var secret = Environment.GetEnvironmentVariable("Jwt__Secret")
            ?? _configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException("Jwt:Secret is not configured.");
        var issuer = Environment.GetEnvironmentVariable("Jwt__Issuer")
            ?? _configuration["Jwt:Issuer"]
            ?? "SmartMaintenance";
        var audience = Environment.GetEnvironmentVariable("Jwt__Audience")
            ?? _configuration["Jwt:Audience"]
            ?? "SmartMaintenance";
        var expiresMinutes = int.TryParse(
            Environment.GetEnvironmentVariable("Jwt__ExpiresMinutes") ?? _configuration["Jwt:ExpiresMinutes"],
            out var m) ? m : 480;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jti = Guid.NewGuid().ToString("N");
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Jti, jti),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role),
            new("role", role)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashToken(string token)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }
}
