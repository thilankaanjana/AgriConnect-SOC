
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminService.DTOs;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/admin/transactions")]
    [Authorize(Roles = "Admin")]
    public class TransactionsController : ControllerBase
    {
        // GET: api/admin/transactions
        // GET: api/admin/transactions?type=Request
        // GET: api/admin/transactions?type=Quotation
        // GET: api/admin/transactions?type=Order
        // GET: api/admin/transactions?type=Sale
        [HttpGet]
        public IActionResult GetTransactions(
            [FromQuery] string? type = null)
        {
            // Validate transaction type filter
            if (!string.IsNullOrEmpty(type) &&
                type != "Request" &&
                type != "Quotation" &&
                type != "Order" &&
                type != "Sale")
            {
                return BadRequest(new
                {
                    message =
                        "Invalid transaction type. " +
                        "Use Request, Quotation, Order or Sale."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Authenticate Admin using JWT.
            //
            // 2. Call TransactionMonitoringService.
            //
            // 3. Fetch Supply Requests
            //    from FarmerService.
            //
            // 4. Fetch Quotations
            //    from SupplierService.
            //
            // 5. Fetch Confirmed Orders
            //    from FarmerService.
            //
            // 6. Fetch Buyer Crop Orders
            //    from FarmerService.
            //
            // 7. Build TransactionRowDto list.
            //
            // 8. Apply type filter:
            //    Request   = Supply requests
            //    Quotation = Supplier quotations
            //    Order     = Confirmed orders
            //    Sale      = Buyer crop orders
            //
            // 9. Sort rows by Created descending.
            //
            // 10. Build TransactionSummaryDto:
            //     SupplyRequests = request count
            //     Quotations = quotation count
            //     ConfirmedOrders = order count
            //     OrderValue = sum of order prices
            //
            // 11. Return TransactionsPageDto.

            return NotReady("Get admin transactions");
        }

        // GET: api/admin/transactions/summary
        // Dashboard statistics only
        [HttpGet("summary")]
        public IActionResult GetTransactionSummary()
        {
            // TODO:
            // 1. Authenticate Admin using JWT.
            // 2. Call TransactionMonitoringService.
            // 3. Count supply requests.
            // 4. Count supplier quotations.
            // 5. Count confirmed orders.
            // 6. Sum confirmed order prices.
            // 7. Return TransactionSummaryDto.
            //
            // Buyer crop orders are NOT included
            // in the OrderValue calculation
            // of the existing Admin UI.

            return NotReady("Get transaction summary");
        }

        // Temporary response until
        // business services are integrated.
        private IActionResult NotReady(string operation)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = operation +
                        " service is not connected yet.",
                    success = false,
                    dataLoaded = false,
                    savedToDatabase = false
                });
        }
    }
}