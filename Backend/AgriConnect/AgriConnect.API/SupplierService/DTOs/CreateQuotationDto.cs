
using System.ComponentModel.DataAnnotations;

namespace SupplierService.DTOs
{
    public class CreateQuotationDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Valid supply request ID is required")]
        public int RequestId { get; set; }

        [Range(typeof(decimal), "1", "999999999",
            ErrorMessage = "Price must be at least Rs. 1")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Delivery estimate is required")]
        [MaxLength(100)]
        public string Delivery { get; set; } = string.Empty;
    }
}