using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts")]
public class AuthorizeController : AbstractFeaturedController
{
    /// <summary>
    /// Authorizes a requests provided credentials agains the user database
    /// </summary>
    /// <param name="credentials"> Only username and password must be set for authorization</param>
    /// <returns>
    /// <see cref="StatusCodes.Status200OK"/>(<see cref="DualToken"/>)
    /// <see cref="StatusCodes.Status403Forbidden"/>
    /// </returns>
    /// 
    [HttpPost("authorize")]
    [ProducesResponseType(typeof(DualToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Authorize([FromBody] Credentials credentials)
    {
        LogWriter.LogInfo("Authorize called");

        // Validate user credential
        (bool success, string[]? roles) = await ServerState.UserStore.VerifyUser(credentials.Username, credentials.Password);
        if (success) 
        {
            // Generate a token set (auth & refresh) from the user's information
            (string authorizationToken, byte[] refreshToken) = await ServerState.TokenStore.GenerateTokenSet(credentials.Username, roles);

            // Return the tokens as a response
            return Ok(new DualToken(authorizationToken, refreshToken));
        }

        // If credentials are invalid, return a 403 Forbid response
        return Forbid();
    }
}
