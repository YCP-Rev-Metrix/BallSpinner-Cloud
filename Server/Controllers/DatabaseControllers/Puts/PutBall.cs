using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Puts;



[ApiController]
[Tags("Puts")]
[Route("api/puts/[controller]")]
public class PutBall : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PutBall")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> UpdateBall([FromBody] Common.POCOs.Ball ball)
    {
        bool success = await ServerState.UserStore.UpdateBall(ball, GetUsername());
        return !success ? Problem("unable to update ball in the database") : Ok("Ball updated successfully");
    }
}