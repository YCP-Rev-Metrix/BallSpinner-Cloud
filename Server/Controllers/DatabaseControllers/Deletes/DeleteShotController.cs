using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteShotController : AbstractFeaturedController
{
    /// <summary>
    /// Deletes a ball from the user's arsenal by its name.
    /// </summary>
    /// <param name="request">The name of the ball to delete</param>
    /// <returns>A response indicating the outcome of the operation.</returns>
    [Authorize]
    [HttpDelete(Name = "DeleteShot")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Changed to NotFound
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteShot([FromQuery] DeleteShotRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ShotName))
        {
            LogWriter.LogWarn("Delete Shot failed: Shot name is null or empty");
            return BadRequest("Shot name cannot be null or empty.");
        }

        try
        {
            LogWriter.LogInfo($"Delete Shot called by user '{GetUsername()}' for shot '{request.ShotName}'");

            bool success = await ServerState.UserDatabase.RemoveShot(request.ShotName, GetUsername());
            if (!success)
            {
                LogWriter.LogWarn($"Shot '{request.ShotName}' not found for user '{GetUsername()}'");
                return NotFound($"Shot '{request.ShotName}' not found for user '{GetUsername()}'"); // Updated to NotFound
            }

            LogWriter.LogInfo($"Shot '{request.ShotName}' successfully deleted for user '{GetUsername()}'");
            return Ok("Shot deleted");
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"An error occurred while deleting Shot '{request.ShotName}' for user '{GetUsername()}': {ex.Message}\n{ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

}
