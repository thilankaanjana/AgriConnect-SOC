
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficerService.DTOs;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("api/officers")]
    public class OfficersController : ControllerBase
    {
        // -------------------------------------
        // 1. VALIDATE OFFICER ACCOUNT
        // POST: api/officers/validate-create
        // ADMIN ONLY
        // -------------------------------------

        [HttpPost("validate-create")]
        [Authorize(Roles = "Admin")]
        public IActionResult ValidateOfficer(
            [FromBody] RegisterOfficerDto dto)
        {
            // DTO validation is automatic.
            // Duplicate accounts are not checked.

            return Ok(new
            {
                message = "Officer account fields are valid.",
                validationPassed = true,
                accountCreated = false,
                savedToDatabase = false
            });
        }

        // -------------------------------------
        // 2. CREATE OFFICER ACCOUNT
        // POST: api/officers
        // ADMIN ONLY
        // -------------------------------------

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateOfficer(
            [FromBody] RegisterOfficerDto dto)
        {
            // TODO: Business Logic integration
            //
            // 1. Verify Admin identity.
            // 2. Check duplicate email and mobile.
            // 3. Validate assigned district.
            // 4. Hash temporary password securely.
            // 5. Create account with role Officer.
            // 6. Save through shared account service.
            // 7. Record account creation.
            // 8. Return 201 Created.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer account creation service is not connected yet.",
                    accountCreated = false,
                    savedToDatabase = false
                });
        }

        // -------------------------------------
        // 3. GET MY PROFILE
        // GET: api/officers/me
        // OFFICER ONLY
        // -------------------------------------

        [HttpGet("me")]
        [Authorize(Roles = "Officer")]
        public IActionResult GetMyProfile()
        {
            string? officerId =
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(officerId))
            {
                return Unauthorized(new
                {
                    message = "Officer identity is missing."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Use officerId from JWT.
            // 2. Call shared account/profile service.
            // 3. Find this Officer's account.
            // 4. Return OfficerProfileDto:
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
            // 5. Do not return Password or hash.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer profile service is not connected yet.",
                    profileLoaded = false
                });
        }

        // -------------------------------------
        // 4. UPDATE MY PROFILE - NEW
        // PUT: api/officers/me
        // OFFICER ONLY
        // -------------------------------------

        [HttpPut("me")]
        [Authorize(Roles = "Officer")]
        public IActionResult UpdateMyProfile(
            [FromBody] UpdateOfficerProfileDto dto)
        {
            string? officerId =
                User.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(officerId))
            {
                return Unauthorized(new
                {
                    message = "Officer identity is missing."
                });
            }

            // Check blank values.
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Contact) ||
                string.IsNullOrWhiteSpace(dto.Address) ||
                string.IsNullOrWhiteSpace(dto.District))
            {
                return BadRequest(new
                {
                    message =
                        "Name, Contact, Address and District are required."
                });
            }

            // Prepare normalized values.
            string name = dto.Name.Trim();
            string contact = dto.Contact.Trim();
            string address = dto.Address.Trim();
            string district = dto.District.Trim();

            // TODO: Business Logic integration
            //
            // 1. Find Officer account using
            //    officerId from the JWT.
            //
            // 2. Verify the account exists
            //    and its status allows updates.
            //
            // 3. Check whether the new mobile
            //    number belongs to another account.
            //
            // 4. Validate District against
            //    the supported district list.
            //
            // 5. If District is changed,
            //    apply approved Officer
            //    reassignment rules.
            //
            // 6. Update:
            //    Name, Contact, Address, District.
            //
            // 7. Do not change Email, Role,
            //    Status or Password here.
            //
            // 8. Save through shared account
            //    service and database.
            //
            // 9. Record profile update
            //    in Activity Log.
            //
            // 10. Return updated OfficerProfileDto.
            //
            // IMPORTANT:
            // Never take OfficerId from
            // the request body.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer profile update service is not connected yet.",
                    profileUpdated = false,
                    savedToDatabase = false
                });
        }
    }
}