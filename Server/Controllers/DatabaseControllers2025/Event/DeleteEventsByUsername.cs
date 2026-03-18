using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Event;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteEventsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteEventsByUsername")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllEventsForUser()
    {
        var username = GetUsername();
        if (string.IsNullOrEmpty(username)) return Unauthorized();
        bool success = await ServerState.UserStore.DeleteEventsByUsername(username);
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
