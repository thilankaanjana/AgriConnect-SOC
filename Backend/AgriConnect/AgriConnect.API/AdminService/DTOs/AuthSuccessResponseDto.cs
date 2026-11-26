
namespace AdminService.DTOs
{
    public class AuthSuccessResponseDto
    {
        public string AccessToken { get; set; }
            = string.Empty;

        public string TokenType { get; set; }
            = "Bearer";

        public DateTime ExpiresAtUtc { get; set; }

        public string UserId { get; set; }
            = string.Empty;

        public string Name { get; set; }
            = string.Empty;

        public string Role { get; set; }
            = string.Empty;

        public bool AccountCreated { get; set; }
    }
}