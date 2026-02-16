using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostPiShot : AbstractFeaturedController
{
    [HttpPost(Name = "PostPiShots")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<IActionResult> InsertPiShot([FromBody] List<PiShot> shot)
    {
        List<int> newIdList = await ServerState.UserStore.AddPiShots(shot);
        return newIdList == null ? Problem("Internal error") : Ok(newIdList);
    }
}