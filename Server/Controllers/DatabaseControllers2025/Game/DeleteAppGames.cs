using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Game;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteAppGames : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteAppGames")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllGamesForUser()
    {
        var username = GetUsername();
        if (string.IsNullOrEmpty(username)) return Unauthorized();
        bool success = await ServerState.UserStore.DeleteAppGames(username);
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
