
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/farmer-dashboard")]
    [Authorize(Roles = "Farmer")]
    public class FarmerDashboardController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDashboard()
        {
            // Business Logic integration pending

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Dashboard service is not connected yet.",
                    dashboardLoaded = false
                });
        }
    }
}