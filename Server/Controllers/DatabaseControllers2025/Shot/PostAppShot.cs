using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Shot;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostAppShot : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostAppShot")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertShotApp([FromBody] ShotTable request)
    {
        // Validate input
        if (request == null) return BadRequest("Request body required.");
    bool success = await ServerState.UserStore.AddShot(
            request.Type, request.SmartDotID, request.SessionID, request.BallID, 
            request.FrameID, request.ShotNumber,  request.LeaveType, request.Side, request.Position, request.Comment);

        return !success ? Problem("unable to add shot to the database") : Ok("shot inserted successfully");
    }
}
