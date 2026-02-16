using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Establishments;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class PostEstablishmentApp : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "PostEstablishmentApp")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertEstablishmentApp([FromBody] EstablishmentTable request)
    {
        // Validate input
        if (request == null) return BadRequest("Request body required.");

        bool success = await ServerState.UserStore.AddEstablishment(
            request.Name, request.Lanes, request.Type, request.Location);

        return !success ? Problem("unable to add establishment to the database") : Ok("establishment inserted successfully");
    }
}
