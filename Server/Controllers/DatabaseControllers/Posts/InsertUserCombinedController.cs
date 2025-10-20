using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class InsertUserCombinedController : AbstractFeaturedController
{
    [Authorize]
    [HttpPost(Name = "InsertUserCombined")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> InsertUserCombined([FromBody] UserCombinedRequest request)
    {
        // Hash the password here as needed
        byte[] data = new byte[10]; // Replace with actual hash logic
        bool success = await ServerState.UserStore.AddUserCombined(
            request.Firstname, request.Lastname, request.Username, data, request.Phone, request.Email);
        return !success ? Problem("unable to add user to the database") : Ok("user inserted successfully");
    }

    public class UserCombinedRequest
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
