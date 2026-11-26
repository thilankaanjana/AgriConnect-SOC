
using System;
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class CreateSupplyRequestDto
    {
        [Required]
        public string ItemName { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999")]
        public decimal Quantity { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;

        [Required]
        public string District { get; set; } = string.Empty;

        [Required]
        public string DeliveryLocation { get; set; } = string.Empty;

        [Required]
        public DateTime PreferredDate { get; set; }
    }
}