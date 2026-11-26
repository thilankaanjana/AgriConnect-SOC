
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficerService.DTOs;

namespace OfficerService.Controllers
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
            // DTO validation runs automatically.
            //
            // TODO:
            // 1. Find account using mobile number.
            // 2. Verify password hash securely.
            // 3. Check account status is Active.
            // 4. Verify account role is Officer.
            // 5. Generate secure 6-digit OTP.
            // 6. Store OTP challenge securely.
            // 7. Set OTP expiry to 5 minutes.
            // 8. Limit verification attempts to 5.
            // 9. Send OTP through SMS service.
            // 10. Return a safe response.
            //
            // Do NOT issue JWT before OTP verification.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer login service is not connected yet.",
                    otpSent = false,
                    authenticated = false,
                    tokenIssued = false
                });
        }

        // POST: api/auth/verify-otp
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(
            [FromBody] VerifyOtpDto dto)
        {
            // DTO validation runs automatically.
            //
            // TODO:
            // 1. Find pending login challenge.
            // 2. Check challenge belongs to Officer.
            // 3. Check OTP has not expired.
            // 4. Check maximum attempts.
            // 5. Verify OTP securely.
            // 6. Invalidate OTP after success.
            // 7. Generate signed JWT with:
            //    sub = Officer ID
            //    role = Officer
            // 8. Return token and Officer details.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer OTP verification service is not connected yet.",
                    verified = false,
                    authenticated = false,
                    tokenIssued = false
                });
        }

        // POST: api/auth/resend-otp
        [HttpPost("resend-otp")]
        public IActionResult ResendOtp(
            [FromBody] ResendOtpDto dto)
        {
            // DTO validation runs automatically.
            //
            // TODO:
            // 1. Find existing pending login challenge.
            // 2. Check resend cooldown (30 seconds).
            // 3. Enforce resend/rate limits.
            // 4. Invalidate previous OTP.
            // 5. Generate and store new OTP.
            // 6. Set new expiry to 5 minutes.
            // 7. Send OTP via SMS.
            // 8. Do not reveal account existence.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Officer OTP resend service is not connected yet.",
                    otpSent = false,
                    success = false
                });
        }
    }
}