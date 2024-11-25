using DatabaseCore.DatabaseComponents;
using Server.Security.Handlers;
using Server.Security.Stores;

namespace Server;

/// <summary>
/// Provides a static server state to reference against. Items such as the following:
/// <list type="bullet">
/// <item><see cref="TokenStore"/></item>
/// </list>
/// </summary>
public static class ServerState
{
    /// <summary>
    /// Provides a basic <see cref="SecurityHandler"/> in order to handle tasks like byte[] generation or password management
    /// </summary>
    public static readonly SecurityHandler SecurityHandler = new();

    /// <summary>
    /// Handles interactions with the User Database
    /// </summary>
    public static readonly RevMetrixDb UserDatabase = new();

    /// <summary>
    /// Provides functionality surrounding JWTs and refresh tokens
    /// </summary>
    public static readonly AbstractTokenStore TokenStore = new DatabaseTokenStore(TimeSpan.FromMinutes(60), TimeSpan.FromHours(24), TimeSpan.FromMinutes(5));

    /// <summary>
    /// Provides functionality surrounding Users such as role management & user authentication
    /// </summary>
    public static readonly AbstractUserStore UserStore = new DatabaseUserStore();
}
