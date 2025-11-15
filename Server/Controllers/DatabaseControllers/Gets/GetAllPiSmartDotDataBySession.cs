using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiSmartDotDataBySession : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiSmartDotDataBySession")]
    [ProducesResponseType(typeof(List<PiSmartDotData>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiShotsBySession(int sessionId)
    {
        var smartDotData = await ServerState.UserStore.GetPiSmartDotDataBySessionId(sessionId);
        return smartDotData == null ? Problem("unable to retrieve smart dot data from the database") : Ok(smartDotData);
    }
}