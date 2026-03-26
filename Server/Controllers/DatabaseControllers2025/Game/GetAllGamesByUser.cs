using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Game;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllGamesByUser : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetAllGamesByUser")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Game>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var games = await ServerState.UserDatabase.GetGamesByUser(GetUsername(), mobileId);
        return Ok(games ?? new List<Common.POCOs.MobileApp.Game>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

