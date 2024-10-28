using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class RefreshController : AbstractFeaturedController
{
    /// <summary>
    /// Allows a user to exchange their refresh token for a new JWT and refresh token
    /// </summary>
    /// <param name="refreshToken">User's old refresh token</param>
    /// <returns><see cref="StatusCodes.Status200OK"/>(<see cref="DualToken"/>) | <see cref="StatusCodes.Status403Forbidden"/> | <see cref="StatusCodes.Status401Unauthorized"/></returns>
    [HttpPost(Name = "Refresh")]
    [ProducesResponseType(typeof(DualToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] ByteArrayToken refreshToken)
    {
        LogWriter.LogInfo("Refresh called");

        // Grab username and verify refresh token
        (bool valid, string? username) = await ServerState.TokenStore.RemoveAndVerifyRefreshToken(refreshToken.Token);

        // Ensure validity
        if (valid && username != null)
        {
            // Retrieve roles for this user
            (bool success, string[]? roles) = await ServerState.UserStore.GetRoles(username);
            if (success)
            {
                // Generate a new auth and refresh token
                (string authorization, byte[] refresh) = await ServerState.TokenStore.GenerateTokenSet(username, roles ?? Array.Empty<string>());

                // Return the tokens as a response
                return Ok(new DualToken(authorization, refresh));
            }
            else
            {
                // If credentials are invalid, return a 403 Forbid response
                return Forbid();
            }
        }

        // Return unauthorized in the event that the refresh token was invalid
        return Unauthorized();
    }
}
