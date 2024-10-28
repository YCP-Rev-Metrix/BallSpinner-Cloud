using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.APIControllers;

/// <summary>
/// Provides base functionality for many controllers - will be added to in the future
/// </summary>
[Controller]
[Tags("Gets")]
public abstract class AbstractFeaturedController : ControllerBase
{
    /// <summary>
    /// Retrieves the username of the user if set in the User Claims. This should only be called in methods using authorization
    /// </summary>
    /// <returns>Currently logged in user's username if available</returns>
    [NonAction]
    public string? GetUsername() => HttpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

    /// <summary>
    /// Retrieves the currently logged in user's JWT from the request headers if set
    /// </summary>
    /// <returns>Currently logged in user's JWT if available</returns>
    [NonAction]
    public string? GetJWT() => HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
}
