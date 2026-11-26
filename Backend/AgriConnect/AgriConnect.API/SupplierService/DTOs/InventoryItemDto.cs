
namespace SupplierService.DTOs
{
    public class InventoryItemDto
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string Item { get; set; } = "";

        public int Qty { get; set; }

        public string Unit { get; set; } = "";

        public decimal Price { get; set; }

        public string Level { get; set; } = "";
    }
}