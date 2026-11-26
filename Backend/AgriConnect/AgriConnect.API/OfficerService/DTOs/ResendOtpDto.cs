
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class ResendOtpDto
    {
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid Sri Lankan mobile number")]
        public string Contact { get; set; } = string.Empty;
    }
}