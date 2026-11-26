
using System;

namespace FarmerService.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int QuoteId { get; set; }

        public int RequestId { get; set; }

        public int FarmerId { get; set; }

        public string FarmerName { get; set; } = "";

        public int SupplierId { get; set; }

        public string Supplier { get; set; } = "";

        public string Item { get; set; } = "";

        public string Qty { get; set; } = "";

        public string Location { get; set; } = "";

        public decimal Price { get; set; }

        public DateTime Created { get; set; }

        public string DeliveryStatus { get; set; } = "";

        // Example: ORD101
        public string RefNo { get; set; } = "";

        // Example: REQ101
        public string RequestRef { get; set; } = "";

        // Example: Fertilizer (100 bags)
        public string ItemText
        {
            get
            {
                return Item + " (" + Qty + ")";
            }
        }

        // Processing = 0
        // Dispatched = 1
        // Delivered = 2
        public int TrackingStep
        {
            get
            {
                return DeliveryStatus switch
                {
                    "Processing" => 0,
                    "Dispatched" => 1,
                    "Delivered" => 2,
                    _ => -1
                };
            }
        }
    }
}