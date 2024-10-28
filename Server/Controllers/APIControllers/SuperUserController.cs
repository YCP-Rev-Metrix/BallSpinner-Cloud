using Common.POCOs;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers.APIControllers;

namespace Server.Controllers.APITestControllers;

/// <summary>
/// Provides elevated admin functionality such as database rebuilding
/// </summary>
[Tags("Posts")]
[Route("api/posts")]
public class SuperUserController : AbstractFeaturedController
{
    /// <summary>
    /// Hashes and salts the provided password
    /// </summary>
    /// <param name="password">Password to hash and salt</param>
    /// <returns><see cref="StatusCodes.Status200OK"/>(<see cref="Common.POCOs.HashAndSalt"/>)</returns>
    [ProducesResponseType(typeof(HashAndSalt), StatusCodes.Status200OK)]
    [HttpPost("HashAndSalt", Name = "HashAndSalt")]
    public IActionResult HashAndSalt([FromBody] Password password)
    {
        (byte[] hash, byte[] salt) = ServerState.SecurityHandler.SaltHashPassword(password.RawPassword);
        return Ok(new HashAndSalt(hash, salt));
    }
}
