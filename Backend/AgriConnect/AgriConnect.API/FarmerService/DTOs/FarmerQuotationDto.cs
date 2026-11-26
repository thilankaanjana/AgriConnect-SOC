
namespace FarmerService.DTOs
{
    public class FarmerQuotationDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string RequestRef { get; set; } = "";

        public string Item { get; set; } = "";

        public string Supplier { get; set; } = "";

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Delivery { get; set; } = "";

        public string Status { get; set; } = "";

        public bool IsLowest { get; set; }

        public bool CanConfirm { get; set; }

        public string ActionText { get; set; } = "";
    }

    public class FarmerQuotationListDto
    {
        public int WaitingVerificationCount { get; set; }

        public List<FarmerQuotationDto> Quotations
        { get; set; } = new();
    }
}