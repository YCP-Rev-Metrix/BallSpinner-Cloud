using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;


namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetUsersController : AbstractFeaturedController
{
    [HttpGet(Name = "GetUsers")]
    [ProducesResponseType(typeof(List<UserIdentification>), StatusCodes.Status200OK)] // Assuming this is the DTO containing user information without sensitive data
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUsers()
    {
        // Attempt to get the list of users from the database
        var (success, users) = await ServerState.UserDatabase.GetUsers();

        // If the operation was successful and we have users, return them
        if (success)
        {
            // Return OK with the list of users
            return Ok(users);
        }
        else
        {
            // If no users were found, return a 404 Not Found
            return NotFound("No users found.");
        }
    }
}
