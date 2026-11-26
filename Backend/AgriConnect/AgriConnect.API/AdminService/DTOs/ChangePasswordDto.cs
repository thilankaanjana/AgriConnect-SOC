
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; } = "";

        [Required(ErrorMessage = "New password is required")]
        [MinLength(6,
            ErrorMessage = "New password must be at least 6 characters")]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; } = "";
    }
}