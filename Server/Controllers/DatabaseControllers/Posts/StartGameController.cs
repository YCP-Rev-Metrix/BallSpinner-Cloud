using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class StartGameController : AbstractFeaturedController
{
    [HttpPost(Name = "StartGame")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartGame([FromBody] Game game)
    {
        return Ok(await ServerState.UserStore.StartGame(game.Session_id, game.Score));
    }
}
