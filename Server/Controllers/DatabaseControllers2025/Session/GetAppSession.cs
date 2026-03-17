using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppSessions : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppSessions")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Session>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAppUsers()
    {
        var (success, sessions) = await ServerState.UserDatabase.GetAppSessions();
        if (success)
            return Ok(sessions);
        return Ok(new List<Common.POCOs.MobileApp.Session>());
    }
}
