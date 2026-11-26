
namespace SupplierService.DTOs
{
    public class SupplierQuotationDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string RefNo { get; set; } = "";

        public string RequestRef { get; set; } = "";

        public string ItemText { get; set; } = "";

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string Delivery { get; set; } = "";

        public string Status { get; set; } = "";

        public string ReviewNote { get; set; } = "";

        public DateTime Created { get; set; }
    }
}