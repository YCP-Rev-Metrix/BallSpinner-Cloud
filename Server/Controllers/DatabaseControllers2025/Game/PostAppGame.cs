using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Game;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostAppGame : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostAppGame")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertGameApp([FromBody] GameTable request)
    {
        // Validate input
        if (request == null) return BadRequest("Request body required.");
    bool success = await ServerState.UserStore.AddGame(
            request.GameNumber, request.Lanes, request.Score, request.Win, 
            request.StartingLane, request.SessionID,  request.TeamResult, request.IndividualResult);

        return !success ? Problem("unable to add game to the database") : Ok("game inserted successfully");
    }
}
