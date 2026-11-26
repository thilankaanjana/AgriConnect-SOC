
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminService.DTOs;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        // GET: api/admin/users
        // GET: api/admin/users?role=Farmer
        // GET: api/admin/users?role=Pending

        [HttpGet]
        public IActionResult GetUsers(
            [FromQuery] string? role = null)
        {
            if (!string.IsNullOrWhiteSpace(role) &&
                role != "Farmer" &&
                role != "Supplier" &&
                role != "Officer" &&
                role != "Admin" &&
                role != "Pending")
            {
                return BadRequest(new
                {
                    message = "Invalid user filter."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Call UserManagementService.
            // 2. Retrieve registered users.
            //
            // If role == "Pending":
            //    Filter by Status == "Pending".
            //
            // Otherwise:
            //    Filter by user Role.
            //
            // 3. Show Pending users first.
            // 4. Return List<AdminUserDto>.
            // 5. Never return passwords or hashes.

            return NotReady("Get registered users");
        }

        // POST: api/admin/users
        // Create Officer / Admin account

        [HttpPost]
        public IActionResult CreateUser(
            [FromBody] CreateAdminUserDto dto)
        {
            // TODO: Business Logic integration
            //
            // 1. Verify Admin identity.
            // 2. Validate Officer / Admin role.
            // 3. Check duplicate email/mobile.
            // 4. Hash password securely.
            // 5. Create account.
            // 6. Set status = "Active".
            // 7. Save to database.
            // 8. Record Activity Log.
            // 9. Return 201 Created.

            return NotReady("Create user account");
        }

        // PATCH: api/admin/users/1/status
        // Approve / Reject / Suspend / Activate

        [HttpPatch("{id:int}/status")]
        public IActionResult UpdateUserStatus(
            int id,
            [FromBody] UpdateUserStatusDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid user ID."
                });
            }

            string? adminIdText =
                User.FindFirst("sub")?.Value;

            if (!int.TryParse(
                    adminIdText,
                    out int currentAdminId) ||
                currentAdminId <= 0)
            {
                return Unauthorized(new
                {
                    message = "Admin identity is invalid."
                });
            }

            // UI prevents Admin changing own account.

            if (currentAdminId == id)
            {
                return BadRequest(new
                {
                    message =
                        "You cannot change your own account status."
                });
            }

            // DTO automatically validates:
            // Active, Rejected or Suspended.

            // TODO: Business Logic integration
            //
            // 1. Find target user by ID.
            // 2. Read current account status.
            //
            // Follow UI status transitions:
            //
            // Pending   -> Active
            // Pending   -> Rejected
            // Active    -> Suspended
            // Suspended -> Active
            // Rejected  -> Active
            //
            // 3. Reject invalid transitions.
            // 4. Save new status to database.
            // 5. Record action in Activity Log.
            // 6. Revoke active sessions/tokens
            //    when account is suspended.
            // 7. Return updated AdminUserDto.
            //
            // Only Active users may log in.

            return NotReady("Update user status");
        }

        private IActionResult NotReady(string operation)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = operation +
                        " service is not connected yet.",
                    success = false,
                    savedToDatabase = false
                });
        }
    }
}