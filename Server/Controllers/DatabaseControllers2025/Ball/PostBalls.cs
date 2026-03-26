using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Ball;



[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostBalls : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertBall")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> InsertBall([FromBody] Common.POCOs.MobileApp.Ball ball)
    {
        if (ball == null) return BadRequest("Request body required.");
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        bool success = await ServerState.UserStore.AddBalls(ball, GetUsername(), mobileId);
        return !success ? Problem("unable to add ball to the database") : Ok("Ball inserted successfully");
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}