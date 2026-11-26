
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class CreateRecommendationDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Valid supply request ID is required")]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "Recommendation is required")]
        [MaxLength(3000,
            ErrorMessage = "Recommendation is too long")]
        public string Text { get; set; } = string.Empty;
    }
}