
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FarmerService.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FarmerService.Controllers
{

    [ApiController]
    [Route("api/orders")]
    [Authorize(Roles = "Farmer")]
    public class OrdersController : ControllerBase
    {
        // GET: api/orders
        [HttpGet]
        public IActionResult GetMyOrders()
        {
            // Later: Get farmer ID from JWT
            // and call OrderService.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "OrderService is not connected yet.",
                    ordersLoaded = false
                });
        }

        // GET: api/orders/1
        [HttpGet("{id:int}")]
        public IActionResult GetOrderById(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid order ID"
                });

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "OrderService is not connected yet.",
                    orderLoaded = false
                });
        }

        // POST: api/orders
        [HttpPost]
        public IActionResult CreateOrder(
            [FromBody] CreateOrderDto dto)
        {
            // Input validation runs automatically.
            // Later: OrderService checks:
            // 1. Supply request belongs to farmer
            // 2. Quotation is approved
            // 3. Order has not already been confirmed

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message = "OrderService is not connected yet.",
                    orderCreated = false,
                    savedToDatabase = false
                });
        }
    }
}