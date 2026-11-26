
namespace OfficerService.DTOs
{
    public class OfficerProfileDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string District { get; set; } = string.Empty;

        public string Role { get; set; } = "Officer";

        public string Status { get; set; } = string.Empty;
    }
}