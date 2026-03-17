using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Pi;

public class GetAllPiSessionsRequest
{
    public string? RangeStart { get; set; }
    public string? RangeEnd { get; set; }
}

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAllPiSessions : AbstractFeaturedController
{
    [HttpGet(Name = "GetAllPiSessions")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(List<PiSession>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RetrieveAllPiSessions([FromBody] GetAllPiSessionsRequest request)
    {
        Console.WriteLine(request);
        
        if (request == null) return BadRequest("request body is required");

        string rangeStart = request.RangeStart ?? "00000000000000";
        string rangeEnd = request.RangeEnd ?? "00000000000000";

        var sessions = await ServerState.UserStore.GetAllPiSessions(rangeStart, rangeEnd);
        return sessions == null ? Problem("unable to retrieve balls from the database") : Ok(sessions);
    }
}
