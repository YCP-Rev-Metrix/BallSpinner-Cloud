using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Establishments;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostEstablishmentApp : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostEstablishmentApp")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InsertEstablishmentApp([FromBody] Establishment request)
    {
        if (request == null) return BadRequest("Request body required.");
        int? mobileId = TryParseQueryInt(Request.Query["mobileID"]) ?? TryParseQueryInt(Request.Query["mobileId"]);
        int? cloudId = await ServerState.UserStore.AddEstablishment(request, GetUsername(), mobileId);
        return cloudId == null ? Problem("unable to add establishment to the database") : Ok(cloudId);
    }

    private static int? TryParseQueryInt(Microsoft.Extensions.Primitives.StringValues values)
    {
        if (values.Count == 0) return null;
        return int.TryParse(values[0], out var v) && v > 0 ? v : null;
    }
}
