using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Game;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppGames : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppGames")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Game>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAppGame()
    {
        var (success, games) = await ServerState.UserDatabase.GetAppGames();
        if (success)
            return Ok(games);
        return Ok(new List<Common.POCOs.MobileApp.Game>());
    }
}
