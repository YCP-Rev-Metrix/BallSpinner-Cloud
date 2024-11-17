using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.POCOs;
using Server.Controllers.APIControllers;


namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetShotsByUsernameController : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetShotsByUsername")]
    [ProducesResponseType(typeof(List<ShotList>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShotsByUsername()
    {
        LogWriter.LogInfo("GetShotsByUsername called");
        // Attempt to get the list of users from the database
        var shots = await ServerState.UserDatabase.GetShotsbyUsername(GetUsername());

        // If the operation was successful and we have users, return them
        // Return OK with the list of users
        return Ok(shots);
        

    }
}
