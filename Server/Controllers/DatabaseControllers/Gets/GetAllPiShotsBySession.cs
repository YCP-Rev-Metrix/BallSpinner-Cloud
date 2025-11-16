using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiShotsBySession : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiShotsBySession")]
    [ProducesResponseType(typeof(List<PiShot>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiShotsBySession(int sessionId)
    {
        var shots = await ServerState.UserStore.GetAllPiShotsBySession(sessionId);
        return shots == null ? Problem("unable to retrieve shots from the database") : Ok(shots);
    }
}