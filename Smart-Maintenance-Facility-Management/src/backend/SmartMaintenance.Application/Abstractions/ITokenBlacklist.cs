namespace SmartMaintenance.Application.Abstractions;

public interface ITokenBlacklist
{
    void Blacklist(string tokenKey, DateTime expiresAtUtc);
    bool IsBlacklisted(string tokenKey);
}
