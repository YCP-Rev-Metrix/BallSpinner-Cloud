
using Common.Logging;
using Common.POCOs;
using DatabaseCore.ServerTableFunctions.Fall2025DBTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers2025.Game;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class GetAppGames : AbstractFeaturedController
{
    [HttpGet(Name = "GetAppGames")]
    [ProducesResponseType(typeof(List<ShotTable>), StatusCodes.Status200OK)] // Assuming this is the DTO containing user information without sensitive data
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetAllAppGame()
    {
        // Attempt to get the list of users from the database
        var (success, games) = await ServerState.UserDatabase.GetAppGames();

        // If the operation was successful and we have users, return them
        if (success)
        {
            // Return OK with the list of users
            return Ok(games);
        }
        else
        {
            // If no users were found, return a 404 Not Found
            return NotFound("No games found.");
        }
    }
}
