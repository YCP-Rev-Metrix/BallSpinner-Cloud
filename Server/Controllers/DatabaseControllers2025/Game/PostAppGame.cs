using Common.Logging;
using Common.POCOs.MobileApp;
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
    public async Task<IActionResult> InsertGameApp([FromBody] Common.POCOs.MobileApp.Game request)
    {
        if (request == null) return BadRequest("Request body required.");
        bool success = await ServerState.UserStore.AddGame(
            request.GameNumber ?? string.Empty, request.Lanes ?? string.Empty,
            request.Score ?? 0, request.Win ?? 0, request.StartingLane ?? 0,
            request.SessionId ?? 0, request.TeamResult ?? 0, request.IndividualResult ?? 0);
        return !success ? Problem("unable to add game to the database") : Ok("game inserted successfully");
    }
}
