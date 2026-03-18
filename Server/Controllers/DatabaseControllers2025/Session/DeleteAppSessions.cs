using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteAppSessions : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteAppSessions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllSessionsForUser()
    {
        var username = GetUsername();
        if (string.IsNullOrEmpty(username)) return Unauthorized();
        bool success = await ServerState.UserStore.DeleteAppSessions(username);
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
