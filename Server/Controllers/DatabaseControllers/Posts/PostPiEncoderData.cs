using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostPiEncoderData : AbstractFeaturedController
{
    [HttpPost(Name = "PostPiEncoderData")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> InsertPiEncoderData([FromBody] List<Common.POCOs.PITeam2025.PiEncoderData> encoderData)
    {
        List<int> newIds = await ServerState.UserStore.AddPiEncoderData(encoderData);
        return newIds.IsNullOrEmpty() ? Problem("Internal error") : Ok(newIds);
    }
}