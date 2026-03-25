using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Ball;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetBallsByUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetBallsByUsername")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Ball>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveBallsByUsername()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var balls = await ServerState.UserStore.GetBalls(GetUsername(), mobileId);
        return balls == null ? Problem("unable to retrieve balls from the database") : Ok(balls);
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}