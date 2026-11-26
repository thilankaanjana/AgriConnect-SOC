
using Microsoft.AspNetCore.Mvc;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                Service = "FarmerService",
                Status = "ONLINE",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}