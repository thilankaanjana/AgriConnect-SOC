
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OfficerService.DTOs;

namespace OfficerService.Controllers
{
    [ApiController]
    [Route("api/guidelines")]
    public class GuidelinesController : ControllerBase
    {
        // Temporary in-memory storage.
        // Business Logic + Database integration pending.

        private static readonly List<GuidelineRecord>
            Guidelines = new();

        private static readonly object Sync = new();

        private static int NextId = 1;

        private class GuidelineRecord
        {
            public int Id { get; set; }

            public string OfficerId { get; set; } = "";

            public string Title { get; set; } = "";

            public string Author { get; set; } = "";

            public string Text { get; set; } = "";

            public DateTime Created { get; set; }
        }

        // ---------------------------------
        // GET OFFICER ID FROM JWT
        // ---------------------------------

        private string? GetOfficerId()
        {
            return User.FindFirst("sub")?.Value;
        }

        private string GetOfficerName()
        {
            return User.FindFirst("name")?.Value
                ?? User.FindFirst(ClaimTypes.Name)?.Value
                ?? "Officer";
        }

        // ---------------------------------
        // CONVERT RECORD TO DTO
        // ---------------------------------

        private static GuidelineDto ToDto(
            GuidelineRecord guideline)
        {
            return new GuidelineDto
            {
                Id = guideline.Id,
                Title = guideline.Title,
                Author = guideline.Author,
                Date = guideline.Created.ToString("yyyy-MM-dd"),
                Text = guideline.Text
            };
        }

        // ---------------------------------
        // 1. GET ALL PUBLIC GUIDELINES
        // GET: api/guidelines
        // GET: api/guidelines?limit=4
        // ---------------------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetGuidelines(
            [FromQuery] int? limit = null)
        {
            if (limit.HasValue &&
                (limit.Value < 1 || limit.Value > 100))
            {
                return BadRequest(new
                {
                    message = "Limit must be between 1 and 100."
                });
            }

            lock (Sync)
            {
                var query = Guidelines
                    .OrderByDescending(g => g.Created)
                    .ThenByDescending(g => g.Id);

                var result = limit.HasValue
                    ? query.Take(limit.Value)
                        .Select(ToDto).ToList()
                    : query.Select(ToDto).ToList();

                return Ok(result);
            }
        }

        // ---------------------------------
        // 2. GET GUIDELINE DETAILS
        // GET: api/guidelines/1
        // ---------------------------------

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult GetGuidelineById(int id)
        {
            lock (Sync)
            {
                var guideline = Guidelines
                    .FirstOrDefault(g => g.Id == id);

                if (guideline == null)
                {
                    return NotFound(new
                    {
                        message = "Guideline not found."
                    });
                }

                return Ok(ToDto(guideline));
            }
        }

        // ---------------------------------
        // 3. CREATE GUIDELINE
        // POST: api/guidelines
        // ---------------------------------

        [HttpPost]
        [Authorize(Roles = "Officer")]
        public IActionResult PublishGuideline(
            [FromBody] CreateGuidelineDto dto)
        {
            var officerId = GetOfficerId();

            if (string.IsNullOrWhiteSpace(officerId))
            {
                return Unauthorized(new
                {
                    message = "Officer identity is missing."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Title) ||
                string.IsNullOrWhiteSpace(dto.Text))
            {
                return BadRequest(new
                {
                    message = "Title and text are required."
                });
            }

            lock (Sync)
            {
                var guideline = new GuidelineRecord
                {
                    Id = NextId++,

                    OfficerId = officerId,

                    Author = GetOfficerName(),

                    Title = dto.Title.Trim(),

                    Text = dto.Text.Trim(),

                    Created = DateTime.UtcNow
                };

                Guidelines.Add(guideline);

                return CreatedAtAction(
                    nameof(GetGuidelineById),
                    new { id = guideline.Id },
                    ToDto(guideline));
            }
        }

        // ---------------------------------
        // 4. EDIT GUIDELINE - NEW
        // PUT: api/guidelines/1
        // ---------------------------------

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Officer")]
        public IActionResult UpdateGuideline(
            int id,
            [FromBody] CreateGuidelineDto dto)
        {
            var officerId = GetOfficerId();

            if (string.IsNullOrWhiteSpace(officerId))
            {
                return Unauthorized(new
                {
                    message = "Officer identity is missing."
                });
            }

            if (string.IsNullOrWhiteSpace(dto.Title) ||
                string.IsNullOrWhiteSpace(dto.Text))
            {
                return BadRequest(new
                {
                    message = "Title and text are required."
                });
            }

            lock (Sync)
            {
                var guideline = Guidelines
                    .FirstOrDefault(g =>
                        g.Id == id &&
                        g.OfficerId == officerId);

                if (guideline == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Guideline not found or you are not its owner."
                    });
                }

                guideline.Title = dto.Title.Trim();

                guideline.Text = dto.Text.Trim();

                // Keep original Author and Created date.

                return Ok(new
                {
                    message = "Guideline updated successfully.",
                    guideline = ToDto(guideline)
                });
            }
        }

        // ---------------------------------
        // 5. DELETE GUIDELINE - NEW
        // DELETE: api/guidelines/1
        // ---------------------------------

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Officer")]
        public IActionResult DeleteGuideline(int id)
        {
            var officerId = GetOfficerId();

            if (string.IsNullOrWhiteSpace(officerId))
            {
                return Unauthorized(new
                {
                    message = "Officer identity is missing."
                });
            }

            lock (Sync)
            {
                var guideline = Guidelines
                    .FirstOrDefault(g =>
                        g.Id == id &&
                        g.OfficerId == officerId);

                if (guideline == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Guideline not found or you are not its owner."
                    });
                }

                Guidelines.Remove(guideline);

                return NoContent();
            }
        }
    }
}