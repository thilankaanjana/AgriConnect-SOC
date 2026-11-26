
namespace OfficerService.DTOs
{
    public class VerificationHistoryDto
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        // Supply Request or Quotation
        public string Type { get; set; } = "";

        // Example: REQ101 or QUO201
        public string Ref { get; set; } = "";

        public string Decision { get; set; } = "";

        public int Passed { get; set; }

        public int Total { get; set; }

        // Example: 3 / 3
        public string Score => $"{Passed} / {Total}";

        public string Note { get; set; } = "";
    }
}