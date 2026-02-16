using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostPiSmartDotData : AbstractFeaturedController
{
    [HttpPost(Name = "PostPiSmartDotData")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<IActionResult> InsertPiSmartDotData([FromBody] List<PiSmartDotData> smartDotData)
    {
        List<int> newIds = await ServerState.UserStore.AddPiSmartDotData(smartDotData);
        return newIds.IsNullOrEmpty() ? Problem("Internal error") : Ok(newIds);
    }
}