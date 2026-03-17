using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Shot;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppShots : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppShots")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Shot>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAppShot()
    {
        var (success, shots) = await ServerState.UserDatabase.GetAppShots();
        if (success)
            return Ok(shots);
        return Ok(new List<Common.POCOs.MobileApp.Shot>());
    }
}
