
namespace AdminService.DTOs
{
    public class TransactionRowDto
    {
        public DateTime Created { get; set; }

        public string DateText { get; set; } = "";

        public string Type { get; set; } = "";

        public string Reference { get; set; } = "";

        public string Parties { get; set; } = "";

        public string Details { get; set; } = "";

        public decimal Amount { get; set; }

        public string Status { get; set; } = "";
    }
}