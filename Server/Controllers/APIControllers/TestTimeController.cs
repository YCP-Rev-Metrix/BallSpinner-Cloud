using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.APITestControllers;

/// <summary>
/// Provides functionality for testing API connections
/// </summary>
[ApiController]
[Tags("Tests")]
[Route("api/tests/[controller]")]
public class TestTimeController : AbstractFeaturedController
{
    /// <summary>
    /// Grabs the current server time in UTC and send it back - good for testing clock sync and connection
    /// </summary>
    /// <returns><see cref="StatusCodes.Status200OK"/>(<see cref="DateTimePoco"/>)</returns>
    [HttpGet(Name = "TestTime")]
    [ProducesResponseType(typeof(DateTimePoco), StatusCodes.Status200OK)]
    public IActionResult TestTime()
    {
        LogWriter.LogInfo("TestTime called");
        return Ok(DateTimePoco.UTCNow);
    }
}
