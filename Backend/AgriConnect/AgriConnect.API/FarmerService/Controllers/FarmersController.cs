
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FarmerService.DTOs;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/farmers")]
    [Authorize(Roles = "Farmer")]
    public class FarmersController : ControllerBase
    {
        // POST: api/farmers/validate-registration
        // Public registration validation
        [HttpPost("validate-registration")]
        [AllowAnonymous]
        public IActionResult ValidateRegistration(
            [FromBody] RegisterFarmerDto dto)
        {
            // DTO validation is automatic.
            // Later connect FarmerAccountService.

            return Ok(new
            {
                message = "Farmer registration data is valid (test only)",
                validationPassed = true,
                accountCreated = false,
                savedToDatabase = false,
                nextStep = "Connect FarmerAccountService"
            });
        }

        // GET: api/farmers/me
        // Farmer only
        [HttpGet("me")]
        public IActionResult GetMyProfile()
        {
            // Later:
            // 1. Read Farmer ID from JWT.
            // 2. Call FarmerAccountService.
            // 3. Return farmer profile.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Farmer profile service is not connected yet.",
                    profileLoaded = false
                });
        }

        // PUT: api/farmers/me
        // Farmer only
        [HttpPut("me")]
        public IActionResult UpdateMyProfile(
            [FromBody] UpdateFarmerProfileDto dto)
        {
            // DTO validation is automatic.
            // Later:
            // 1. Read Farmer ID from JWT.
            // 2. Call FarmerAccountService.
            // 3. Update farmer profile.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Farmer profile update service is not connected yet.",
                    profileUpdated = false,
                    savedToDatabase = false
                });
        }
    }
}