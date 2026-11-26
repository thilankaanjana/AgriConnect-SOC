
using System.ComponentModel.DataAnnotations;

namespace SupplierService.DTOs
{
    public class CreateInventoryItemDto
    {
        [Required(ErrorMessage = "Item name is required")]
        [MaxLength(150)]
        public string Item { get; set; } = string.Empty;

        [Range(0, 999999999,
            ErrorMessage = "Quantity must be 0 or more")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [RegularExpression(
            @"^(kg|units|bags|litres)$",
            ErrorMessage = "Invalid unit")]
        public string Unit { get; set; } = string.Empty;

        [Range(typeof(decimal), "1", "999999999",
            ErrorMessage = "Price must be at least Rs. 1")]
        public decimal Price { get; set; }
    }
}