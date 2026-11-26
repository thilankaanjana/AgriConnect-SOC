
using System.Collections.Generic;

namespace AdminService.DTOs
{
    public class TransactionsPageDto
    {
        public TransactionSummaryDto Stats { get; set; }
            = new();

        public string SelectedType { get; set; } = "";

        public List<TransactionRowDto> Transactions
        { get; set; } = new();
    }
}