
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminService.DTOs;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/admin/activity-log")]
    [Authorize(Roles = "Admin")]
    public class ActivityLogController : ControllerBase
    {
        // GET: api/admin/activity-log
        // GET: api/admin/activity-log?role=Farmer
        // GET: api/admin/activity-log?page=2
        // ADMIN ONLY
        [HttpGet]
        public IActionResult GetActivityLog(
            [FromQuery] string? role = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25)
        {
            // Validate role filter
            if (!string.IsNullOrWhiteSpace(role) &&
                role != "Farmer" &&
                role != "Supplier" &&
                role != "Agriculture Officer" &&
                role != "Administrator")
            {
                return BadRequest(new
                {
                    message = "Invalid activity role filter."
                });
            }

            // Validate page number
            if (page < 1)
            {
                return BadRequest(new
                {
                    message = "Page must be 1 or greater."
                });
            }

            // Validate page size
            if (pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message =
                        "Page size must be between 1 and 100."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Authenticate Admin using JWT.
            //
            // 2. Call ActivityLogService.
            //
            // 3. Fetch platform-wide activity
            //    records from the audit store.
            //
            // 4. Apply role filter:
            //    Farmer
            //    Supplier
            //    Agriculture Officer
            //    Administrator
            //
            // 5. No role filter means all roles,
            //    including System activities.
            //
            // 6. Sort by Created descending,
            //    then Id descending.
            //
            // 7. Apply pagination:
            //    Skip = (page - 1) * pageSize
            //    Take = pageSize
            //
            // 8. Calculate TotalCount
            //    and TotalPages.
            //
            // 9. Return ActivityLogPageDto.

            return NotReady("Get platform activity log");
        }

        // GET: api/admin/activity-log/recent
        // GET: api/admin/activity-log/recent?limit=8
        // ADMIN ONLY
        [HttpGet("recent")]
        public IActionResult GetRecentActivities(
            [FromQuery] int limit = 8)
        {
            if (limit < 1 || limit > 100)
            {
                return BadRequest(new
                {
                    message =
                        "Limit must be between 1 and 100."
                });
            }

            // TODO:
            // 1. Authenticate Admin using JWT.
            //
            // 2. Call ActivityLogService.
            //
            // 3. Fetch platform-wide activities.
            //
            // 4. Sort newest first.
            //
            // 5. Return latest 'limit' entries.
            //
            // 6. Return List<ActivityLogEntryDto>.
            //
            // Admin Dashboard uses limit=8.

            return NotReady("Get recent activities");
        }

        // Temporary response until
        // Business Logic is connected.
        private IActionResult NotReady(string operation)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = operation +
                        " service is not connected yet.",
                    success = false,
                    dataLoaded = false
                });
        }
    }
}