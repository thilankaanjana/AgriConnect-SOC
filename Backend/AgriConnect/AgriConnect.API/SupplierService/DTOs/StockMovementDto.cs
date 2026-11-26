
namespace SupplierService.DTOs
{
    public class StockMovementDto
    {
        public int Id { get; set; }

        public int InventoryItemId { get; set; }

        public DateTime Created { get; set; }

        public string Item { get; set; } = "";

        public int Change { get; set; }

        public string Unit { get; set; } = "";

        public int Balance { get; set; }

        public string Note { get; set; } = "";
    }
}