using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class InsertBallController : AbstractFeaturedController
{
    [HttpPost("AddBall")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertBall([FromBody] Ball ball)
    {
        return Ok(await ServerState.UserStore.InsertBall(ball.Weight, ball.Color));
    }
}
