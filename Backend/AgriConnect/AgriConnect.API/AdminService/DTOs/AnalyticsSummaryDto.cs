
namespace AdminService.DTOs
{
    public class AnalyticsSummaryDto
    {
        public int TotalUsers { get; set; }

        public int SupplyRequests { get; set; }

        public int Quotations { get; set; }

        public int ConfirmedOrders { get; set; }

        public decimal OrderValue { get; set; }

        public string QuotationApprovalRate
        { get; set; } = "-";
    }
}