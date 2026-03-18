using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Ball;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteBallsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteBallsByUsername")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllBallsForUser()
    {
        var username = GetUsername();
        if (string.IsNullOrEmpty(username)) return Unauthorized();
        bool success = await ServerState.UserStore.DeleteBallsByUsername(username);
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
