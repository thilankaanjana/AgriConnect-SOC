
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class StartRegistrationDto
    {
        [Required(ErrorMessage = "Role is required")]
        [RegularExpression(
            @"^(Farmer|Supplier)$",
            ErrorMessage =
                "Public registration is allowed only for Farmer or Supplier")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
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

        [Required(ErrorMessage = "District is required")]
        [MaxLength(100)]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6,
            ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;
    }
}