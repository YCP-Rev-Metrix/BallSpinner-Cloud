using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class InsertSimulatedShotController : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertSimulatedShot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> InsertSimulatedShot([FromBody] SimulatedShot simulatedShot)
    {
        bool success = await ServerState.UserDatabase.InsertSimulatedShot(simulatedShot, GetUsername());
        if (success)
        {
            return Ok("success");
        }
        return Forbid();
    }

}