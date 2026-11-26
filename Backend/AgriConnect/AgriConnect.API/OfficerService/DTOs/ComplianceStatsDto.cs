
namespace OfficerService.DTOs
{
    public class ComplianceStatsDto
    {
        public int ItemsReviewed { get; set; }

        public int AwaitingReview { get; set; }

        public int Approved { get; set; }

        public int Rejected { get; set; }
    }
}