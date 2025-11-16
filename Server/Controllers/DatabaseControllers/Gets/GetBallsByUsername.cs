using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetBallsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetBallsByUsername")]
    [ProducesResponseType(typeof(List<Common.POCOs.Ball>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveBallsByUsername()
    {
        var balls = await ServerState.UserStore.GetBalls(GetUsername());
        return balls == null ? Problem("unable to retrieve balls from the database") : Ok(balls);
    }
}