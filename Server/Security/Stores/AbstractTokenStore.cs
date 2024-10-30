using Common.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Server.Security.Stores;

/// <summary>
/// Object to generate, retrieve, renew, verify, and destroy tokens relating to security (JWT & refresh)
/// </summary>
public abstract class AbstractTokenStore
{
    private readonly TimeSpan _defaultAuthorizationExpiration;
    private readonly TimeSpan _defaultRefreshExpiration;
    private readonly TimeSpan _clockSkew;

    public AbstractTokenStore(TimeSpan defaultAuthoriationExpiration, TimeSpan defaultRefreshExpiration, TimeSpan clockSkew)
    {
        _defaultAuthorizationExpiration = defaultAuthoriationExpiration;
        _defaultRefreshExpiration = defaultRefreshExpiration;
        _clockSkew = clockSkew;
    }

    public string GenerateAuthorizationToken(string? username, string[]? roles) => GenerateAuthorizationToken(username, roles, _defaultAuthorizationExpiration);
    public string GenerateAuthorizationToken(string? username, string[]? roles, TimeSpan expiration)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username), // Subject (typically the user's identifier)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token identifier
            new Claim(JwtRegisteredClaimNames.Iss, Config.AuthIssuer), // Issuer
            new Claim(JwtRegisteredClaimNames.Aud, Config.AuthAudience), // Audience
        };

        if (roles != null)
        {
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.Add(expiration), // Token expiration time

            SigningCredentials = ServerState.SecurityHandler.AuthorizationSigningCredentials
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        LogWriter.LogInfo("Generated new authorization token");

        return tokenHandler.WriteToken(token);
    }

    public async Task<byte[]> GenerateRefreshToken(string? username) => await GenerateRefreshToken(username, _defaultRefreshExpiration);
    public async Task<byte[]> GenerateRefreshToken(string? username, TimeSpan expiration)
    {
        byte[] token = ServerState.SecurityHandler.GenerateRefreshToken();

        _ = await StoreRefreshToken(token, username, DateTime.UtcNow.Add(expiration));

        LogWriter.LogInfo("Generated new refresh token");

        return token;
    }

    public async Task<(string authorizationToken, byte[] refreshToken)> GenerateTokenSet(string? username, string[] roles) => (GenerateAuthorizationToken(username, roles), await GenerateRefreshToken(username));

    public async Task<(bool verified, string? username)> RemoveAndVerifyRefreshToken(byte[] token)
    {
        (bool exists, string username, DateTime? expiration) = await RemoveRefreshToken(token);

        if (exists)
        {
            if (expiration >= DateTime.UtcNow.Add(_clockSkew))
            {
                return (true, username);
            }
        }

        return (false, null);
    }

    public abstract Task<(bool exists, string username, DateTime? expiration)> RemoveRefreshToken(byte[] token);

    public abstract Task RemoveRelatedRefreshTokens(string username);

    public abstract Task<bool> StoreRefreshToken(byte[] token, string? username, DateTime expiration);

    public abstract void BlacklistAuthorizationToken(string jwt);
    public abstract bool IsAuthorizationBlacklisted(string jwt);
}
