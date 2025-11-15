using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostPiDiagnosticScripts : AbstractFeaturedController
{
    [HttpPost(Name = "PostPiDiagnosticScripts")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<IActionResult> InsertPiDiagnosticScripts([FromBody] List<Common.POCOs.PITeam2025.PiDiagnosticScript> scripts)
    {
        List<int> newIds = await ServerState.UserStore.AddPiDiagnosticScript(scripts);
        return newIds.IsNullOrEmpty() ? Problem("Internal error") : Ok(newIds);
    }
    
}