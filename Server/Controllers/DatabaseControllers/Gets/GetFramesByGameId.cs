using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetFramesByGameId : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetFramesByGameId")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Frame>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveFramesByGameID(int gameId)
    {
        var frames = await ServerState.UserStore.GetFrames(gameId);
        return frames == null ? Problem("unable to retrieve frames from the database") : Ok(frames);
    }
}