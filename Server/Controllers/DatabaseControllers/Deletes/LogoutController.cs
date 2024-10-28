using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class LogoutController : AbstractFeaturedController
{
    /// <summary>
    /// Logs a user out of their account by removing their refresh tokens and blacklisting their JWT
    /// </summary>
    /// <returns><see cref="StatusCodes.Status200OK"/></returns>
    [Authorize]
    [HttpDelete(Name = "Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        LogWriter.LogInfo("Logout called");
        string? username = GetUsername();
        if (username != null)
        {
            await ServerState.TokenStore.RemoveRelatedRefreshTokens(username);
        }

        string? jwt = GetJWT();
        if (jwt != null)
        {
            ServerState.TokenStore.BlacklistAuthorizationToken(jwt);
        }

        return Ok();
    }
}
