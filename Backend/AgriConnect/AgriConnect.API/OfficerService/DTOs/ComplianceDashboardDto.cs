
using System.Collections.Generic;

namespace OfficerService.DTOs
{
    public class ComplianceDashboardDto
    {
        public ComplianceStatsDto Stats { get; set; }
            = new();

        public List<VerificationHistoryDto>
            VerificationHistory
        { get; set; } = new();

        public List<SupplierComplianceDto>
            SupplierCompliance
        { get; set; } = new();
    }
}