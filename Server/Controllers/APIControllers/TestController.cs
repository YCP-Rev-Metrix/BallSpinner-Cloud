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
public class TestController : AbstractFeaturedController
{
    /// <summary>
    /// Basic test - should recieve a 200 code
    /// </summary>
    /// <returns><see cref="StatusCodes.Status200OK"/></returns>
    [HttpGet(Name = "Test")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Test()
    {
        LogWriter.LogInfo("Test called");
        return Ok();
    }
}
