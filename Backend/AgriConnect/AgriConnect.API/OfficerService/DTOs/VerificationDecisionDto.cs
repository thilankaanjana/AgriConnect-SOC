
using System.ComponentModel.DataAnnotations;

namespace OfficerService.DTOs
{
    public class VerificationDecisionDto
    {
        [Required(ErrorMessage = "Decision is required")]
        [RegularExpression(
            @"^(Approved|Rejected)$",
            ErrorMessage = "Decision must be Approved or Rejected")]
        public string Decision { get; set; } = string.Empty;

        public List<int> PassedCheckIds { get; set; } = new();

        [MaxLength(2000,
            ErrorMessage = "Officer note is too long")]
        public string? Note { get; set; }
    }
}