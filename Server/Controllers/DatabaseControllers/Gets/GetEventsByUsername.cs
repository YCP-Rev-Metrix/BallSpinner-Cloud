using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetEventsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetEventsByUsername")]
    [ProducesResponseType(typeof(List<Common.POCOs.Ball>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveEventsByUsername()
    {
        var events = await ServerState.UserStore.GetEvents(GetUsername());
        return events == null ? Problem("unable to retrieve events from the database") : Ok(events);
    }
}