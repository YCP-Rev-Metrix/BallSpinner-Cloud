using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.POCOs;
using Server.Controllers.APIControllers;


namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class GetShotsByShotnameController : AbstractFeaturedController
{

    [Authorize]
    [HttpPost(Name = "GetShotsByShotname")]
    [ProducesResponseType(typeof(List<ShotList>), StatusCodes.Status200OK)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetShotsByShotname([FromBody] GetShotsRequest request)
    {
        LogWriter.LogInfo("GetShotsByShotname called");
        var shots = await ServerState.UserDatabase.GetShotsByShotname(GetUsername(), request.Shotname);
        return Ok(shots);
    }
    public class GetShotsRequest
    {
        public string? Shotname { get; set; }
    }

}