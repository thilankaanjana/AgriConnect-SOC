
namespace AdminService.DTOs
{
    public class ActivityLogEntryDto
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string User { get; set; } = "";

        public string Role { get; set; } = "";

        public string Action { get; set; } = "";
    }
}