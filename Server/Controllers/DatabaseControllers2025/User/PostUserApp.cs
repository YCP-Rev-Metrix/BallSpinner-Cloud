using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.User;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostUserApp : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostUserApp")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InsertUserCombined([FromBody] Common.POCOs.MobileApp.User request)
    {
        if (request?.HashedPassword == null) return BadRequest("User and HashedPassword required.");
        bool success = await ServerState.UserStore.AddUserCombined(
            request.Firstname, request.Lastname, request.Username, request.HashedPassword, request.PhoneNumber, request.Email, request.LastLogin, request.Hand, request.MobileID);
        return !success ? Problem("unable to add user to the database") : Ok("user inserted successfully");
    }
}
