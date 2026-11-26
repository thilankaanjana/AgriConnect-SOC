
using System.Collections.Generic;

namespace OfficerService.DTOs
{
    public class OfficerDashboardDto
    {
        // Dashboard statistics
        public int PendingRequests { get; set; }

        public int PendingQuotations { get; set; }

        public int ApprovedItems { get; set; }

        public int RejectedItems { get; set; }

        public int Recommendations { get; set; }

        // Waiting for verification
        public List<DashboardPendingRequestDto>
            PendingRequestItems
        { get; set; } = new();

        public List<DashboardPendingQuotationDto>
            PendingQuotationItems
        { get; set; } = new();
    }

    // Supply request details displayed on dashboard
    public class DashboardPendingRequestDto
    {
        public int Id { get; set; }

        public string RefNo { get; set; } = "";

        public string Item { get; set; } = "";

        public string Qty { get; set; } = "";

        public string Farmer { get; set; } = "";

        public string District { get; set; } = "";

        public string Status { get; set; } = "";
    }

    // Quotation details displayed on dashboard
    public class DashboardPendingQuotationDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string RefNo { get; set; } = "";

        public string RequestRef { get; set; } = "";

        public string Supplier { get; set; } = "";

        public decimal Price { get; set; }

        public string Status { get; set; } = "";
    }
}