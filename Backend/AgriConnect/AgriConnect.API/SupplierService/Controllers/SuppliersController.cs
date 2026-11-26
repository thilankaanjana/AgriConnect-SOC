
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    [Authorize(Roles = "Supplier")]
    public class SuppliersController : ControllerBase
    {
        // POST: api/suppliers/validate-registration
        // PUBLIC
        [HttpPost("validate-registration")]
        [AllowAnonymous]
        public IActionResult ValidateRegistration(
            [FromBody] RegisterSupplierDto dto)
        {
            // DTO validation is automatic.
            // No account is created here.

            return Ok(new
            {
                message = "Supplier registration data is valid (test only)",
                validationPassed = true,
                accountCreated = false,
                savedToDatabase = false,
                nextStep = "Connect SupplierAccountService"
            });
        }

        // POST: api/suppliers/register
        // PUBLIC
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult RegisterSupplier(
            [FromBody] RegisterSupplierDto dto)
        {
            // TODO: Connect SupplierAccountService.
            // Check duplicate email and mobile.
            // Hash password securely.
            // Create account with Pending status.
            // Complete required verification workflow.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Supplier registration service is not connected yet.",
                    accountCreated = false,
                    savedToDatabase = false
                });
        }

        // GET: api/suppliers/me
        // SUPPLIER ONLY
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            // TODO:
            // Read Supplier ID from JWT.
            // Call SupplierAccountService.
            // Return authenticated supplier profile.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Supplier profile service is not connected yet.",
                    profileLoaded = false
                });
        }

        // PUT: api/suppliers/me
        // SUPPLIER ONLY
        [HttpPut("me")]
        public IActionResult UpdateMyProfile(
            [FromBody] UpdateSupplierProfileDto dto)
        {
            // TODO:
            // Read Supplier ID from JWT.
            // Call SupplierAccountService.
            // Check duplicate contact/email.
            // Update authenticated supplier profile.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Supplier profile update service is not connected yet.",
                    profileUpdated = false,
                    savedToDatabase = false
                });
        }
    }
}