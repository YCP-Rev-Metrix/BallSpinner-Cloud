using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Establishments;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppEstablishments : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppEstablishments")]
    [ProducesResponseType(typeof(List<EstablishmentTable>), StatusCodes.Status200OK)] // Assuming this is the DTO containing user information without sensitive data
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetAllAppEstablishment()
    {
        // Attempt to get the list of users from the database
        var (success, establishments) = await ServerState.UserDatabase.GetAppEstablishments();

        // If the operation was successful and we have users, return them
        if (success)
        {
            // Return OK with the list of users
            return Ok(establishments);
        }
        else
        {
            // If no establishments were found, return a 200 OK with an empty list instead of a 404
            return Ok(new List<EstablishmentTable>());
        }
    }
}
