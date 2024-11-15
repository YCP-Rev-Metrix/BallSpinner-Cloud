
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetUserIdController : AbstractFeaturedController
{
    [HttpGet(Name = "GetUserID")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserId([FromQuery] string username)
    {
        // Attempt to get the user ID from the database
        var userId = await ServerState.UserDatabase.GetUserId(username);

        // If the user is found, return the user ID
        if (userId != null)
        {
            return Ok(userId); // Return the user ID in the response
        }

        // If the user is not found, return 404 Not Found
        return NotFound(new { message = "User not found" });
    }
}