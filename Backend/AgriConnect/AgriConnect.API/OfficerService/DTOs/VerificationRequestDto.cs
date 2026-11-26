
namespace OfficerService.DTOs
{
    public class VerificationRequestDto
    {
        public int Id { get; set; }

        public string RefNo { get; set; } = "";

        public string Farmer { get; set; } = "";

        public string ItemText { get; set; } = "";

        public string District { get; set; } = "";

        public string CreatedText { get; set; } = "";

        public DateTime Created { get; set; }

        public string Status { get; set; } = "";

        public string Note { get; set; } = "";

        public bool CanReview { get; set; }

        // Additional review panel details
        public string Item { get; set; } = "";

        public string Qty { get; set; } = "";

        public string Location { get; set; } = "";

        public string PreferredDate { get; set; } = "";

        public int QuotationsReceived { get; set; }
    }
}