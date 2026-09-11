using System.Collections.Concurrent;
using SmartMaintenance.Application.Abstractions;

namespace SmartMaintenance.Infrastructure.Auth;

public sealed class TokenBlacklist : ITokenBlacklist
{
    private readonly ConcurrentDictionary<string, DateTime> _entries = new(StringComparer.Ordinal);

    public void Blacklist(string tokenKey, DateTime expiresAtUtc)
    {
        if (string.IsNullOrWhiteSpace(tokenKey))
            return;

        _entries[tokenKey] = expiresAtUtc;
        CleanupExpired();
    }

    public bool IsBlacklisted(string tokenKey)
    {
        if (string.IsNullOrWhiteSpace(tokenKey))
            return false;

        if (!_entries.TryGetValue(tokenKey, out var expiresAt))
            return false;

        if (expiresAt <= DateTime.UtcNow)
        {
            _entries.TryRemove(tokenKey, out _);
            return false;
        }

        return true;
    }

    private void CleanupExpired()
    {
        var now = DateTime.UtcNow;
        foreach (var pair in _entries)
        {
            if (pair.Value <= now)
                _entries.TryRemove(pair.Key, out _);
        }
    }
}
