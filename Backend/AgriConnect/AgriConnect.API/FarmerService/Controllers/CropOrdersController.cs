
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FarmerService.DTOs;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/crop-orders")]
    [Authorize(Roles = "Farmer")]
    public class CropOrdersController : ControllerBase
    {
        // POST: api/crop-orders
        // Public buyers can place orders
        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateCropOrder(
            [FromBody] CreateCropOrderDto dto)
        {
            // Later: Call CropOrderService
            // Validate crop availability and quantity
            // Calculate total using actual crop price

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Crop order service is not connected yet.",
                    orderCreated = false
                });
        }

        // GET: api/crop-orders/my
        // Farmer only
        [HttpGet("my")]
        public IActionResult GetMyCropOrders()
        {
            // Later: Identify farmer from JWT
            // Return only the farmer's crop orders

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Crop order service is not connected yet.",
                    ordersLoaded = false
                });
        }

        // PATCH: api/crop-orders/1/status
        // Farmer only
        [HttpPatch("{id:int}/status")]
        public IActionResult UpdateStatus(
            int id,
            [FromBody] UpdateCropOrderStatusDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid order ID"
                });
            }

            // Later: Call CropOrderService
            // Check farmer ownership
            // Validate allowed status transitions

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Crop order service is not connected yet.",
                    orderUpdated = false
                });
        }
    }
}