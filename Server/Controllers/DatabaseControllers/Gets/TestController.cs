using Common.Logging;
using Common.POCOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Common.POCOs;
using Server.Controllers.APIControllers;
using Common.POCOs.MobileApp;


namespace Server.Controllers.DatabaseControllers.Posts;

[ApiController]
[Tags("Gets")]
[Route("api/gets/[controller]")]
public class TestController : AbstractFeaturedController
{
    [HttpGet(Name = "GetTest")]
    [ProducesResponseType(typeof(TestPOCO), StatusCodes.Status200OK)] // Assuming this is the DTO containing user information without sensitive data
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTest()
    {
        // Attempt to get the list of users from the database
        var testInteger = await ServerState.UserDatabase.GetTest();

        return Ok(testInteger);
    }
}
