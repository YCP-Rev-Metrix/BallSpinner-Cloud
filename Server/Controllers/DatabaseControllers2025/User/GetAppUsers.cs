using Common.Logging;
using Common.POCOs.MobileApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.User;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppUsers : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppUsers")]
    [ProducesResponseType(typeof(List<Common.POCOs.MobileApp.User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllAppUsers()
    {
        var (success, users) = await ServerState.UserDatabase.GetAppUsers();
        if (success)
            return Ok(users);
        return Ok(new List<Common.POCOs.MobileApp.User>());
    }
}
