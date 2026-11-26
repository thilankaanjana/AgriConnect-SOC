
using System;

namespace SupplierService.DTOs
{
    public class SupplierSupplyRequestDto
    {
        // Supply Request ID
        public int Id { get; set; }

        // Example: REQ1
        public string RefNo { get; set; } = "";

        // Farmer Name
        public string Farmer { get; set; } = "";

        // Requested Item
        public string Item { get; set; } = "";

        // Example: 100 kg
        public string Qty { get; set; } = "";

        // Numeric Quantity
        public decimal Amount { get; set; }

        // Example: kg, bags, units
        public string Unit { get; set; } = "";

        // Delivery Location
        public string Location { get; set; } = "";

        // Farmer District
        public string District { get; set; } = "";

        // Preferred Delivery Date
        public string Date { get; set; } = "";

        // Request Created Date
        public DateTime Created { get; set; }

        // Pending Verification / Approved / etc.
        public string Status { get; set; } = "";

        // This Supplier's Quotation Status
        // Empty if quotation not submitted
        public string MyQuote { get; set; } = "";

        // Can Supplier Send a Quotation?
        public bool CanQuote { get; set; }
    }
}