
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class UpdateOfficerProfileDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid Sri Lankan mobile number")]
        public string Contact { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        [MaxLength(100)]
        public string District { get; set; } = string.Empty;
    }
}