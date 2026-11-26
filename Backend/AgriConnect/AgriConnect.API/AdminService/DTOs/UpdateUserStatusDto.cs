
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class UpdateUserStatusDto
    {
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression(
            @"^(Active|Rejected|Suspended)$",
            ErrorMessage =
                "Status must be Active, Rejected or Suspended")]
        public string Status { get; set; } = string.Empty;
    }
}