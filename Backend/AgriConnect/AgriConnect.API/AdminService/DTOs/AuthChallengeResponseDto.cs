
namespace AdminService.DTOs
{
    public class AuthChallengeResponseDto
    {
        public Guid ChallengeId { get; set; }

        public string MaskedContact { get; set; }
            = string.Empty;

        public string Purpose { get; set; }
            = string.Empty;

        public int ExpiresInSeconds { get; set; } = 300;

        public int ResendAfterSeconds { get; set; } = 30;
    }
}