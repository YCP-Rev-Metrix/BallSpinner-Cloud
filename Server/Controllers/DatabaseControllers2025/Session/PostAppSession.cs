using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostAppSession : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostAppSession")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InsertUserCombined([FromBody] Common.POCOs.MobileApp.Session request)
    {
        if (request == null) return BadRequest("Request body required.");
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        bool success = await ServerState.UserDatabase.AddSessionForUser(request, GetUsername(), mobileId);
        return !success ? Problem("unable to add Session to the database") : Ok("Session inserted successfully");
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}
