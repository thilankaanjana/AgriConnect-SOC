
namespace AdminService.DTOs
{
    public class RegistrationPendingResponseDto
    {
        public int UserId { get; set; }

        public string Role { get; set; } = "";

        public string Status { get; set; } = "Pending";

        public bool AccountCreated { get; set; } = true;

        public bool CanLogin { get; set; } = false;

        public bool TokenIssued { get; set; } = false;

        public string Message { get; set; } =
            "Registration successful. Your account is waiting for administrator approval.";
    }
}