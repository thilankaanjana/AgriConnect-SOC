
namespace FarmerService.DTOs
{
    public class MarketplaceCropDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public string Qty { get; set; } = "";

        public decimal Price { get; set; }

        public string Farmer { get; set; } = "";

        public string Location { get; set; } = "";

        public string Contact { get; set; } = "";

        public string Tag { get; set; } = "";

        public string Status { get; set; } = "";

        public string ImageUrl { get; set; } = "";
    }
}