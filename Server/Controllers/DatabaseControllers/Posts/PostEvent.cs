using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostEvent : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertEvent")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<IActionResult> InsertEvent([FromBody] Common.POCOs.MobileApp.Event eventObj)
    {
        bool success = await ServerState.UserStore.AddEvent(eventObj, GetUsername());
        return !success ? Problem("unable to add event to the database") : Ok("Event inserted successfully");
    }
}