
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using FarmerService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/supply-requests")]
    [Authorize(Roles = "Farmer")]
    public class SupplyRequestsController : ControllerBase
    {
        // Temporary in-memory storage.
        // Business Logic and Database integration pending.

        private static readonly List<RequestRecord> Requests = new();
        private static readonly object Sync = new();
        private static int NextId = 1;

        private const string PendingStatus =
            "Pending Verification";

        private class RequestRecord
        {
            public int Id { get; set; }

            public int FarmerId { get; set; }

            public string RefNo { get; set; } = "";

            public string ItemName { get; set; } = "";

            public decimal Quantity { get; set; }

            public string Unit { get; set; } = "";

            public string District { get; set; } = "";

            public string DeliveryLocation { get; set; } = "";

            public DateTime PreferredDate { get; set; }

            public DateTime Created { get; set; }

            public string Status { get; set; } =
                PendingStatus;

            public int Quotes { get; set; }

            public string Officer { get; set; } = "";

            public string Note { get; set; } = "";

            public string Advice { get; set; } = "";
        }

        // GET FARMER ID FROM JWT

        private int? GetFarmerId()
        {
            string? idText =
                User.FindFirst("sub")?.Value;

            if (int.TryParse(idText, out int id) &&
                id > 0)
            {
                return id;
            }

            return null;
        }

        // CHECK WHETHER REQUEST CAN CHANGE

        private static bool CanChange(RequestRecord request)
        {
            // Actual quotation count must be
            // checked from the database later.

            return request.Status == PendingStatus
                && request.Quotes == 0;
        }

        // RESPONSE FORMAT - UPDATED STEP 77

        private static object ToResponse(RequestRecord r)
        {
            return new
            {
                r.Id,
                r.FarmerId,
                r.RefNo,

                Item = r.ItemName,
                ItemName = r.ItemName,

                Amount = r.Quantity,
                Quantity = r.Quantity,

                Qty = r.Quantity.ToString(
                    "0.##",
                    CultureInfo.InvariantCulture
                ) + " " + r.Unit,

                r.Unit,
                r.District,

                Location = r.DeliveryLocation,
                r.DeliveryLocation,

                Date = r.PreferredDate.ToString(
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture
                ),

                r.Created,
                r.Status,

                // UI-compatible Officer fields
                Officer = r.Officer,
                AssignedOfficer = r.Officer,

                r.Quotes,

                // UI-compatible Review Note fields
                Note = r.Note,
                ReviewNote = r.Note,

                r.Advice,

                CanChange = CanChange(r)
            };
        }

        // -------------------------------------
        // 1. GET MY SUPPLY REQUESTS
        // GET: api/supply-requests
        // -------------------------------------

        [HttpGet]
        public IActionResult GetAll()
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            lock (Sync)
            {
                var myRequests = Requests
                    .Where(r =>
                        r.FarmerId == farmerId.Value)
                    .OrderByDescending(r => r.Created)
                    .Select(ToResponse)
                    .ToList();

                return Ok(myRequests);
            }
        }

        // -------------------------------------
        // 2. GET REQUEST BY ID
        // GET: api/supply-requests/1
        // -------------------------------------

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            lock (Sync)
            {
                var request = Requests.FirstOrDefault(
                    r => r.Id == id &&
                         r.FarmerId == farmerId.Value);

                if (request == null)
                {
                    return NotFound(new
                    {
                        message = "Supply request not found."
                    });
                }

                return Ok(ToResponse(request));
            }
        }

        // -------------------------------------
        // 3. CREATE SUPPLY REQUEST
        // POST: api/supply-requests
        // -------------------------------------

        [HttpPost]
        public IActionResult CreateSupplyRequest(
            [FromBody] CreateSupplyRequestDto dto)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            if (dto.PreferredDate == default ||
                dto.PreferredDate.Date < DateTime.Today)
            {
                return BadRequest(new
                {
                    message =
                        "Preferred date cannot be in the past."
                });
            }

            lock (Sync)
            {
                int id = NextId++;

                var request = new RequestRecord
                {
                    Id = id,

                    FarmerId = farmerId.Value,

                    RefNo = $"REQ{id}",

                    ItemName = dto.ItemName.Trim(),

                    Quantity = dto.Quantity,

                    Unit = dto.Unit.Trim(),

                    District = dto.District.Trim(),

                    DeliveryLocation =
                        dto.DeliveryLocation.Trim(),

                    PreferredDate = dto.PreferredDate,

                    Created = DateTime.UtcNow,

                    Status = PendingStatus,

                    Quotes = 0
                };

                Requests.Add(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = request.Id },
                    ToResponse(request)
                );
            }
        }

        // -------------------------------------
        // 4. UPDATE SUPPLY REQUEST
        // PUT: api/supply-requests/1
        // -------------------------------------

        [HttpPut("{id:int}")]
        public IActionResult UpdateSupplyRequest(
            int id,
            [FromBody] CreateSupplyRequestDto dto)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            if (dto.PreferredDate == default ||
                dto.PreferredDate.Date < DateTime.Today)
            {
                return BadRequest(new
                {
                    message =
                        "Preferred date cannot be in the past."
                });
            }

            lock (Sync)
            {
                var request = Requests.FirstOrDefault(
                    r => r.Id == id &&
                         r.FarmerId == farmerId.Value);

                if (request == null)
                {
                    return NotFound(new
                    {
                        message = "Supply request not found."
                    });
                }

                if (!CanChange(request))
                {
                    return Conflict(new
                    {
                        message =
                            "This request cannot be edited after verification or quotation."
                    });
                }

                request.ItemName = dto.ItemName.Trim();

                request.Quantity = dto.Quantity;

                request.Unit = dto.Unit.Trim();

                request.District = dto.District.Trim();

                request.DeliveryLocation =
                    dto.DeliveryLocation.Trim();

                request.PreferredDate =
                    dto.PreferredDate;

                // TODO:
                // If district changes, Business Logic
                // must reassign the relevant Officer.

                return Ok(new
                {
                    message = "Supply request updated.",
                    request = ToResponse(request)
                });
            }
        }

        // -------------------------------------
        // 5. DELETE SUPPLY REQUEST
        // DELETE: api/supply-requests/1
        // -------------------------------------

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSupplyRequest(int id)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            lock (Sync)
            {
                var request = Requests.FirstOrDefault(
                    r => r.Id == id &&
                         r.FarmerId == farmerId.Value);

                if (request == null)
                {
                    return NotFound(new
                    {
                        message = "Supply request not found."
                    });
                }

                if (!CanChange(request))
                {
                    return Conflict(new
                    {
                        message =
                            "This request cannot be deleted after verification or quotation."
                    });
                }

                Requests.Remove(request);

                // TODO:
                // Business Logic must handle
                // linked quotations, review records
                // and notifications safely.

                return NoContent();
            }
        }
    }
}