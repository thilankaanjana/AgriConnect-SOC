
using System.ComponentModel.DataAnnotations;

namespace SupplierService.DTOs
{
    public class DispatchOrderDto
    {
        [Required(ErrorMessage = "Delivery status is required")]
        [RegularExpression(
            @"^(Processing|Dispatched|Delivered)$",
            ErrorMessage =
                "Status must be Processing, Dispatched or Delivered")]
        public string Status { get; set; } = string.Empty;
    }
}