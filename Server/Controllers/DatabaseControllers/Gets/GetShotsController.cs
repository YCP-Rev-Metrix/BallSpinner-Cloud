using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetShotsController : AbstractFeaturedController
{
    [HttpGet(Name = "GetShots")]
    [ProducesResponseType(typeof(List<Shot>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ActionResult<Shot>), StatusCodes.Status500InternalServerError)] // Specific response type for 500 error
    public async Task<ActionResult<Shot>> GetShots()
    {
       


        // If the operation was successful and we have users, return them
        try
        {
            // attempt to get the list of shots from the database
            var (success, shots) = await ServerState.UserDatabase.GetShots();

            // If the operation was successful and we have users, return them
            if (success)
            {
                // Return OK with the list of users
                return Ok(shots);
            }
            else
            {
                // If no shots were found, return a 404 Not Found
                return NotFound("No shots found.");
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new IdentityError
            {
                Description = "An unexpected error occurred: " + e.Message
            });

        }
    }
}
