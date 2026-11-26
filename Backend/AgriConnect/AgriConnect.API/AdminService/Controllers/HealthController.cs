
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
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
                Service = "AdminService",
                Status = "ONLINE",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}