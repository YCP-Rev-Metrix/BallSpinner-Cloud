using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Posts")]
[Route("api/posts/[controller]")]
public class UnregisterController : AbstractFeaturedController
{
    /// <summary>
    /// Unregister's a user's account and removes their access permentantly
    /// </summary>
    /// <param name="userIdentification">The user's identification (username and password)</param>
    /// <returns><see cref="StatusCodes.Status500InternalServerError"/></returns>
    /// <exception cref="NotImplementedException"></exception>
    [Authorize]
    [HttpPost(Name = "Unregister")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Unregister([FromBody] UserIdentification userIdentification) => throw new NotImplementedException();
}
