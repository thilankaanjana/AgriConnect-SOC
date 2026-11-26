
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SupplierService.DTOs;

namespace SupplierService.Controllers
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        // ---------------------------------
        // 1. GET MY INVENTORY
        // SUPPLIER ONLY
        // GET: api/inventory
        // ---------------------------------

        [HttpGet]
        [Authorize(Roles = "Supplier")]
        public IActionResult GetMyInventory()
        {
            // TODO:
            // 1. Get SupplierId from JWT "sub".
            // 2. Call InventoryService.
            // 3. Get only this supplier's items.
            // 4. Return List<InventoryItemDto>.

            return NotReady("Get supplier inventory");
        }

        // ---------------------------------
        // 2. GET INVENTORY ITEM BY ID
        // SUPPLIER ONLY
        // GET: api/inventory/1
        // ---------------------------------

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Supplier")]
        public IActionResult GetInventoryById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid inventory item ID"
                });
            }

            // TODO:
            // Check ownership using JWT SupplierId.
            // Return matching InventoryItemDto.
            // Return 404 if item is not owned.

            return NotReady("Get inventory item");
        }

        // ---------------------------------
        // 3. CREATE INVENTORY ITEM
        // SUPPLIER ONLY
        // POST: api/inventory
        // ---------------------------------

        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public IActionResult CreateInventory(
            [FromBody] CreateInventoryItemDto dto)
        {
            // TODO:
            // 1. Get SupplierId from JWT.
            // 2. Call InventoryService.
            // 3. Save inventory item.
            // 4. Record initial stock movement.
            // 5. Return 201 Created.

            return NotReady("Create inventory item");
        }

        // ---------------------------------
        // 4. UPDATE INVENTORY ITEM
        // SUPPLIER ONLY
        // PUT: api/inventory/1
        // ---------------------------------

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Supplier")]
        public IActionResult UpdateInventory(
            int id,
            [FromBody] UpdateInventoryItemDto dto)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid inventory item ID"
                });
            }

            // TODO:
            // 1. Check Supplier ownership.
            // 2. Update Qty and Price.
            // 3. Record stock movement.
            // 4. Return updated InventoryItemDto.

            return NotReady("Update inventory item");
        }

        // ---------------------------------
        // 5. DELETE INVENTORY ITEM
        // SUPPLIER ONLY
        // DELETE: api/inventory/1
        // ---------------------------------

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Supplier")]
        public IActionResult DeleteInventory(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid inventory item ID"
                });
            }

            // TODO:
            // Check Supplier ownership.
            // Delete using InventoryService.
            // Return 204 No Content.

            return NotReady("Delete inventory item");
        }

        // ---------------------------------
        // 6. GET STOCK MOVEMENT HISTORY
        // SUPPLIER ONLY
        // GET: api/inventory/stock-history
        // ---------------------------------

        [HttpGet("stock-history")]
        [Authorize(Roles = "Supplier")]
        public IActionResult GetStockHistory()
        {
            // TODO:
            // 1. Identify Supplier from JWT.
            // 2. Get Supplier stock movements.
            // 3. Return List<StockMovementDto>.

            return NotReady("Get stock history");
        }

        // ---------------------------------
        // 7. FARMER SUPPLIER STOCK SEARCH
        // FARMER ONLY
        // GET: api/inventory/supplier-stock
        // ---------------------------------

        [HttpGet("supplier-stock")]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(
            typeof(List<SupplierStockItemDto>),
            StatusCodes.Status200OK)]
        public IActionResult GetSupplierStock(
            [FromQuery] string? search = null,
            [FromQuery] string? district = null)
        {
            search = (search ?? "").Trim();
            district = (district ?? "").Trim();

            if (search.Length > 100)
            {
                return BadRequest(new
                {
                    message =
                        "Search must not exceed 100 characters."
                });
            }

            if (district.Length > 100)
            {
                return BadRequest(new
                {
                    message =
                        "District must not exceed 100 characters."
                });
            }

            // TODO: Business Logic integration
            //
            // 1. Call InventoryService.
            //
            // 2. Get inventory of ALL suppliers
            //    whose account Status == Active.
            //
            // 3. Include supplier details:
            //    Supplier name
            //    District
            //    Contact
            //
            // 4. Search by Item or Supplier name.
            //    Search should be case-insensitive.
            //
            // 5. Apply District filter when given.
            //
            // 6. Calculate Level:
            //    Qty <= 0  : Out of stock
            //    Qty <= 20 : Low stock
            //    Qty > 20  : In stock
            //
            // 7. Include zero-stock items
            //    to match the current UI.
            //
            // 8. Sort by Item name ascending.
            //
            // 9. Return List<SupplierStockItemDto>.
            //
            // Never let the client supply
            // or modify SupplierId here.
            //
            // Do not return passwords
            // or other private account data.

            return NotReady("Get supplier stock");
        }

        // ---------------------------------
        // TEMPORARY RESPONSE
        // ---------------------------------

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