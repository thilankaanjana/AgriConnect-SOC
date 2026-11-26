
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace FarmerService.Controllers
{

    [ApiController]
    [Route("api/farmer-quotations")]
    [Authorize(Roles = "Farmer")]
    public class FarmerQuotationsController : ControllerBase
    {
        // GET: api/farmer-quotations
        [HttpGet]
        public IActionResult GetMyQuotations()
        {
            // Later:
            // 1. Identify farmer using JWT.
            // 2. Get farmer's supply request IDs.
            // 3. Fetch related quotations from
            //    SupplierService through Business Logic.
            // 4. Return only officer-verified quotations.
            // 5. Return FarmerQuotationListDto.

            return StatusCode(
                StatusCodes.Status501NotImplemented,
                new
                {
                    message =
                        "Farmer quotation service is not connected yet.",
                    quotationsLoaded = false
                });
        }
    }
}