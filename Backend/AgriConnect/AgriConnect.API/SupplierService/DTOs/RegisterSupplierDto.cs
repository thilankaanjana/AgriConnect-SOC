
using System.ComponentModel.DataAnnotations;

namespace SupplierService.DTOs
{
    public class RegisterSupplierDto
    {
        [Required(ErrorMessage = "Business name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Invalid Sri Lankan mobile number")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        [MaxLength(100)]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}