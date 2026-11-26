
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminService.DTOs;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        // GET: api/admin/dashboard
        // ADMIN ONLY

        [HttpGet]
        public IActionResult GetDashboard()
        {
            // TODO: Business Logic integration
            //
            // 1. Authenticate Admin using JWT.
            //
            // 2. Call AdminDashboardService.
            //
            // 3. Get registered users from
            //    the account management service.
            //
            // 4. Calculate:
            //
            //    Users = total registered users
            //
            //    Farmers = users with Farmer role
            //
            //    Suppliers = users with Supplier role
            //
            //    Officers = users with Officer role
            //
            // 5. Get supply requests from
            //    FarmerService.
            //
            // 6. Count total supply requests.
            //
            // 7. Get quotations from
            //    SupplierService.
            //
            // 8. Count total quotations.
            //
            // 9. Get confirmed orders from
            //    FarmerService.
            //
            // 10. Count confirmed orders.
            //
            // 11. Calculate OrderValue:
            //
            //     Sum of prices of
            //     confirmed orders.
            //
            // 12. Get platform activity
            //     from ActivityLogService.
            //
            // 13. Sort activities by
            //     Created descending.
            //
            // 14. Take latest 8 activities.
            //
            // 15. Build AdminDashboardDto.
            //
            // 16. Return 200 OK with
            //     dashboard data.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Admin dashboard service is not connected yet.",
                    dashboardLoaded = false,
                    success = false
                });
        }
    }
}