
namespace SupplierService.DTOs
{
    public class SupplierDeliveryDto
    {
        public int Id { get; set; }

        public string RefNo { get; set; } = string.Empty;

        public string FarmerName { get; set; } = string.Empty;

        public string ItemText { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string DeliveryStatus { get; set; } = string.Empty;
    }
}