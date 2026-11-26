
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login(
            [FromBody] LoginDto dto)
        {
            // Input validation runs automatically.
            //
            // Later connect AuthService:
            // 1. Find supplier by mobile number.
            // 2. Verify hashed password.
            // 3. Check account status is Active.
            // 4. Generate and send 6-digit OTP.
            // 5. Store OTP securely with expiry.
            //
            // Do not issue JWT before OTP verification.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Supplier login service is not connected yet.",
                    authenticated = false,
                    otpSent = false,
                    tokenIssued = false
                });
        }

        // POST: api/auth/verify-otp
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(
            [FromBody] VerifyOtpDto dto)
        {
            // Input validation runs automatically.
            //
            // Later connect AuthService:
            // 1. Find pending OTP verification.
            // 2. Check OTP expiry and attempts.
            // 3. Verify OTP securely.
            // 4. Invalidate the used OTP.
            // 5. Generate JWT with Supplier role.
            // 6. Return JWT to the UI.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Supplier OTP service is not connected yet.",
                    verified = false,
                    authenticated = false,
                    tokenIssued = false
                });
        }
    }
}