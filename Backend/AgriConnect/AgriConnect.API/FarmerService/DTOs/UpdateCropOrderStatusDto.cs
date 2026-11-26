
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class UpdateCropOrderStatusDto
    {
        [Required]
        [RegularExpression(
            @"^(Accepted|Declined|Completed)$",
            ErrorMessage =
                "Status must be Accepted, Declined or Completed")]
        public string Status { get; set; } = "";
    }
}