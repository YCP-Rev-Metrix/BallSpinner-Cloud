using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Frame;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllFramesByUser : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetAllFramesByUser")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Frame>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var frames = await ServerState.UserDatabase.GetFramesByUser(GetUsername(), mobileId);
        return Ok(frames ?? new List<Common.POCOs.MobileApp.Frame>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

