
using Microsoft.AspNetCore.Mvc;

namespace SupplierService.Controllers
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
                Service = "SupplierService",
                Status = "ONLINE",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}