using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PetCare.Server.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class HealthChecksController : ControllerBase
    {
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new
            {
                DateTime = DateTime.UtcNow,
            });
        }
    }
}
