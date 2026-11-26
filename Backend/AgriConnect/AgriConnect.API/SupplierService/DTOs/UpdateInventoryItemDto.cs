
using System.ComponentModel.DataAnnotations;

namespace SupplierService.DTOs
{
    public class UpdateInventoryItemDto
    {
        [Range(0, 999999999,
            ErrorMessage = "Quantity must be 0 or more")]
        public int Qty { get; set; }

        [Range(typeof(decimal), "1", "999999999",
            ErrorMessage = "Price must be at least Rs. 1")]
        public decimal Price { get; set; }
    }
}