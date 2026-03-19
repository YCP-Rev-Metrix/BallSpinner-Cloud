using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Frame;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteAppFrames : AbstractFeaturedController
{
    [Authorize]
    [HttpDelete(Name = "DeleteAppFrames")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAllFramesForUser([FromBody] DeleteAccountRequest request)
    {
        var username = request?.Username;
        if (string.IsNullOrEmpty(username)) return BadRequest("username is required.");
        bool success = await ServerState.UserStore.DeleteAppFrames(username, request?.MobileID);
        return success ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}
