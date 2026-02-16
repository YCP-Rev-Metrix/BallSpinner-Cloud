using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiEncoderDataBySession : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiEncoderDataBySession")]
    [ProducesResponseType(typeof(List<PiDiagnosticScript>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiEncoderDataBySession(int sessionId)
    {
        var encoderData = await ServerState.UserStore.GetPiEncoderDataBySessionId(sessionId);
        return encoderData == null ? Problem("unable to retrieve encoder data from the database") : Ok(encoderData);
    }
}