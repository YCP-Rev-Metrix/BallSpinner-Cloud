using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostAppSession : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostAppSession")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InsertUserCombined([FromBody] Common.POCOs.MobileApp.Session request)
    {
        bool success = await ServerState.UserStore.AddSession(request);
        return !success ? Problem("unable to add Session to the database") : Ok("Session inserted successfully");
    }
}
