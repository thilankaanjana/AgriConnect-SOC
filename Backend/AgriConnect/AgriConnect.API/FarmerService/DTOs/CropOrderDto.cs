
using System;

namespace FarmerService.DTOs
{
    public class CropOrderDto
    {
        public int Id { get; set; }

        public int CropId { get; set; }

        public string Crop { get; set; } = "";

        public int FarmerId { get; set; }

        public string Farmer { get; set; } = "";

        public string BuyerName { get; set; } = "";

        public string BuyerPhone { get; set; } = "";

        public string BuyerAddress { get; set; } = "";

        // Example: 5 kg
        public string Quantity { get; set; } = "";

        public decimal Total { get; set; }

        public string Note { get; set; } = "";

        public DateTime Created { get; set; }

        // Pending / Accepted / Declined / Completed
        public string Status { get; set; } = "";

        // Example: BUY101
        public string RefNo { get; set; } = "";
    }
}