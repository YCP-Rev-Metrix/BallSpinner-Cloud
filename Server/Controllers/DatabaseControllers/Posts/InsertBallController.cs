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
    [HttpPost(Name = "InsertBall")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> InsertBall([FromBody] Ball ball)
    {
        return Ok(await ServerState.UserStore.InsertBall(ball.Weight, ball.Color));
    }
}
