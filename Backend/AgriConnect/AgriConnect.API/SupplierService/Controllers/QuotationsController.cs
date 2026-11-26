
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/quotations")]
    [Authorize(Roles = "Supplier")]
    public class QuotationsController : ControllerBase
    {
        // GET: api/quotations/supply-requests
        // SUPPLIER ONLY
        [HttpGet("supply-requests")]
        public IActionResult GetSupplyRequests(
            [FromQuery] bool onlyOpen = false)
        {
            // TODO:
            // Identify Supplier from JWT.
            // Call FarmerIntegrationService.
            // Fetch regional Farmer Supply Requests.
            // Apply onlyOpen filter.
            // Return SupplierSupplyRequestDto list.

            return NotReady("Supply request integration");
        }

        // GET: api/quotations
        // SUPPLIER ONLY
        [HttpGet]
        public IActionResult GetMyQuotations()
        {
            // TODO:
            // Identify Supplier from JWT.
            // Call QuotationService.
            // Return only this Supplier's quotations.
            // Include Officer verification status.

            return NotReady("Get supplier quotations");
        }

        // GET: api/quotations/1
        // SUPPLIER ONLY
        [HttpGet("{id:int}")]
        public IActionResult GetQuotationById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid quotation ID"
                });
            }

            // TODO:
            // Check quotation ownership.
            // Call QuotationService.
            // Return SupplierQuotationDto.

            return NotReady("Get quotation details");
        }

        // POST: api/quotations
        // SUPPLIER ONLY
        [HttpPost]
        public IActionResult CreateQuotation(
            [FromBody] CreateQuotationDto dto)
        {
            // DTO validation is automatic.
            //
            // TODO:
            // Identify Supplier using JWT.
            // Check account is Active.
            // Check supply request exists.
            // Validate regional eligibility.
            // Check request accepts quotations.
            // Check stock availability.
            // Prevent duplicate quotations.
            // Call QuotationService.
            // Set status to Pending Verification.
            // Return 201 Created.

            return NotReady("Create supplier quotation");
        }

        // PUT: api/quotations/1
        // SUPPLIER ONLY
        [HttpPut("{id:int}")]
        public IActionResult UpdateQuotation(
            int id,
            [FromBody] UpdateQuotationDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid quotation ID"
                });
            }

            // TODO:
            // Identify Supplier using JWT.
            // Check quotation ownership.
            // Validate quotation status.
            // Check price and available stock.
            // Re-submit for verification if required.
            // Call QuotationService.

            return NotReady("Update supplier quotation");
        }

        private IActionResult NotReady(string operation)
        {
            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = operation +
                        " service is not connected yet.",
                    success = false,
                    savedToDatabase = false
                });
        }
    }
}