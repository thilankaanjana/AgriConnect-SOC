
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class UpdateAdminProfileDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Enter a valid Sri Lankan mobile number")]
        public string Contact { get; set; } = "";

        [MaxLength(300)]
        public string Address { get; set; } = "";

        [Required(ErrorMessage = "District is required")]
        [MaxLength(100)]
        public string District { get; set; } = "";
    }
}