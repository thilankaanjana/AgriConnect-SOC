
namespace AdminService.DTOs
{
    public class TransactionSummaryDto
    {
        public int SupplyRequests { get; set; }

        public int Quotations { get; set; }

        public int ConfirmedOrders { get; set; }

        public decimal OrderValue { get; set; }
    }
}