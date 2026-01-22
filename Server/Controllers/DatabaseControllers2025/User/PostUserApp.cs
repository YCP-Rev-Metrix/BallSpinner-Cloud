using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers.DatabaseControllers2025.User;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostUserApp : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostUserApp")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertUserCombined([FromBody] UserTable request)
    {
        bool success = await ServerState.UserStore.AddUserCombined(
            request.Firstname, request.Lastname, request.Username, request.HashedPassword, request.PhoneNumber, request.Email, request.LastLogin, request.Hand);
        return !success ? Problem("unable to add user to the database") : Ok("user inserted successfully");
    }
}
