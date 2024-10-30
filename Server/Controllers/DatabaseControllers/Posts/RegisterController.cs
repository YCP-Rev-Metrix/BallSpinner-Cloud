using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Mvc;
using RevMetrix.BallSpinner.BackEnd.Common.POCOs;
using Server.Controllers.APIControllers;
namespace Server.Controllers.DatabaseControllers.Posts;


[ApiController]
[Tags("Posts")]
[Route("api/posts")]
public class RegisterController : AbstractFeaturedController
{
    private readonly AuthorizeController _authorizeController;
    
    public RegisterController(AuthorizeController authorizeController)
    {
        _authorizeController = authorizeController;
    }

    /// <summary>
    /// Registers new user in the server. Authorizes the user and returns their refresh token and JWT
    /// </summary>
    
    /// <param name="user">All user identification provided by the user</param>
    
    /// <returns>
    /// <see cref="StatusCodes.Status200OK"/>(<see cref="DualToken"/>)
    /// <see cref="StatusCodes.Status403Forbidden"/>
    /// <see cref="StatusCodes.Status409Conflict"/>
    /// </returns>
    
    [HttpPost("register")]
    [ProducesResponseType(typeof(DualToken), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    
    public async Task<IActionResult> Register([FromBody] UserIdentification user)
    {
        LogWriter.LogInfo("Register called");
        // Creating user 
        bool result = await ServerState.UserStore.CreateUser(
            user.Firstname,
            user.Lastname,
            user.Username,
            user.Password,
            user.Email,
            user.PhoneNumber); 
        // Waiting for results which if successful the system will authorize user
       
        return result ? await _authorizeController.Authorize(new Credentials(user.Username, user.Password)) : Conflict();
    }
}