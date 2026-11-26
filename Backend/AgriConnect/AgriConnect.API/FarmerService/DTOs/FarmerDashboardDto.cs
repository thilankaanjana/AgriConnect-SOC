
namespace FarmerService.DTOs
{
    public class FarmerDashboardDto
    {
        public int MyRequests { get; set; }
        public int AwaitingVerification { get; set; }
        public int QuotationsToCompare { get; set; }
        public int ConfirmedOrders { get; set; }
        public int BuyerOrdersWaiting { get; set; }
        public int ActiveCropListings { get; set; }

        public List<DashboardRequestDto> RecentRequests
        { get; set; } = new();

        public List<DashboardNotificationDto> RecentNotifications
        { get; set; } = new();
    }

    public class DashboardRequestDto
    {
        public int Id { get; set; }
        public string RefNo { get; set; } = "";
        public string Item { get; set; } = "";
        public string Qty { get; set; } = "";
        public string Status { get; set; } = "";
        public string ReviewNote { get; set; } = "";
        public int ProgressStep { get; set; }
    }

    public class DashboardNotificationDto
    {
        public string Text { get; set; } = "";
        public string Link { get; set; } = "";
        public DateTime Created { get; set; }
        public bool Read { get; set; }
    }
}