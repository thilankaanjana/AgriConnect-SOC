
namespace OfficerService.DTOs
{
    public class VerificationQuotationDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string RefNo { get; set; } = "";

        public string RequestRef { get; set; } = "";

        public string Supplier { get; set; } = "";

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Delivery { get; set; } = "";

        public string Status { get; set; } = "";

        public bool CanReview { get; set; }

        public DateTime Created { get; set; }

        // Additional review panel details
        public string RequestedItem { get; set; } = "";

        public string RequestedQty { get; set; } = "";

        public bool StockCoversRequest { get; set; }

        public List<OtherQuotationDto> OtherQuotations
        { get; set; } = new();
    }

    public class OtherQuotationDto
    {
        public string Supplier { get; set; } = "";

        public decimal Price { get; set; }

        public string Delivery { get; set; } = "";
    }
}