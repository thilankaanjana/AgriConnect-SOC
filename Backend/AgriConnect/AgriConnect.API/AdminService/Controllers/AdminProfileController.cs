
using AdminService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/admin/me")]
    [Authorize(Roles = "Admin")]
    public class AdminProfileController : ControllerBase
    {
        // -------------------------------------
        // 1. GET ADMIN PROFILE
        // GET: api/admin/me
        // -------------------------------------

        [HttpGet]
        public IActionResult GetMyProfile()
        {
            // Get authenticated Admin ID from JWT
            string? adminId =
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized(new
                {
                    message = "Admin identity is missing."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Use adminId from JWT.
            //
            // 2. Call shared Account/Profile Service.
            //
            // 3. Find the logged-in Admin account.
            //
            // 4. Check the account is active.
            //
            // 5. Return AdminProfileDto containing:
            //
            //    Id
            //    Name
            //    Email
            //    Contact
            //    Address
            //    District
            //    Role
            //    Status
            //
            // 6. Do not return passwords
            //    or password hashes.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Admin profile service is not connected yet.",
                    profileLoaded = false,
                    success = false
                });
        }

        // -------------------------------------
        // 2. UPDATE ADMIN PROFILE
        // PUT: api/admin/me
        // -------------------------------------

        [HttpPut]
        public IActionResult UpdateMyProfile(
            [FromBody] UpdateAdminProfileDto dto)
        {
            // Get authenticated Admin ID from JWT
            string? adminId =
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized(new
                {
                    message = "Admin identity is missing."
                });
            }

            // DTO automatically validates
            // required fields and mobile format.

            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Contact) ||
                string.IsNullOrWhiteSpace(dto.District))
            {
                return BadRequest(new
                {
                    message =
                        "Name, Contact and District are required."
                });
            }

            // Prepare cleaned values
            string name = dto.Name.Trim();

            string contact = dto.Contact.Trim();

            string address = dto.Address?.Trim() ?? "";

            string district = dto.District.Trim();

            // TODO: Business Logic integration
            //
            // 1. Find Admin account using adminId.
            //
            // 2. Verify account is active.
            //
            // 3. Check whether Contact is already
            //    used by another account.
            //
            // 4. Validate District against
            //    supported districts.
            //
            // 5. Update:
            //    Name
            //    Contact
            //    Address
            //    District
            //
            // 6. Do not modify:
            //    Email
            //    Role
            //    Status
            //    Password
            //
            // 7. Save through shared Account Service.
            //
            // 8. Record update in Activity Log.
            //
            // 9. Return updated AdminProfileDto.
            //
            // IMPORTANT:
            // Never accept Admin ID from
            // the request body.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Admin profile update service is not connected yet.",
                    profileUpdated = false,
                    savedToDatabase = false
                });
        }
    }
}