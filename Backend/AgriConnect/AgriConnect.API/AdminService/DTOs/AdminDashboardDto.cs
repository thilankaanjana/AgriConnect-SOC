
using System.Collections.Generic;

namespace AdminService.DTOs
{
    public class AdminDashboardDto
    {
        // Dashboard statistics

        public int Users { get; set; }

        public int Farmers { get; set; }

        public int Suppliers { get; set; }

        public int Officers { get; set; }

        public int SupplyRequests { get; set; }

        public int Quotations { get; set; }

        public int ConfirmedOrders { get; set; }

        public decimal OrderValue { get; set; }

        // Latest 8 platform activities

        public List<ActivityLogEntryDto> RecentActivities
        { get; set; } = new();
    }
}