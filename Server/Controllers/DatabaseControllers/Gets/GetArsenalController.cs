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
public class GetArsenalController : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetArsenal")]
    [ProducesResponseType(typeof(Arsenal), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArsenal()
    {
        LogWriter.LogInfo("GetShotsByUsername called");
        // Attempt to get the list of users from the database
        var arsenal = await ServerState.UserDatabase.GetArsenalbyUsername(GetUsername());
        
        // If the operation was successful and we have users, return them
        // Return OK with the list of users
        return Ok(arsenal);
        

    }
}