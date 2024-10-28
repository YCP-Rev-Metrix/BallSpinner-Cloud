using Common.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.APITestControllers;

/// <summary>
/// Provides functionality for testing API connections
/// </summary>
[ApiController]
[Tags("Tests")]
[Route("api/tests/[controller]")]
public class TestAuthController : AbstractFeaturedController
{
    /// <summary>
    /// Tests to ensure that the accessing user is authenticated with a JWT
    /// </summary>
    /// <returns><see cref="StatusCodes.Status200OK"/> | <see cref="StatusCodes.Status403Forbidden"/></returns>
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet(Name = "TestAuthorize")]
    public IActionResult TestAuthorize()
    {
        LogWriter.LogInfo("TestAuthorize called");
        return Ok();
    }
}
