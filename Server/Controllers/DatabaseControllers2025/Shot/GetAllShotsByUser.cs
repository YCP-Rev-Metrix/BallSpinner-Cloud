using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Shot;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllShotsByUser : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetAllShotsByUser")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Shot>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var shots = await ServerState.UserDatabase.GetShotsByUser(GetUsername(), mobileId);
        return Ok(shots ?? new List<Common.POCOs.MobileApp.Shot>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

