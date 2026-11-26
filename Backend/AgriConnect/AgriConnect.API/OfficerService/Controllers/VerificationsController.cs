
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficerService.DTOs;
using System.Linq;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("api/verifications")]
    [Authorize(Roles = "Officer")]
    public class VerificationsController : ControllerBase
    {
        // GET: api/verifications/requests
        [HttpGet("requests")]
        public IActionResult GetAssignedRequests()
        {
            // TODO:
            // 1. Get Officer ID from JWT.
            // 2. Fetch requests assigned to Officer.
            // 3. Sort newest first.
            // 4. Return List<VerificationRequestDto>.

            return NotReady("Get verification requests");
        }

        // GET: api/verifications/requests/1
        [HttpGet("requests/{id:int}")]
        public IActionResult GetRequestDetails(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid request ID"
                });

            // TODO:
            // Check request assigned to this Officer.
            // Return VerificationRequestDto.

            return NotReady("Get request review details");
        }

        // GET: api/verifications/quotations
        [HttpGet("quotations")]
        public IActionResult GetAssignedQuotations()
        {
            // TODO:
            // 1. Get Officer ID from JWT.
            // 2. Find requests assigned to Officer.
            // 3. Get quotations for those requests.
            // 4. Sort newest first.
            // 5. Return List<VerificationQuotationDto>.

            return NotReady("Get verification quotations");
        }

        // GET: api/verifications/quotations/1
        [HttpGet("quotations/{id:int}")]
        public IActionResult GetQuotationDetails(int id)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid quotation ID"
                });

            // TODO:
            // Verify quotation belongs to a request
            // assigned to this Officer.
            // Include requested quantity and
            // other quotations for comparison.

            return NotReady("Get quotation review details");
        }

        // GET: api/verifications/checklists
        [HttpGet("checklists")]
        public IActionResult GetChecklists()
        {
            // These checklists are defined by
            // the existing UI workflow.

            var requestChecks = new[]
            {
                new
                {
                    Id = 1,
                    Text = "The requested item is a permitted agricultural input"
                },
                new
                {
                    Id = 2,
                    Text = "The quantity is suitable for the farm's needs"
                },
                new
                {
                    Id = 3,
                    Text = "The delivery location is within the service region"
                }
            };

            var quotationChecks = new[]
            {
                new
                {
                    Id = 1,
                    Text = "The quoted price is within the normal market range"
                },
                new
                {
                    Id = 2,
                    Text = "The supplier's stock covers the requested quantity"
                },
                new
                {
                    Id = 3,
                    Text = "The delivery estimate is acceptable"
                }
            };

            return Ok(new
            {
                requestChecks,
                quotationChecks
            });
        }

        // POST: api/verifications/requests/1/decision
        [HttpPost("requests/{id:int}/decision")]
        public IActionResult ReviewRequest(
            int id,
            [FromBody] VerificationDecisionDto dto)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid request ID"
                });

            var error = ValidateDecision(dto);
            if (error != null)
                return error;

            // TODO:
            // 1. Get Officer ID from JWT.
            // 2. Verify request is assigned to Officer.
            // 3. Verify request status is Pending.
            // 4. Verify compliance evidence.
            // 5. Call verification business service.
            // 6. Record Approved / Rejected decision.
            // 7. Store Officer note and audit history.
            // 8. Notify Farmer.
            // 9. If rejected, close related quotations.

            return NotReady("Review supply request");
        }

        // POST: api/verifications/quotations/1/decision
        [HttpPost("quotations/{id:int}/decision")]
        public IActionResult ReviewQuotation(
            int id,
            [FromBody] VerificationDecisionDto dto)
        {
            if (id <= 0)
                return BadRequest(new
                {
                    message = "Invalid quotation ID"
                });

            var error = ValidateDecision(dto);
            if (error != null)
                return error;

            // TODO:
            // 1. Get Officer ID from JWT.
            // 2. Find quotation and related request.
            // 3. Verify request assigned to Officer.
            // 4. Verify quotation status is Pending.
            // 5. Verify compliance evidence.
            // 6. Call verification business service.
            // 7. Save decision, note and audit record.
            // 8. Notify Supplier.
            // 9. If approved, notify Farmer.

            return NotReady("Review supplier quotation");
        }

        // Check checklist and note validation
        private IActionResult? ValidateDecision(
            VerificationDecisionDto dto)
        {
            var selected = dto.PassedCheckIds
                ?? new List<int>();

            // Only checklist IDs 1, 2, 3 are valid.
            if (selected.Any(id => id < 1 || id > 3)
                || selected.Distinct().Count() != selected.Count)
            {
                return BadRequest(new
                {
                    message =
                        "Invalid or duplicate compliance check IDs."
                });
            }

            // Every check must be passed to approve.
            if (dto.Decision == "Approved")
            {
                bool allPassed =
                    selected.OrderBy(id => id)
                    .SequenceEqual(new[] { 1, 2, 3 });

                if (!allPassed)
                {
                    return BadRequest(new
                    {
                        message =
                            "All 3 compliance checks must pass before approval."
                    });
                }
            }

            // Rejection must include an explanation.
            if (dto.Decision == "Rejected"
                && string.IsNullOrWhiteSpace(dto.Note))
            {
                return BadRequest(new
                {
                    message =
                        "Officer note is required when rejecting."
                });
            }

            return null;
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
                    decisionSaved = false,
                    savedToDatabase = false
                });
        }
    }
}