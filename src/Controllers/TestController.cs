using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Ok(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        return BadRequest("User not authenticated");
    }
}
