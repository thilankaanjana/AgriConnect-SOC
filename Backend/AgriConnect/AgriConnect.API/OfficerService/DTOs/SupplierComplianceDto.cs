
namespace OfficerService.DTOs
{
    public class SupplierComplianceDto
    {
        public int SupplierId { get; set; }

        public string Supplier { get; set; } = "";

        public int Total { get; set; }

        public int Approved { get; set; }

        public int Rejected { get; set; }

        public int Waiting { get; set; }

        // Example: 75% or -
        public string Rate { get; set; } = "-";
    }
}