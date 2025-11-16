using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
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

    public async Task<IActionResult> InsertUserCombined([FromBody] SessionTable request)
    {
        // Hash the password here as needed
        bool success = await ServerState.UserStore.AddSession(
            request.SessionNumber, request.EstablishmentID, request.EventID, request.DateTime, request.TeamOpponent, request.IndividualOpponent, request.Score, request.Stats, request.TeamRecord, request.IndividualRecord);
        return !success ? Problem("unable to add Session to the database") : Ok("Session inserted successfully");
    }
}
