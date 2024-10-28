namespace Server.Security.Stores;

public class DatabaseTokenStore : AbstractTokenStore
{
    public readonly HashSet<string> JWTBlacklist = new();

    public DatabaseTokenStore(TimeSpan defaultAuthoriationExpiration, TimeSpan defaultRefreshExpiration, TimeSpan clockSkew) : base(defaultAuthoriationExpiration, defaultRefreshExpiration, clockSkew) { }

    public override void BlacklistAuthorizationToken(string jwt) => JWTBlacklist.Add(jwt);

    public override bool IsAuthorizationBlacklisted(string jwt) => JWTBlacklist.Contains(jwt);

    public override async Task<(bool exists, string username, DateTime? expiration)> RemoveRefreshToken(byte[] token) => await ServerState.UserDatabase.RemoveRefreshToken(token);

    public override async Task RemoveRelatedRefreshTokens(string username) => await ServerState.UserDatabase.RemoveRelatedRefreshTokens(username);

    public override async Task<bool> StoreRefreshToken(byte[] token, string username, DateTime expiration) => await ServerState.UserDatabase.AddRefreshToken(token, username, expiration);
}
