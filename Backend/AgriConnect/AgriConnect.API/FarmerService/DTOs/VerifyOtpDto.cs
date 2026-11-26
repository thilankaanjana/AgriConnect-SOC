
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class VerifyOtpDto
    {
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid mobile number")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP is required")]
        [RegularExpression(
            @"^\d{6}$",
            ErrorMessage = "OTP must contain exactly 6 digits")]
        public string Otp { get; set; } = string.Empty;
    }
}