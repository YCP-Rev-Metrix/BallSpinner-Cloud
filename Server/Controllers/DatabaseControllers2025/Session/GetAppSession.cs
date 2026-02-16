
using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Session;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppSessions : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppSessions")]
    [ProducesResponseType(typeof(List<SessionTable>), StatusCodes.Status200OK)] // Assuming this is the DTO containing user information without sensitive data
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetAllAppUsers()
    {
        // Attempt to get the list of users from the database
        var (success, sessions) = await ServerState.UserDatabase.GetAppSessions();

        // If the operation was successful and we have users, return them
        if (success)
        {
            // Return OK with the list of users
            return Ok(sessions);
        }
        else
        {
            // If no users were found, return a 404 Not Found
            return NotFound("No sessions found.");
        }
    }
}
