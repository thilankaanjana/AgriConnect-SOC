
namespace SupplierService.DTOs
{
    public class SupplierStockItemDto
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public string Item { get; set; } = "";

        public string Supplier { get; set; } = "";

        public string District { get; set; } = "";

        public string Contact { get; set; } = "";

        public int Qty { get; set; }

        public string Unit { get; set; } = "";

        public decimal Price { get; set; }

        public string Level
        {
            get
            {
                if (Qty <= 0)
                    return "Out of stock";

                if (Qty <= 20)
                    return "Low stock";

                return "In stock";
            }
        }
    }
}