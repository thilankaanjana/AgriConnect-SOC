
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class CreateOrderDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SupplyRequestId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QuotationId { get; set; }
    }
}