using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class InsertSmartDotController : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertSmartDot")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertSmartDot([FromBody] SmartDot smartDot)
    {
        bool sucess = await ServerState.UserStore.AddSmartDot(smartDot, GetUsername());
        return !sucess ? Problem("unable to add smartdot to the database") : Ok("SmartDot inserted successfully");
    }
}
