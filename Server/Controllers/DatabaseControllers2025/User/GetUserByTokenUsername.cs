using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.User;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetUserByTokenUsername : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetUserByTokenUsername")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.User>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var users = await ServerState.UserDatabase.GetAppUserByUsername(GetUsername(), mobileId);
        return Ok(users ?? new List<Common.POCOs.MobileApp.User>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

