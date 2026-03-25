using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Event;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetEventsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetEventsByUsername")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Event>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveEventsByUsername()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var events = await ServerState.UserStore.GetEvents(GetUsername(), mobileId);
        return events == null ? Problem("unable to retrieve events from the database") : Ok(events);
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}