
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/supplier-dashboard")]
    [Authorize(Roles = "Supplier")]
    public class SupplierDashboardController : ControllerBase
    {
        // GET: api/supplier-dashboard

        [HttpGet]
        public IActionResult GetDashboard()
        {
            // Identify Supplier using JWT
            string? supplierId =
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(supplierId))
            {
                return Unauthorized(new
                {
                    message = "Supplier identity is missing."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Use supplierId from JWT.
            //
            // 2. Get this Supplier's inventory.
            //
            // 3. Count stock items.
            //
            // 4. Count low/out-of-stock items
            //    (quantity <= 20).
            //
            // 5. Get regional supply requests
            //    from FarmerService.
            //
            // 6. Count requests the Supplier
            //    has not already quoted for.
            //
            // 7. Get this Supplier's quotations.
            //
            // 8. Count approved quotations.
            //
            // 9. Get this Supplier's deliveries.
            //
            // 10. Count active deliveries.
            //
            // 11. Return SupplierDashboardDto
            //     including recent 5 requests
            //     and recent 5 deliveries.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Supplier dashboard service is not connected yet.",
                    dashboardLoaded = false,
                    success = false
                });
        }
    }
}