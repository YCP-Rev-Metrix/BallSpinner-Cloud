using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevMetrix.BallSpinner.BackEnd.Common.POCOs;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts")]
public class AuthAndRegisterController : AbstractFeaturedController
{
    /// <summary>
    /// Authorizes a requests provided credentials agains the user database
    /// </summary>
    /// <param name="userIdentification">The user's information, only username and password must be set for authorization</param>
    /// <returns><see cref="StatusCodes.Status200OK"/>(<see cref="DualToken"/>) | <see cref="StatusCodes.Status403Forbidden"/></returns>
    [HttpPost("Authorize")]
    [ProducesResponseType(typeof(DualToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Authorize([FromBody] Credentials credentials)
    {
        LogWriter.LogInfo("Authorize called");

        // Validate user credentials (e.g., check against a database)
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

    /// <summary>
    /// Allows someone to register for a new account in the database. Will then (if account created) authorize the user and provide their refresh token and JWT
    /// </summary>
    /// <param name="userIdentification">All user identification provided by the user</param>
    /// <returns><see cref="StatusCodes.Status200OK"/>(<see cref="DualToken"/>) | <see cref="StatusCodes.Status403Forbidden"/> | <see cref="StatusCodes.Status409Conflict"/></returns>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(DualToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] UserIdentification userIdentification)
    {
        LogWriter.LogInfo("Register called");
        bool result = await ServerState.UserStore.CreateUser(userIdentification.Firstname,
                                                              userIdentification.Lastname,
                                                              userIdentification.Username,
                                                              userIdentification.Password,
                                                              userIdentification.Email,
                                                              userIdentification.PhoneNumber,
                                                              new string[] { "user" });

        return result ? await Authorize(new Credentials(userIdentification.Username,userIdentification.Password)) : Conflict();
    }
}
