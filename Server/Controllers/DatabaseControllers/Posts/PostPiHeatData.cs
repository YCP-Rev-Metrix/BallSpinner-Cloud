using Common.POCOs.MobileApp;
using Common.POCOs.PITeam2025;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostPiHeatData : AbstractFeaturedController
{
    [HttpPost(Name = "PostPiHeatData")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InsertPiHeatData([FromBody] List<PiHeatData> heatData)
    {
        List<int> newIds = await ServerState.UserStore.AddPiHeatData(heatData);
        return newIds.IsNullOrEmpty() ? Problem("Internal error") : Ok(newIds);
    }
}