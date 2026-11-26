
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficerService.DTOs;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("api/recommendations")]
    [Authorize(Roles = "Officer")]
    public class RecommendationsController : ControllerBase
    {
        // GET: api/recommendations/assigned-requests
        // Officer's assigned supply requests
        [HttpGet("assigned-requests")]
        public IActionResult GetAssignedRequests()
        {
            // TODO:
            // 1. Identify Officer from JWT.
            // 2. Call FarmerService integration.
            // 3. Get requests assigned to this Officer.
            // 4. Return List<OfficerRequestOptionDto>.

            return NotReady("Get assigned supply requests");
        }

        // GET: api/recommendations
        // Officer's recommendation history
        [HttpGet]
        public IActionResult GetMyRecommendations()
        {
            // TODO:
            // 1. Identify Officer from JWT.
            // 2. Call RecommendationService.
            // 3. Fetch this Officer's recommendations.
            // 4. Sort newest first.
            // 5. Return List<RecommendationDto>.

            return NotReady("Get officer recommendations");
        }

        // POST: api/recommendations
        // Add recommendation for supply request
        [HttpPost]
        public IActionResult CreateRecommendation(
            [FromBody] CreateRecommendationDto dto)
        {
            // DTO validation is automatic.
            //
            // TODO:
            // 1. Identify Officer from JWT.
            // 2. Verify supply request exists.
            // 3. Confirm request is assigned
            //    to this Officer.
            // 4. Call RecommendationService.
            // 5. Save recommendation.
            // 6. Notify the relevant Farmer.
            // 7. Return 201 Created.

            return NotReady("Create recommendation");
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