
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FarmerService.DTOs;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // DTO validation is automatic.
            // Real password verification and OTP generation
            // will be handled by AuthService.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "Login service is not connected yet.",
                    authenticated = false,
                    otpSent = false
                });
        }

        // POST: api/auth/verify-otp
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(
            [FromBody] VerifyOtpDto dto)
        {
            // Real OTP verification and JWT generation
            // will be handled by AuthService.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "OTP verification service is not connected yet.",
                    verified = false,
                    authenticated = false
                });
        }
    }
}