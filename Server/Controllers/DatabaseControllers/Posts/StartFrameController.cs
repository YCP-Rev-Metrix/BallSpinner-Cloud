using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class StartFrameController : AbstractFeaturedController
{
    [HttpPost(Name = "StartFrame")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartFrame([FromBody] Frame frame)
    {
        return Ok(await ServerState.UserStore.StartFrame(frame.Game_id, frame.Shot_number, frame.Score));
    }
}
