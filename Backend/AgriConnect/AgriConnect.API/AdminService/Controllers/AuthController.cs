
using AdminService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        // -------------------------------------
        // 1. SHARED LOGIN
        // POST: api/auth/login
        // -------------------------------------

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(
            typeof(AuthChallengeResponseDto),
            StatusCodes.Status200OK)]
        public IActionResult Login(
            [FromBody] LoginDto dto)
        {
            // TODO: AuthenticationService
            //
            // 1. Normalize mobile number.
            // 2. Find user from shared accounts.
            // 3. Verify stored password hash.
            // 4. Read account Status.
            //
            // Status rules:
            //
            // Pending:
            //    Reject login.
            //    Admin approval required.
            //
            // Rejected:
            //    Reject login.
            //
            // Suspended:
            //    Reject login.
            //
            // Active:
            //    Allow login OTP process.
            //
            // Show account-specific status errors
            // only after password verification.
            //
            // 5. Determine Role from database.
            // 6. Generate secure 6-digit OTP.
            // 7. Save hashed OTP challenge.
            // 8. Set 5-minute expiry.
            // 9. Limit incorrect attempts to 5.
            // 10. Send OTP through SMS.
            // 11. Return AuthChallengeResponseDto.
            //
            // Do not issue JWT yet.

            return NotReady("Shared login");
        }

        // -------------------------------------
        // 2. START PUBLIC REGISTRATION
        // POST: api/auth/register/start
        // -------------------------------------

        [HttpPost("register/start")]
        [AllowAnonymous]
        [ProducesResponseType(
            typeof(AuthChallengeResponseDto),
            StatusCodes.Status200OK)]
        public IActionResult StartRegistration(
            [FromBody] StartRegistrationDto dto)
        {
            // DTO allows only Farmer or Supplier.
            //
            // TODO: AuthenticationService
            //
            // 1. Normalize email and mobile.
            // 2. Check duplicate email/mobile.
            // 3. Validate district.
            // 4. Securely hash password.
            // 5. Store pending registration data
            //    in protected temporary storage.
            // 6. Generate secure 6-digit OTP.
            // 7. Save hashed OTP challenge.
            // 8. Set 5-minute expiry.
            // 9. Send SMS.
            // 10. Return AuthChallengeResponseDto.
            //
            // IMPORTANT:
            // Do not create an account before
            // successful registration OTP.
            //
            // Do not issue JWT.

            return NotReady("Start public registration");
        }

        // -------------------------------------
        // 3. VERIFY OTP
        // POST: api/auth/verify-otp
        //
        // Registration -> Pending account
        // Login        -> JWT after success
        // -------------------------------------

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        [ProducesResponseType(
            typeof(AuthSuccessResponseDto),
            StatusCodes.Status200OK)]
        [ProducesResponseType(
            typeof(RegistrationPendingResponseDto),
            StatusCodes.Status201Created)]
        public IActionResult VerifyOtp(
            [FromBody] VerifyOtpDto dto)
        {
            if (!dto.ChallengeId.HasValue ||
                dto.ChallengeId.Value == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Invalid Challenge ID."
                });
            }

            // TODO: AuthenticationService
            //
            // 1. Find challenge by ChallengeId.
            // 2. Check challenge is not expired.
            // 3. Check maximum 5 failed attempts.
            // 4. Verify OTP securely.
            // 5. Invalidate OTP after success.
            //
            // Read Purpose from stored challenge.
            // Never trust Purpose from client.

            // =================================
            // CASE A: REGISTRATION OTP
            // =================================
            //
            // If challenge Purpose = Registration:
            //
            // 1. Load pending registration data.
            // 2. Recheck duplicate email/mobile.
            // 3. Create Farmer/Supplier account.
            // 4. Set Status = "Pending".
            // 5. Save account to database.
            // 6. Create registration activity log.
            // 7. Notify active Administrators.
            // 8. Return HTTP 201 with:
            //
            //    RegistrationPendingResponseDto
            //
            // IMPORTANT:
            //    AccountCreated = true
            //    Status = "Pending"
            //    CanLogin = false
            //    TokenIssued = false
            //
            // DO NOT generate a JWT.
            //
            // UI should show:
            // "Waiting for administrator approval."

            // =================================
            // CASE B: LOGIN OTP
            // =================================
            //
            // If challenge Purpose = Login:
            //
            // 1. Find existing user account.
            // 2. Check Status == "Active".
            // 3. Identify Role from database.
            // 4. Generate signed JWT:
            //
            //    sub  = UserId
            //    role = Farmer/Supplier/Officer/Admin
            //
            // 5. Return HTTP 200 with:
            //
            //    AuthSuccessResponseDto
            //
            // AccountCreated = false.
            //
            // Token must have an expiry.
            // Only Active users receive JWT.
            //
            // Prevent reuse of an OTP challenge.
            // Apply database transaction handling
            // to registration and challenge use.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "OTP verification service is not connected yet.",
                    verified = false,
                    accountCreated = false,
                    tokenIssued = false
                });
        }

        // -------------------------------------
        // 4. RESEND OTP
        // POST: api/auth/resend-otp
        // -------------------------------------

        [HttpPost("resend-otp")]
        [AllowAnonymous]
        public IActionResult ResendOtp(
            [FromBody] ResendOtpDto dto)
        {
            if (!dto.ChallengeId.HasValue ||
                dto.ChallengeId.Value == Guid.Empty)
            {
                return BadRequest(new
                {
                    message = "Invalid Challenge ID."
                });
            }

            // TODO: AuthenticationService
            //
            // 1. Find pending challenge.
            // 2. Enforce 30-second cooldown.
            // 3. Enforce resend rate limits.
            // 4. Invalidate previous OTP.
            // 5. Generate new secure OTP.
            // 6. Save hashed OTP.
            // 7. Set 5-minute expiry.
            // 8. Send SMS.
            // 9. Return updated challenge data.
            //
            // Do not issue JWT here.

            return NotReady("Resend OTP");
        }

        // -------------------------------------
        // 5. GET LOGGED-IN USER IDENTITY
        // GET: api/auth/me
        // -------------------------------------

        [HttpGet("me")]
        [Authorize(
            Roles = "Farmer,Supplier,Officer,Admin")]
        public IActionResult GetMyIdentity()
        {
            var userId =
                User.FindFirst("sub")?.Value;

            var role =
                User.FindFirst("role")?.Value;

            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(role))
            {
                return Unauthorized(new
                {
                    message =
                        "Required identity claims are missing."
                });
            }

            // TODO: Shared AccountService
            //
            // Before production, recheck that
            // this account is still Active.
            //
            // Suspended/Rejected accounts must
            // lose access even with older JWTs.
            //
            // Implement token revocation or
            // session-version validation.

            return Ok(new
            {
                userId,
                role,
                authenticated = true
            });
        }

        // -------------------------------------
        // TEMPORARY BUSINESS LOGIC RESPONSE
        // -------------------------------------

        private IActionResult NotReady(
            string operation)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        operation +
                        " service is not connected yet.",
                    success = false,
                    tokenIssued = false
                });
        }
    }
}