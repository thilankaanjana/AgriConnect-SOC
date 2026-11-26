
namespace AdminService.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public string Text { get; set; } = "";

        public string Link { get; set; } = "";

        public DateTime Created { get; set; }

        public bool Read { get; set; }
    }
}