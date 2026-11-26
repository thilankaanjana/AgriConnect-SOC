
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficerService.DTOs;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("api/officer-dashboard")]
    [Authorize(Roles = "Officer")]
    public class OfficerDashboardController : ControllerBase
    {
        // GET: api/officer-dashboard
        // Officer Dashboard
        [HttpGet]
        public IActionResult GetDashboard()
        {
            // TODO: Business Logic integration
            //
            // 1. Authenticate Officer using JWT.
            //
            // 2. Get Officer ID from JWT "sub" claim.
            //
            // 3. Fetch supply requests assigned
            //    to this Officer.
            //
            // 4. Fetch quotations belonging
            //    to those assigned requests.
            //
            // 5. Calculate PendingRequests:
            //    Requests with status
            //    "Pending Verification".
            //
            // 6. Calculate PendingQuotations:
            //    Quotations with status
            //    "Pending Verification".
            //
            // 7. Calculate ApprovedItems:
            //    Requests with status:
            //      - Approved
            //      - Order Confirmed
            //
            //    Quotations with status:
            //      - Approved
            //      - Selected
            //      - Not Selected
            //
            // 8. Calculate RejectedItems:
            //    Rejected requests
            //    + Rejected quotations.
            //
            // 9. Count recommendations created
            //    by this Officer.
            //
            // 10. Build pending request list
            //     using DashboardPendingRequestDto.
            //
            // 11. Build pending quotation list
            //     using DashboardPendingQuotationDto.
            //
            // 12. Return OfficerDashboardDto.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer dashboard service is not connected yet.",
                    dashboardLoaded = false,
                    success = false
                });
        }
    }
}