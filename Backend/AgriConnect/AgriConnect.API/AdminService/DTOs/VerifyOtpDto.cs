
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Challenge ID is required")]
        public Guid? ChallengeId { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        [RegularExpression(
            @"^\d{6}$",
            ErrorMessage = "OTP must contain exactly 6 digits")]
        public string Otp { get; set; } = string.Empty;
    }
}