using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Shot;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostAppShot : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostAppShot")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InsertShotApp([FromBody] Common.POCOs.MobileApp.Shot request)
    {
        if (request == null) return BadRequest("Request body required.");
        bool success = await ServerState.UserStore.AddShot(request);
        return !success ? Problem("unable to add shot to the database") : Ok("shot inserted successfully");
    }
}
