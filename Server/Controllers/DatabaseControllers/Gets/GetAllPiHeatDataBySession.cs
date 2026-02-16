using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiHeatDataBySession : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiHeatDataBySession")]
    [ProducesResponseType(typeof(List<PiDiagnosticScript>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiHeatDataBySession(int sessionId)
    {
        var heatData = await ServerState.UserStore.GetPiHeatDataBySessionId(sessionId);
        return heatData == null ? Problem("unable to retrieve heat data from the database") : Ok(heatData);
    }
}