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
    public async Task<IActionResult> InsertSampleData([FromBody] SensorData data)
    {
        
        //bool success = await ServerState.UserDatabase.InsertSampleData(data.SensorType, data.Count, data.TimeStamp, data.X, data.Y, data.Z);
        //if (success)
        //{
        // Return the tokens as a response
            return Ok(GetUsername());
        //}
        //return Forbid();
    }

}