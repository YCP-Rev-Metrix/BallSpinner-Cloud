using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAcclerationFromBall_idController : AbstractFeaturedController
{

    [HttpGet(Name = "GetAcclerationFromBall_id")]
    [ProducesResponseType(typeof(List<Shot>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccelerations(int ball_id)
    {
        var (success, accelerations) = await ServerState.UserDatabase.GetAcclerationFromBall_id(ball_id);

        if (success)
        {
            return Ok(accelerations);
        }

        else
        {
            return NotFound("No Accelerations for ball_id found");
        }
    }



    //Give me the percentage of strikes vs shots for the last 100 frames from a user, a games can have 10 to 12 frames 
}