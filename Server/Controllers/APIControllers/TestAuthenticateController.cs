using Common.Logging;
using Common.POCOs;
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
public class TestAuthenticateController : AbstractFeaturedController
{
    /// <summary>
    /// Tests to ensure that the accessing user is authenticated with a JWT with the admin role
    /// </summary>
    /// <returns><see cref="StatusCodes.Status200OK"/> | <see cref="StatusCodes.Status403Forbidden"/> | <see cref="StatusCodes.Status401Unauthorized"/></returns>
    [Authorize(Roles = "Admin")]
    [HttpGet(Name = "TestAuthenticateAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult TestAuthenticateAdmin()
    {
        LogWriter.LogInfo("TestAuthenticateAdmin called");
        return Ok();
    }
}
