
using System.ComponentModel.DataAnnotations;

namespace FarmerService.DTOs
{
    public class CreateCropOrderDto
    {
        [Range(1, int.MaxValue)]
        public int CropId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BuyerName { get; set; } = "";

        [Required]
        [RegularExpression(
            @"^(\+?94|0)?7\d{8}$",
            ErrorMessage = "Invalid mobile number")]
        public string BuyerPhone { get; set; } = "";

        [Range(typeof(decimal), "0.1", "999999")]
        public decimal Quantity { get; set; }

        [MaxLength(250)]
        public string BuyerAddress { get; set; } = "";

        [MaxLength(500)]
        public string Note { get; set; } = "";
    }
}