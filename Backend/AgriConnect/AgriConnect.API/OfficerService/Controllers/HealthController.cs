
using Microsoft.AspNetCore.Mvc;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        // GET: /health
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                Service = "OfficerService",
                Status = "ONLINE",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}