
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid mobile number")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}