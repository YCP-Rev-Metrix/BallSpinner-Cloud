using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;



[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostBalls : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertBall")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> InsertBall([FromBody] Common.POCOs.Ball ball)
    {
        bool success = await ServerState.UserStore.AddBalls(ball, GetUsername());
        return !success ? Problem("unable to add ball to the database") : Ok("Ball inserted successfully");
    }
}