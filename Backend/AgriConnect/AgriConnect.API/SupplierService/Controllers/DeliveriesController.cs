
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/deliveries")]
    [Authorize(Roles = "Supplier")]
    public class DeliveriesController : ControllerBase
    {
        // GET: api/deliveries
        // SUPPLIER ONLY
        [HttpGet]
        public IActionResult GetMyDeliveries()
        {
            // TODO:
            // Identify Supplier from JWT.
            // Call FarmerIntegrationService.
            // Fetch confirmed orders assigned to Supplier.
            // Return List<SupplierDeliveryDto>.

            return NotReady("Get supplier deliveries");
        }

        // GET: api/deliveries/1
        // SUPPLIER ONLY
        [HttpGet("{id:int}")]
        public IActionResult GetDeliveryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid delivery ID"
                });
            }

            // TODO:
            // Identify Supplier from JWT.
            // Check order belongs to Supplier.
            // Fetch delivery details.

            return NotReady("Get delivery details");
        }

        // PATCH: api/deliveries/1/status
        // SUPPLIER ONLY
        [HttpPatch("{id:int}/status")]
        public IActionResult UpdateDeliveryStatus(
            int id,
            [FromBody] DispatchOrderDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid delivery ID"
                });
            }

            // DTO validation is automatic.
            //
            // TODO:
            // Identify Supplier from JWT.
            // Verify order belongs to Supplier.
            // Check current delivery status.
            // Validate allowed status transitions.
            // Prevent changes after Delivered.
            // Call FarmerIntegrationService.
            // Update delivery status.
            // Record dispatch information.
            // Notify Farmer.

            return NotReady("Update delivery status");
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
                    updated = false,
                    savedToDatabase = false
                });
        }
    }
}