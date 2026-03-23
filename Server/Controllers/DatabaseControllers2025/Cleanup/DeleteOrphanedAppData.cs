using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Cleanup;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteOrphanedAppData : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteOrphanedAppData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrphanedRows()
    {
        bool success = await ServerState.UserDatabase.DeleteOrphanedAppData();
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}

