using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.POCOs;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class InsertSampleDataController : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertSampleData")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> InsertSampleData([FromBody] SampleData sampleData)
    {
        bool success = await ServerState.UserDatabase.InsertSampleData(sampleData);
        if (success)
        {
            return Ok("success");
        }
        return Forbid();
    }

}