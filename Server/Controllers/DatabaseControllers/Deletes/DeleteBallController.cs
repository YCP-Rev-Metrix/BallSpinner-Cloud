using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Deletes")]
[Route("api/deletes/[controller]")]
public class DeleteBallController : AbstractFeaturedController
{
    /// <summary>
    /// Deletes a ball from the user's arsenal by its name.
    /// </summary>
    /// <param name="request">The name of the ball to delete</param>
    /// <returns>A response indicating the outcome of the operation.</returns>
    [Authorize]
    [HttpDelete(Name = "DeleteBall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Changed to NotFound
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBall([FromQuery] string ballName)
    {
        if (string.IsNullOrWhiteSpace(ballName))
        {
            LogWriter.LogWarn("Delete Ball failed: Ball name is null or empty");
            return BadRequest("Ball name cannot be null or empty.");
        }
        try
        {
            LogWriter.LogInfo($"Delete Ball called by user '{GetUsername()}' for ball '{ballName}'");

            bool success = await ServerState.UserDatabase.RemoveBall(ballName, GetUsername());
            if (!success)
            {
                LogWriter.LogWarn($"Ball '{ballName}' not found for user '{GetUsername()}'");
                return NotFound($"Ball '{ballName}' not found for user '{GetUsername()}'"); // Updated to NotFound
            }

            LogWriter.LogInfo($"Ball '{ballName}' successfully deleted for user '{GetUsername()}'");
            return Ok("Ball deleted");
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"An error occurred while deleting ball '{ballName}' for user '{GetUsername()}': {ex.Message}\n{ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

}
