using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class StartSessionController : AbstractFeaturedController
{
    [HttpPost(Name = "StartSession")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartSession([FromBody] Session session)
    {
        return Ok(await ServerState.UserStore.StartSession(session.Leauge_id,
                                                           session.Tournament_id,
                                                           session.Practice_id,
                                                           session.Time,
                                                           session.Location,
                                                           session.Total_Games,
                                                           session.Total_Frames));
    }
}
