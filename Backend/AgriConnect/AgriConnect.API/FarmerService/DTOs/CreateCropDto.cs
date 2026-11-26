
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FarmerService.DTOs
{
    public class CreateCropDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Qty { get; set; } = string.Empty;

        [Range(typeof(decimal), "1", "999999999")]
        public decimal Price { get; set; }

        public string Tag { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Contact { get; set; } = string.Empty;

        public IFormFile? Photo { get; set; }
    }
}