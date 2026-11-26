
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class CreateGuidelineDto
    {
        [Required(ErrorMessage = "Guideline title is required")]
        [MaxLength(150,
            ErrorMessage = "Title is too long")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Guideline text is required")]
        [MaxLength(5000,
            ErrorMessage = "Guideline text is too long")]
        public string Text { get; set; } = string.Empty;
    }
}