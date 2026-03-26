using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllSessionsByUser : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetAllSessionsByUser")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Session>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var sessions = await ServerState.UserDatabase.GetSessionsByUser(GetUsername(), mobileId);
        return Ok(sessions ?? new List<Common.POCOs.MobileApp.Session>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

