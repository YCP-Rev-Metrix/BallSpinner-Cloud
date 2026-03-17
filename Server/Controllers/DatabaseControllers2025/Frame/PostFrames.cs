using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Frame;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostFrames : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostFrames")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<IActionResult> InsertFrames([FromBody] Common.POCOs.MobileApp.Frame frames)
    {
        bool success = await ServerState.UserStore.AddFrames(frames, GetUsername());
        return !success ? Problem("unable to add frames to the database") : Ok("Frames added successfully");
    }
}