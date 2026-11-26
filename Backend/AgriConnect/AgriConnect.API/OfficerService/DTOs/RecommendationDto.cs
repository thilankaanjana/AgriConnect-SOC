
namespace OfficerService.DTOs
{
    public class RecommendationDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string RequestRef { get; set; } = "";

        public int OfficerId { get; set; }

        public string Officer { get; set; } = "";

        public string Text { get; set; } = "";

        public DateTime Created { get; set; }
    }
}