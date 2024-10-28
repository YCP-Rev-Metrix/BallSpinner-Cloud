using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class StartPracticeController : AbstractFeaturedController
{
    [HttpPost(Name = "StartPractice")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> StartPractice([FromBody] Practice practice)
    {
        return Ok(await ServerState.UserStore.StartPractice(practice.Event_id));
    }
}
