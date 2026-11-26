
namespace AdminService.DTOs
{
    public class AdminUserDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public string Contact { get; set; } = "";

        public string District { get; set; } = "";

        public string Role { get; set; } = "";

        public string Status { get; set; } = "";
    }
}