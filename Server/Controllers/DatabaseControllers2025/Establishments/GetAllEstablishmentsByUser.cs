using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Establishments;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllEstablishmentsByUser : AbstractFeaturedController
{
    [Authorize]
    [HttpGet(Name = "GetAllEstablishmentsByUser")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Establishment>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        var establishments = await ServerState.UserDatabase.GetEstablishmentsByUser(GetUsername(), mobileId);
        return Ok(establishments ?? new List<Common.POCOs.MobileApp.Establishment>());
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}

