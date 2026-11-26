
using System.Collections.Generic;

namespace AdminService.DTOs
{
    public class MarketplaceAnalyticsDto
    {
        public List<AnalyticsCountDto>
            ActiveListingsByCategory
        { get; set; } = new();

        public int SoldOut { get; set; }
    }
}