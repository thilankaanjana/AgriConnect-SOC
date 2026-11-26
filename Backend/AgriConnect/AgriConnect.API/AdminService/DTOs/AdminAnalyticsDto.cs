
using System.Collections.Generic;

namespace AdminService.DTOs
{
    public class AdminAnalyticsDto
    {
        public AnalyticsSummaryDto Summary
        { get; set; } = new();

        public List<AnalyticsCountDto> UsersByRole
        { get; set; } = new();

        public List<AnalyticsCountDto> SupplyRequestsByStatus
        { get; set; } = new();

        public List<AnalyticsCountDto> QuotationsByStatus
        { get; set; } = new();

        public List<AnalyticsCountDto> DeliveriesByStatus
        { get; set; } = new();

        public List<AnalyticsCountDto> RequestsByDistrict
        { get; set; } = new();

        public MarketplaceAnalyticsDto Marketplace
        { get; set; } = new();
    }
}