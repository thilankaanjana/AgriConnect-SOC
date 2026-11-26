
namespace AdminService.DTOs
{
    public class AdminProfileDto
    {
        public string Id { get; set; } = "";

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public string Contact { get; set; } = "";

        public string Address { get; set; } = "";

        public string District { get; set; } = "";

        public string Role { get; set; } = "Admin";

        public string Status { get; set; } = "";
    }
}