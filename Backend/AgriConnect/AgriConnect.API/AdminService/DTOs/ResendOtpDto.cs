
using System.ComponentModel.DataAnnotations;

namespace AdminService.DTOs
{
    public class ResendOtpDto
    {
        [Required(ErrorMessage = "Challenge ID is required")]
        public Guid? ChallengeId { get; set; }
    }
}