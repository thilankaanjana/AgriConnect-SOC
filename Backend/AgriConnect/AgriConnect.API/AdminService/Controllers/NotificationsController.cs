
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdminService.DTOs;

namespace AdminService.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(Roles = "Farmer,Supplier,Officer,Admin")]
    public class NotificationsController : ControllerBase
    {
        // GET: api/notifications
        // GET: api/notifications?limit=4
        [HttpGet]
        public IActionResult GetMyNotifications(
            [FromQuery] int? limit = null)
        {
            if (limit.HasValue &&
                (limit.Value < 1 || limit.Value > 100))
            {
                return BadRequest(new
                {
                    message =
                        "Limit must be between 1 and 100."
                });
            }

            // TODO:
            // 1. Read user ID from JWT "sub".
            // 2. Read role from JWT "role".
            // 3. Call NotificationService.
            // 4. Fetch only this user's notifications.
            // 5. Sort newest first.
            // 6. Apply limit if provided.
            // 7. Return List<NotificationDto>.
            //
            // Do not mark notifications as read here.
            // The UI must display the list first.

            return NotReady("Get notifications");
        }

        // GET: api/notifications/unread-count
        [HttpGet("unread-count")]
        public IActionResult GetUnreadCount()
        {
            // TODO:
            // 1. Read authenticated user ID and role.
            // 2. Call NotificationService.
            // 3. Count notifications where Read = false.
            // 4. Return NotificationCountDto.

            return NotReady("Get unread notification count");
        }

        // PATCH: api/notifications/mark-read
        [HttpPatch("mark-read")]
        public IActionResult MarkNotificationsRead()
        {
            // TODO:
            // 1. Read authenticated user ID and role.
            // 2. Call NotificationService.
            // 3. Mark only this user's notifications
            //    as Read = true.
            // 4. Save changes to database.
            // 5. Return updated unread count.
            //
            // Never accept UserId from request body.

            return NotReady("Mark notifications as read");
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