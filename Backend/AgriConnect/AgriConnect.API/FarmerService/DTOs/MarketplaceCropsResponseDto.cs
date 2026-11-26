
using System.Collections.Generic;

namespace FarmerService.DTOs
{
    public class MarketplaceCropsResponseDto
    {
        public int TotalAvailable { get; set; }

        public int FilteredCount { get; set; }

        public string Q { get; set; } = "";

        public string Sort { get; set; } = "newest";

        public string Cat { get; set; } = "";

        public string Pr { get; set; } = "";

        public string Tag { get; set; } = "";

        public List<MarketplaceCountDto> Categories
        { get; set; } = new();

        public List<MarketplaceCountDto> QualityTags
        { get; set; } = new();

        public List<MarketplaceCropDto> Crops
        { get; set; } = new();
    }
}