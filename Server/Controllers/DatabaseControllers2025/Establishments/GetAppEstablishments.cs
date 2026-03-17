using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Establishments;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppEstablishments : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppEstablishments")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.Establishment>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAppEstablishment()
    {
        var (success, establishments) = await ServerState.UserDatabase.GetAppEstablishments();
        if (success)
            return Ok(establishments);
        return Ok(new List<Common.POCOs.MobileApp.Establishment>());
    }
}
