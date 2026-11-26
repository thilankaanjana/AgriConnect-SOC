
using System;
using System.Collections.Generic;

namespace SupplierService.DTOs
{
    public class SupplierDashboardDto
    {
        // Dashboard Statistics
        public int StockItems { get; set; }

        public int LowOrOutOfStock { get; set; }

        public int OpenRequests { get; set; }

        public int MyQuotations { get; set; }

        public int ApprovedQuotations { get; set; }

        public int ActiveDeliveries { get; set; }

        // Latest 5 available regional requests
        public List<DashboardSupplyRequestDto>
            RecentOpenRequests
        { get; set; } = new();

        // Latest 5 active deliveries
        public List<DashboardDeliveryDto>
            RecentDeliveries
        { get; set; } = new();
    }

    public class DashboardSupplyRequestDto
    {
        public int Id { get; set; }

        public string RefNo { get; set; } = "";

        public string Item { get; set; } = "";

        public string Qty { get; set; } = "";

        public string District { get; set; } = "";

        public bool CanQuote { get; set; }
    }

    public class DashboardDeliveryDto
    {
        public int Id { get; set; }

        public string RefNo { get; set; } = "";

        public string Item { get; set; } = "";

        public string FarmerName { get; set; } = "";

        public string DeliveryStatus { get; set; } = "";
    }
}