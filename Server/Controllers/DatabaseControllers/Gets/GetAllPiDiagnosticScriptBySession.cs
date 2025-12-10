using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Gets;


[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiDiagnosticScriptBySession : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiDiagnosticScriptBySession")]
    [ProducesResponseType(typeof(List<PiDiagnosticScript>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiShotsBySession(int sessionId)
    {
        var diagnosticScripts = await ServerState.UserStore.GetAllPiDiagnosticScriptsBySession(sessionId);
        return diagnosticScripts == null 
            ? Problem("unable to retrieve diagnostic scripts from the database") :
            Ok(diagnosticScripts);
    }
    
}