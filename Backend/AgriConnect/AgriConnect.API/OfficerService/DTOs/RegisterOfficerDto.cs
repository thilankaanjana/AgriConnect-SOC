
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class RegisterOfficerDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid Sri Lankan mobile number")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Assigned district is required")]
        [MaxLength(100)]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Temporary password is required")]
        [MinLength(6,
            ErrorMessage = "Password must contain at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}