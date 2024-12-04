using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Deletes;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class DeleteBallController : AbstractFeaturedController
{
    /// <summary>
    /// Deletes a ball from the user's arsenal by its name.
    /// </summary>
    /// <param name="request">The name of the ball to delete</param>
    /// <returns>A response indicating the outcome of the operation.</returns>
    [Authorize]
    [HttpPost(Name = "DeleteBall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] // Changed to NotFound
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBall([FromBody] DeleteBallRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BallName))
        {
            LogWriter.LogWarn("Delete Ball failed: Ball name is null or empty");
            return BadRequest("Ball name cannot be null or empty.");
        }

        try
        {
            LogWriter.LogInfo($"Delete Ball called by user '{GetUsername()}' for ball '{request.BallName}'");

            bool success = await ServerState.UserDatabase.RemoveBall(request.BallName, GetUsername());
            if (!success)
            {
                LogWriter.LogWarn($"Ball '{request.BallName}' not found for user '{GetUsername()}'");
                return NotFound($"Ball '{request.BallName}' not found for user '{GetUsername()}'"); // Updated to NotFound
            }

            LogWriter.LogInfo($"Ball '{request.BallName}' successfully deleted for user '{GetUsername()}'");
            return Ok("Ball deleted");
        }
        catch (Exception ex)
        {
            LogWriter.LogError($"An error occurred while deleting ball '{request.BallName}' for user '{GetUsername()}': {ex.Message}\n{ex}");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
        }
    }

}
