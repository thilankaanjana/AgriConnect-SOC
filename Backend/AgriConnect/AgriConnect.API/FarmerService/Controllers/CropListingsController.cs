
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FarmerService.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmerService.Controllers
{
    [ApiController]
    [Route("api/crops")]
    [Authorize(Roles = "Farmer")]
    public class CropListingsController : ControllerBase
    {
        // Temporary storage.
        // Business Logic + Database integration pending.

        private static readonly List<CropRecord> Crops = new();
        private static readonly object Sync = new();
        private static int NextId = 1;

        private class CropRecord
        {
            public int Id { get; set; }

            public int FarmerId { get; set; }

            public string Farmer { get; set; } = "";

            public string Name { get; set; } = "";

            // STEP 77 - NEW CATEGORY FIELD
            // Matches the current UI category logic.
            public string Category
            {
                get
                {
                    string n = (Name ?? "").ToLowerInvariant();

                    if (n.Contains("rice") ||
                        n.Contains("paddy"))
                    {
                        return "Grains";
                    }

                    if (n.Contains("strawberr") ||
                        n.Contains("apple") ||
                        n.Contains("blueberr") ||
                        n.Contains("peach"))
                    {
                        return "Fruits";
                    }

                    return "Vegetables";
                }
            }

            public string Qty { get; set; } = "";

            public decimal Price { get; set; }

            public string Tag { get; set; } = "";

            public string Location { get; set; } = "";

            public string Contact { get; set; } = "";

            public string Status { get; set; } = "Available";

            public byte[]? Photo { get; set; }

            public string PhotoType { get; set; } = "";

            public DateTime Created { get; set; }
        }

        // -------------------------------------
        // GET FARMER ID FROM JWT
        // -------------------------------------

        private int? GetFarmerId()
        {
            string? idText =
                User.FindFirst("sub")?.Value;

            if (int.TryParse(idText, out int id) && id > 0)
            {
                return id;
            }

            return null;
        }

        private string GetFarmerName()
        {
            return User.FindFirst("name")?.Value
                   ?? "Farmer";
        }

        // -------------------------------------
        // RESPONSE FORMAT - UPDATED STEP 77
        // -------------------------------------

        private object ToResponse(CropRecord crop)
        {
            return new
            {
                crop.Id,
                crop.FarmerId,
                crop.Farmer,
                crop.Name,

                // New UI-compatible field
                crop.Category,

                crop.Qty,
                crop.Price,
                crop.Tag,
                crop.Location,
                crop.Contact,
                crop.Status,
                crop.Created,

                ImageUrl = crop.Photo == null
                    ? ""
                    : $"{Request.Scheme}://{Request.Host}/api/crops/{crop.Id}/photo"
            };
        }

        // -------------------------------------
        // VALIDATE CROP DETAILS
        // -------------------------------------

        private static string? ValidateCrop(CreateCropDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return "Crop name is required.";
            }

            if (string.IsNullOrWhiteSpace(dto.Qty))
            {
                return "Quantity is required.";
            }

            if (dto.Price < 1)
            {
                return "Price must be at least Rs. 1.";
            }

            if (string.IsNullOrWhiteSpace(dto.Location))
            {
                return "Location is required.";
            }

            if (string.IsNullOrWhiteSpace(dto.Contact))
            {
                return "Contact number is required.";
            }

            if (!string.IsNullOrEmpty(dto.Tag) &&
                dto.Tag != "Organic" &&
                dto.Tag != "Pesticide-free")
            {
                return "Invalid quality label.";
            }

            return null;
        }

        // -------------------------------------
        // VALIDATE AND READ PHOTO
        // -------------------------------------

        private static async Task<
            (byte[]? Bytes, string Type, string? Error)>
            ReadPhotoAsync(IFormFile? photo)
        {
            if (photo == null || photo.Length == 0)
            {
                return (null, "", null);
            }

            const long maxBytes = 3 * 1024 * 1024;

            var allowedTypes = new Dictionary<string, string>
            {
                [".jpg"] = "image/jpeg",
                [".jpeg"] = "image/jpeg",
                [".png"] = "image/png",
                [".webp"] = "image/webp"
            };

            string ext = Path.GetExtension(
                photo.FileName).ToLowerInvariant();

            if (!allowedTypes.TryGetValue(
                    ext, out string? expectedType))
            {
                return (
                    null,
                    "",
                    "Photo must be JPG, PNG or WEBP."
                );
            }

            if (photo.Length > maxBytes)
            {
                return (
                    null,
                    "",
                    "Photo size must not exceed 3 MB."
                );
            }

            if (!string.Equals(
                    photo.ContentType,
                    expectedType,
                    StringComparison.OrdinalIgnoreCase))
            {
                return (
                    null,
                    "",
                    "Photo content type is invalid."
                );
            }

            using var stream = new MemoryStream();

            await photo.CopyToAsync(stream);

            byte[] bytes = stream.ToArray();

            bool validSignature = expectedType switch
            {
                "image/jpeg" =>
                    bytes.Length >= 3 &&
                    bytes[0] == 0xFF &&
                    bytes[1] == 0xD8 &&
                    bytes[2] == 0xFF,

                "image/png" =>
                    bytes.Length >= 8 &&
                    bytes[0] == 0x89 &&
                    bytes[1] == 0x50 &&
                    bytes[2] == 0x4E &&
                    bytes[3] == 0x47 &&
                    bytes[4] == 0x0D &&
                    bytes[5] == 0x0A &&
                    bytes[6] == 0x1A &&
                    bytes[7] == 0x0A,

                "image/webp" =>
                    bytes.Length >= 12 &&
                    bytes[0] == 'R' &&
                    bytes[1] == 'I' &&
                    bytes[2] == 'F' &&
                    bytes[3] == 'F' &&
                    bytes[8] == 'W' &&
                    bytes[9] == 'E' &&
                    bytes[10] == 'B' &&
                    bytes[11] == 'P',

                _ => false
            };

            if (!validSignature)
            {
                return (
                    null,
                    "",
                    "Uploaded file is not a valid image format."
                );
            }

            return (bytes, expectedType, null);
        }

        // -------------------------------------
        // 1. GET PUBLIC CROPS
        // GET: api/crops
        // -------------------------------------

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            lock (Sync)
            {
                var crops = Crops
                    .Where(c => c.Status == "Available")
                    .OrderByDescending(c => c.Id)
                    .Select(ToResponse)
                    .ToList();

                return Ok(crops);
            }
        }

        // -------------------------------------
        // 2. GET MY CROPS
        // GET: api/crops/my
        // -------------------------------------

        [HttpGet("my")]
        public IActionResult GetMyCrops()
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
                var myCrops = Crops
                    .Where(c =>
                        c.FarmerId == farmerId.Value)
                    .OrderByDescending(c => c.Id)
                    .Select(ToResponse)
                    .ToList();

                return Ok(myCrops);
            }
        }

        // -------------------------------------
        // 3. GET CROP DETAILS
        // GET: api/crops/1
        // -------------------------------------

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            lock (Sync)
            {
                var crop = Crops.FirstOrDefault(
                    c => c.Id == id &&
                         c.Status == "Available");

                if (crop == null)
                {
                    return NotFound(new
                    {
                        message = "Available crop not found."
                    });
                }

                return Ok(ToResponse(crop));
            }
        }

        // -------------------------------------
        // 4. GET CROP PHOTO
        // GET: api/crops/1/photo
        // -------------------------------------

        [HttpGet("{id:int}/photo")]
        [AllowAnonymous]
        public IActionResult GetPhoto(int id)
        {
            lock (Sync)
            {
                var crop = Crops.FirstOrDefault(
                    c => c.Id == id);

                if (crop == null || crop.Photo == null)
                {
                    return NotFound();
                }

                return File(
                    crop.Photo,
                    crop.PhotoType
                );
            }
        }

        // -------------------------------------
        // 5. CREATE CROP
        // POST: api/crops
        // -------------------------------------

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateCrop(
            [FromForm] CreateCropDto dto)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            string? error = ValidateCrop(dto);

            if (error != null)
            {
                return BadRequest(new
                {
                    message = error
                });
            }

            var photoResult =
                await ReadPhotoAsync(dto.Photo);

            if (photoResult.Error != null)
            {
                return BadRequest(new
                {
                    message = photoResult.Error
                });
            }

            lock (Sync)
            {
                var crop = new CropRecord
                {
                    Id = NextId++,

                    FarmerId = farmerId.Value,

                    Farmer = GetFarmerName(),

                    Name = dto.Name.Trim(),

                    Qty = dto.Qty.Trim(),

                    Price = dto.Price,

                    Tag = dto.Tag ?? "",

                    Location = dto.Location.Trim(),

                    Contact = dto.Contact.Trim(),

                    Photo = photoResult.Bytes,

                    PhotoType = photoResult.Type,

                    Status = "Available",

                    Created = DateTime.UtcNow
                };

                Crops.Add(crop);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = crop.Id },
                    ToResponse(crop)
                );
            }
        }

        // -------------------------------------
        // 6. UPDATE CROP
        // PUT: api/crops/1
        // -------------------------------------

        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateCrop(
            int id,
            [FromForm] CreateCropDto dto)
        {
            int? farmerId = GetFarmerId();

            if (!farmerId.HasValue)
            {
                return Unauthorized(new
                {
                    message = "Farmer identity is missing."
                });
            }

            string? error = ValidateCrop(dto);

            if (error != null)
            {
                return BadRequest(new
                {
                    message = error
                });
            }

            var photoResult =
                await ReadPhotoAsync(dto.Photo);

            if (photoResult.Error != null)
            {
                return BadRequest(new
                {
                    message = photoResult.Error
                });
            }

            lock (Sync)
            {
                var crop = Crops.FirstOrDefault(
                    c => c.Id == id &&
                         c.FarmerId == farmerId.Value);

                if (crop == null)
                {
                    return NotFound(new
                    {
                        message = "Crop listing not found."
                    });
                }

                crop.Name = dto.Name.Trim();

                crop.Qty = dto.Qty.Trim();

                crop.Price = dto.Price;

                crop.Tag = dto.Tag ?? "";

                crop.Location = dto.Location.Trim();

                crop.Contact = dto.Contact.Trim();

                // New photo uploaded:
                // Replace old photo.
                //
                // No new photo uploaded:
                // Keep existing photo.

                if (photoResult.Bytes != null)
                {
                    crop.Photo = photoResult.Bytes;

                    crop.PhotoType = photoResult.Type;
                }

                return Ok(new
                {
                    message = "Crop listing updated.",
                    crop = ToResponse(crop)
                });
            }
        }

        // -------------------------------------
        // 7. MARK CROP SOLD OUT
        // PUT: api/crops/1/sold-out
        // -------------------------------------

        [HttpPut("{id:int}/sold-out")]
        public IActionResult MarkSoldOut(int id)
        {
            return ChangeStatus(id, "Sold Out");
        }

        // -------------------------------------
        // 8. RELIST CROP
        // PUT: api/crops/1/relist
        // -------------------------------------

        [HttpPut("{id:int}/relist")]
        public IActionResult Relist(int id)
        {
            return ChangeStatus(id, "Available");
        }

        // -------------------------------------
        // CHANGE CROP STATUS
        // -------------------------------------

        private IActionResult ChangeStatus(
            int id,
            string status)
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
                var crop = Crops.FirstOrDefault(
                    c => c.Id == id &&
                         c.FarmerId == farmerId.Value);

                if (crop == null)
                {
                    return NotFound(new
                    {
                        message = "Crop listing not found."
                    });
                }

                crop.Status = status;

                return Ok(ToResponse(crop));
            }
        }

        // -------------------------------------
        // 9. DELETE CROP
        // DELETE: api/crops/1
        // -------------------------------------

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCrop(int id)
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
                var crop = Crops.FirstOrDefault(
                    c => c.Id == id &&
                         c.FarmerId == farmerId.Value);

                if (crop == null)
                {
                    return NotFound(new
                    {
                        message = "Crop listing not found."
                    });
                }

                // TODO:
                // Check linked Buyer Orders before
                // deleting the crop from database.

                Crops.Remove(crop);

                return NoContent();
            }
        }
    }
}