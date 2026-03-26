using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Event;

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
        if (eventObj == null) return BadRequest("Request body required.");
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        bool success = await ServerState.UserStore.AddEvent(eventObj, GetUsername(), mobileId);
        return !success ? Problem("unable to add event to the database") : Ok("Event inserted successfully");
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}