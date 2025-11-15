using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiSessions : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiSessions")]
    [ProducesResponseType(typeof(List<PiSession>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiSessions(DateTime rangeStart, DateTime rangeEnd)
    {
        var sessions = await ServerState.UserStore.GetAllPiSessions(rangeStart, rangeEnd);
        return sessions == null ? Problem("unable to retrieve balls from the database") : Ok(sessions);
    }
}