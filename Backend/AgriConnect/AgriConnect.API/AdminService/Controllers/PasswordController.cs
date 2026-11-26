
using AdminService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize(Roles = "Farmer,Supplier,Officer,Admin")]
    public class PasswordController : ControllerBase
    {
        // --------------------------------------
        // CHANGE PASSWORD
        // POST: api/auth/change-password
        //
        // Farmer, Supplier, Officer and Admin
        // --------------------------------------

        [HttpPost("change-password")]
        public IActionResult ChangePassword(
            [FromBody] ChangePasswordDto dto)
        {
            // Get logged-in user ID from JWT
            string? userId =
                User.FindFirst("sub")?.Value;

            // Get logged-in user's role
            string? role =
                User.FindFirst("role")?.Value;

            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(role))
            {
                return Unauthorized(new
                {
                    message = "User identity is missing."
                });
            }

            // Validate password fields
            if (string.IsNullOrWhiteSpace(dto.CurrentPassword) ||
                string.IsNullOrWhiteSpace(dto.NewPassword) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return BadRequest(new
                {
                    message = "All password fields are required."
                });
            }

            // Check new password confirmation
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest(new
                {
                    message =
                        "New password and confirm password do not match."
                });
            }

            // Prevent using the same password
            if (dto.CurrentPassword == dto.NewPassword)
            {
                return BadRequest(new
                {
                    message =
                        "New password must be different from current password."
                });
            }

            // ---------------------------------
            // TODO: BUSINESS LOGIC INTEGRATION
            // ---------------------------------
            //
            // 1. Use userId from the JWT.
            //
            // 2. Find the user's account using
            //    the shared Account Service.
            //
            // 3. Check account is Active.
            //
            // 4. Verify CurrentPassword against
            //    the stored password hash.
            //
            // 5. Reject incorrect password.
            //
            // 6. Hash NewPassword securely.
            //    Never store plaintext passwords.
            //
            // 7. Save the new password hash
            //    to the database.
            //
            // 8. Invalidate other active sessions
            //    and revoke existing tokens.
            //
            // 9. Record a password-change event
            //    without logging passwords.
            //
            // 10. Return 200 OK only after
            //     password is actually changed.
            //
            // Security:
            // Never accept UserId from request body.
            // Never return passwords in response.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Password change service is not connected yet.",
                    passwordChanged = false,
                    savedToDatabase = false
                });
        }
    }
}