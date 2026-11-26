using System;
using System.Collections.Generic;

/// <summary>
/// The AgriConnect business workflow (UI prototype version, runs on DemoData).
///   1. Farmer submits a supply request      -> an officer is assigned, regional suppliers are notified
///   2. Suppliers send quotations            -> forwarded to the assigned officer
///   3. Officer verifies requests/quotations -> approve or reject (with compliance checks)
///   4. Farmer compares approved quotations  -> selects a supplier and confirms the order
///   5. Supplier updates the delivery status -> farmer is notified
/// When the backend is ready, each method here becomes a call to the matching service/API
/// (TODO (backend) marks the spots) and the pages do not need to change.
/// </summary>
public static class Workflow
{
    public static readonly string[] DeliverySteps = { "Processing", "Dispatched", "Delivered" };
    public static readonly string[] RequestChecks = {
        "The requested item is a permitted agricultural input",
        "The quantity is suitable for the farm's needs",
        "The delivery location is within the service region" };
    public static readonly string[] QuoteChecks = {
        "The quoted price is within the normal market range",
        "The supplier's stock covers the requested quantity",
        "The delivery estimate is acceptable" };

    // ---------- helpers ----------
    /// <summary>"50 kg" -> 50   (0 when there is no number)</summary>
    public static decimal ParseAmount(string qty)
    {
        if (string.IsNullOrEmpty(qty)) return 0;
        System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(qty, @"\d+(\.\d+)?");
        decimal v;
        return m.Success && decimal.TryParse(m.Value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out v) ? v : 0;
    }

    static bool SameItem(string a, string b)
    {
        a = (a ?? "").Trim().ToLower(); b = (b ?? "").Trim().ToLower();
        return a != "" && b != "" && (a == b || a.Contains(b) || b.Contains(a));
    }

    /// <summary>Changes a stock quantity and records why (shown to the supplier under Inventory > Stock history).</summary>
    public static void RecordStock(InventoryItem it, int newQty, string note)
    {
        int change = newQty - it.Qty;
        it.Qty = newQty;
        if (change == 0) return;
        DemoData.StockMovements.Add(new StockMovement { Id = DemoData.NextId(), SupplierId = it.SupplierId, Item = it.Item, Unit = it.Unit, Change = change, Balance = newQty, Note = note, Created = DateTime.Now });
    }

    // ---------- 1. supply request ----------
    public static SupplyRequest SubmitRequest(AppUser farmer, string item, decimal amount, string unit, string location, string district, string preferredDate)
    {
        string qty = amount.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + " " + unit;
        AppUser officer = DemoData.AssignOfficer(district);
        SupplyRequest r = new SupplyRequest
        {
            Id = DemoData.NextId(),
            FarmerId = farmer.Id,
            Farmer = farmer.Name,
            Item = item,
            Qty = qty,
            Amount = amount,
            Unit = unit,
            Location = location,
            District = district,
            Date = preferredDate,
            Created = DateTime.Now,
            Status = State.Pending,
            AssignedOfficerId = officer == null ? 0 : officer.Id,
            AssignedOfficer = officer == null ? "Unassigned" : officer.Name
        };
        DemoData.Requests.Add(r);

        foreach (AppUser s in DemoData.SuppliersFor(district))
            DemoData.Notify(s.Id, "New supply request " + r.RefNo + ": " + item + " (" + qty + ") in " + district + ". Please send a quotation.", "~/Supplier/CreateQuotation.aspx?req=" + r.Id);
        if (officer != null)
            DemoData.Notify(officer.Id, r.RefNo + " from " + farmer.Name + " is waiting for your verification.", "~/Officer/Verifications.aspx");
        DemoData.Notify(farmer.Id, "Your request " + r.RefNo + " was submitted. Suppliers in " + district + " were notified.", "~/Farmer/SupplyRequest.aspx");
        DemoData.Log(farmer, "Submitted supply request " + r.RefNo + " (" + item + ")");
        return r;
    }

    // ---------- 2. supplier quotation ----------
    public static bool SubmitQuotation(AppUser supplier, int requestId, decimal price, int stock, string delivery, out string error)
    {
        error = null;
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == requestId);
        if (r == null) { error = "Supply request not found."; return false; }
        if (r.Status != State.Pending && r.Status != State.Approved) { error = r.RefNo + " is no longer open for quotations."; return false; }
        decimal needed = r.Amount > 0 ? r.Amount : ParseAmount(r.Qty);
        if (needed > 0 && stock < needed) { error = "Your stock (" + stock + ") is less than the quantity the farmer needs (" + r.Qty + ")."; return false; }
        if (DemoData.Quotes.Exists(q => q.RequestId == requestId && q.SupplierId == supplier.Id)) { error = "You have already sent a quotation for " + r.RefNo + "."; return false; }

        Quotation quote = new Quotation
        {
            Id = DemoData.NextId(),
            RequestId = requestId,
            SupplierId = supplier.Id,
            Supplier = supplier.Name,
            Price = price,
            Stock = stock,
            Delivery = delivery,
            Created = DateTime.Now,
            Status = State.Pending
        };
        DemoData.Quotes.Add(quote);
        if (r.AssignedOfficerId != 0)
            DemoData.Notify(r.AssignedOfficerId, "Quotation " + quote.RefNo + " for " + r.RefNo + " was forwarded for verification.", "~/Officer/Verifications.aspx");
        DemoData.Log(supplier, "Submitted quotation " + quote.RefNo + " for " + r.RefNo);
        return true;
    }

    /// <summary>Requests a supplier can see: requests of its region that were not rejected.</summary>
    public static List<SupplyRequest> RequestsForSupplier(AppUser supplier)
    {
        return DemoData.Requests.FindAll(r => r.Status != State.Rejected &&
            DemoData.SuppliersFor(r.District).Exists(s => s.Id == supplier.Id));
    }

    public static Quotation QuoteBy(AppUser supplier, int requestId)
    {
        return AppData.Quotes.Find(q => q.RequestId == requestId && q.SupplierId == supplier.Id);
    }

    public static bool IsOpenForQuotes(SupplyRequest r)
    {
        return r.Status == State.Pending || r.Status == State.Approved;
    }

    // ---------- 3. officer verification ----------
    public static bool ReviewRequest(AppUser officer, int requestId, bool approve, string note, int passed, out string error)
    {
        error = null;
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == requestId);
        if (r == null || r.Status != State.Pending) { error = "This request is no longer waiting for verification."; return false; }

        r.Status = approve ? State.Approved : State.Rejected;
        r.ReviewNote = note;
        AddReview("Request", r.Id, r.RefNo, officer, r.Status, note, passed, RequestChecks.Length);

        DemoData.Notify(r.FarmerId, "Your request " + r.RefNo + " was " + r.Status.ToLower() + " by " + officer.Name + "." + (string.IsNullOrEmpty(note) ? "" : " Note: " + note), "~/Farmer/SupplyRequest.aspx");
        if (!approve)
        {
            foreach (Quotation q in DemoData.Quotes.FindAll(x => x.RequestId == r.Id && (x.Status == State.Pending || x.Status == State.Approved)))
            {
                q.Status = State.Closed;
                q.ReviewNote = "The supply request was rejected.";
                DemoData.Notify(q.SupplierId, "Quotation " + q.RefNo + " was closed because " + r.RefNo + " was rejected.", "~/Supplier/MyQuotations.aspx");
            }
        }
        DemoData.Log(officer, (approve ? "Approved " : "Rejected ") + "supply request " + r.RefNo);
        return true;
    }

    public static bool ReviewQuotation(AppUser officer, int quoteId, bool approve, string note, int passed, out string error)
    {
        error = null;
        Quotation q = DemoData.Quotes.Find(x => x.Id == quoteId);
        if (q == null || q.Status != State.Pending) { error = "This quotation is no longer waiting for verification."; return false; }

        q.Status = approve ? State.Approved : State.Rejected;
        q.ReviewNote = note;
        AddReview("Quotation", q.Id, q.RefNo, officer, q.Status, note, passed, QuoteChecks.Length);

        DemoData.Notify(q.SupplierId, "Your quotation " + q.RefNo + " for " + q.RequestRef + " was " + q.Status.ToLower() + "." + (string.IsNullOrEmpty(note) ? "" : " Note: " + note), "~/Supplier/MyQuotations.aspx");
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == q.RequestId);
        if (approve && r != null)
            DemoData.Notify(r.FarmerId, "A verified quotation from " + q.Supplier + " is available for " + r.RefNo + ". Compare and confirm your order.", "~/Farmer/MyQuotations.aspx");
        DemoData.Log(officer, (approve ? "Approved " : "Rejected ") + "quotation " + q.RefNo);
        return true;
    }

    static void AddReview(string type, int targetId, string reference, AppUser officer, string decision, string note, int passed, int total)
    {
        DemoData.Reviews.Add(new Review
        {
            Id = DemoData.NextId(),
            Type = type,
            TargetId = targetId,
            Ref = reference,
            OfficerId = officer.Id,
            Officer = officer.Name,
            Decision = decision,
            Note = note ?? "",
            Passed = passed,
            Total = total,
            Created = DateTime.Now
        });
    }

    public static void AddRecommendation(AppUser officer, int requestId, string text)
    {
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == requestId);
        if (r == null) return;
        DemoData.Recommendations.Add(new Recommendation { Id = DemoData.NextId(), RequestId = requestId, OfficerId = officer.Id, Officer = officer.Name, Text = text, Created = DateTime.Now });
        DemoData.Notify(r.FarmerId, "New recommendation from " + officer.Name + " for " + r.RefNo + ".", "~/Farmer/SupplyRequest.aspx");
        DemoData.Log(officer, "Added a recommendation for " + r.RefNo);
    }

    // ---------- 4. farmer confirmation ----------
    public static bool ConfirmOrder(AppUser farmer, int quoteId, out string error)
    {
        error = null;
        Quotation chosen = DemoData.Quotes.Find(x => x.Id == quoteId);
        if (chosen == null) { error = "Quotation not found."; return false; }
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == chosen.RequestId);
        if (r == null || r.FarmerId != farmer.Id) { error = "This quotation does not belong to your request."; return false; }
        if (r.Status != State.Approved) { error = r.Status == State.Confirmed ? "An order is already confirmed for " + r.RefNo + "." : r.RefNo + " must be approved by the officer first."; return false; }
        if (chosen.Status != State.Approved) { error = "Only officer-approved quotations can be confirmed."; return false; }

        Order o = new Order
        {
            Id = DemoData.NextId(),
            QuoteId = chosen.Id,
            RequestId = r.Id,
            FarmerId = farmer.Id,
            FarmerName = farmer.Name,
            SupplierId = chosen.SupplierId,
            Supplier = chosen.Supplier,
            Item = r.Item,
            Qty = r.Qty,
            Location = r.Location,
            Price = chosen.Price,
            Created = DateTime.Now,
            DeliveryStatus = DeliverySteps[0]
        };
        DemoData.Orders.Add(o);
        r.Status = State.Confirmed;
        chosen.Status = State.Selected;
        foreach (Quotation other in DemoData.Quotes.FindAll(x => x.RequestId == r.Id && x.Id != chosen.Id && (x.Status == State.Pending || x.Status == State.Approved)))
        {
            other.Status = State.NotSelected;
            DemoData.Notify(other.SupplierId, "The farmer selected another supplier for " + r.RefNo + ".", "~/Supplier/MyQuotations.aspx");
        }
        // stock of the supplier goes down by the ordered quantity (and the change is recorded)
        decimal ordered = r.Amount > 0 ? r.Amount : ParseAmount(r.Qty);
        InventoryItem stockItem = DemoData.Inventory.Find(i => i.SupplierId == chosen.SupplierId && SameItem(i.Item, r.Item));
        if (stockItem != null && ordered > 0)
        {
            int before = stockItem.Qty;
            RecordStock(stockItem, Math.Max(0, before - (int)Math.Ceiling(ordered)), "Order " + o.RefNo + " (" + farmer.Name + ")");
            DemoData.Notify(chosen.SupplierId, "Stock of " + stockItem.Item + " went down from " + before + " to " + stockItem.Qty + " " + stockItem.Unit + " because of order " + o.RefNo + "." + (stockItem.Qty == 0 ? " The item is now out of stock." : ""), "~/Supplier/Inventory.aspx");
        }
        DemoData.Notify(chosen.SupplierId, "Order " + o.RefNo + " confirmed by " + farmer.Name + " for " + r.Item + " (" + r.Qty + "). Please deliver to " + r.Location + ".", "~/Supplier/Deliveries.aspx");
        DemoData.Notify(farmer.Id, "Your order " + o.RefNo + " with " + chosen.Supplier + " is confirmed.", "~/Farmer/MyOrders.aspx");
        DemoData.Log(farmer, "Confirmed order " + o.RefNo + " (" + chosen.Supplier + ")");
        return true;
    }

    // ---------- 5. delivery ----------
    public static bool UpdateDelivery(AppUser supplier, int orderId, string status, out string error)
    {
        error = null;
        Order o = DemoData.Orders.Find(x => x.Id == orderId);
        if (o == null || o.SupplierId != supplier.Id) { error = "Order not found."; return false; }
        int from = Array.IndexOf(DeliverySteps, o.DeliveryStatus);
        int to = Array.IndexOf(DeliverySteps, status);
        if (to < 0) { error = "Unknown delivery status."; return false; }
        if (to < from) { error = "Delivery status cannot go backwards."; return false; }
        if (to == from) return true;
        o.DeliveryStatus = status;
        DemoData.Notify(o.FarmerId, "Your order " + o.RefNo + " is now " + status + ".", "~/Farmer/MyOrders.aspx");
        DemoData.Log(supplier, "Updated delivery of " + o.RefNo + " to " + status);
        return true;
    }

    // ---------- helpers used by dashboards ----------
    /// <summary>Progress of a supply request through the five workflow steps.</summary>
    public static bool[] Progress(SupplyRequest r)
    {
        Order o = AppData.Orders.Find(x => x.RequestId == r.Id);
        bool hasQuotes = AppData.Quotes.Exists(q => q.RequestId == r.Id);
        return new bool[]
        {
            true,
            hasQuotes,
            r.Status == State.Approved || r.Status == State.Confirmed,
            o != null,
            o != null && o.DeliveryStatus == "Delivered"
        };
    }

    public static readonly string[] ProgressLabels = { "Submitted", "Quotations", "Verified", "Confirmed", "Delivered" };

    // ---------- edit / delete (own records only) ----------
    /// <summary>A request can be changed or deleted by its farmer until a supplier has quoted for it and while it still waits for verification.</summary>
    public static bool CanChangeRequest(SupplyRequest r)
    {
        return r.Status == State.Pending && !DemoData.Quotes.Exists(q => q.RequestId == r.Id);
    }

    public static bool UpdateRequest(AppUser farmer, int requestId, string item, decimal amount, string unit, string location, string district, string preferredDate, out string error)
    {
        error = null;
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == requestId && x.FarmerId == farmer.Id);
        if (r == null) { error = "Supply request not found."; return false; }
        if (!CanChangeRequest(r)) { error = r.RefNo + " can no longer be edited (it was already verified or suppliers have sent quotations)."; return false; }
        bool districtChanged = r.District != district;
        r.Item = item;
        r.Amount = amount;
        r.Unit = unit;
        r.Qty = amount.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) + " " + unit;
        r.Location = location;
        r.District = district;
        r.Date = preferredDate;
        if (districtChanged)
        {
            AppUser officer = DemoData.AssignOfficer(district);
            r.AssignedOfficerId = officer == null ? 0 : officer.Id;
            r.AssignedOfficer = officer == null ? "Unassigned" : officer.Name;
            if (officer != null) DemoData.Notify(officer.Id, r.RefNo + " from " + farmer.Name + " is waiting for your verification.", "~/Officer/Verifications.aspx");
            foreach (AppUser s in DemoData.SuppliersFor(district))
                DemoData.Notify(s.Id, "Supply request " + r.RefNo + ": " + item + " (" + r.Qty + ") in " + district + ". Please send a quotation.", "~/Supplier/CreateQuotation.aspx?req=" + r.Id);
        }
        else if (r.AssignedOfficerId != 0)
            DemoData.Notify(r.AssignedOfficerId, r.RefNo + " was updated by " + farmer.Name + ".", "~/Officer/Verifications.aspx");
        DemoData.Log(farmer, "Edited supply request " + r.RefNo);
        return true;
    }

    public static bool DeleteRequest(AppUser farmer, int requestId, out string error)
    {
        error = null;
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == requestId && x.FarmerId == farmer.Id);
        if (r == null) { error = "Supply request not found."; return false; }
        if (!CanChangeRequest(r)) { error = r.RefNo + " can no longer be deleted (it was already verified or suppliers have sent quotations)."; return false; }
        DemoData.Requests.Remove(r);
        DemoData.Recommendations.RemoveAll(x => x.RequestId == r.Id);
        DemoData.Log(farmer, "Deleted supply request " + r.RefNo);
        return true;
    }

    public static bool UpdateQuotation(AppUser supplier, int quoteId, decimal price, int stock, string delivery, out string error)
    {
        error = null;
        Quotation q = DemoData.Quotes.Find(x => x.Id == quoteId && x.SupplierId == supplier.Id);
        if (q == null) { error = "Quotation not found."; return false; }
        if (q.Status != State.Pending) { error = q.RefNo + " can no longer be edited (the officer already verified it)."; return false; }
        SupplyRequest r = DemoData.Requests.Find(x => x.Id == q.RequestId);
        if (r != null)
        {
            decimal needed = r.Amount > 0 ? r.Amount : ParseAmount(r.Qty);
            if (needed > 0 && stock < needed) { error = "Your stock (" + stock + ") is less than the quantity the farmer needs (" + r.Qty + ")."; return false; }
        }
        q.Price = price;
        q.Stock = stock;
        q.Delivery = delivery;
        if (r != null && r.AssignedOfficerId != 0)
            DemoData.Notify(r.AssignedOfficerId, "Quotation " + q.RefNo + " for " + r.RefNo + " was updated and is waiting for verification.", "~/Officer/Verifications.aspx");
        DemoData.Log(supplier, "Edited quotation " + q.RefNo);
        return true;
    }

    // ---------- 6. buyers (public marketplace) ----------
    public static bool PlaceCropOrder(CropOrder o, out string error)
    {
        error = null;
        Crop c = DemoData.Crops.Find(x => x.Id == o.CropId);
        if (c == null || c.Status != "Available") { error = "This crop is no longer available."; return false; }
        decimal wanted = ParseAmount(o.Quantity), have = ParseAmount(c.Qty);
        if (wanted <= 0) { error = "Enter the quantity you want to buy."; return false; }
        if (have > 0 && wanted > have) { error = "Only " + c.Qty + " is available."; return false; }
        o.Id = DemoData.NextId(); o.Crop = c.Name; o.FarmerId = c.FarmerId; o.Farmer = c.Farmer;
        o.Total = wanted * c.Price; o.Created = DateTime.Now; o.Status = State.BuyPending;
        DemoData.CropOrders.Add(o);
        DemoData.Notify(c.FarmerId, "New buyer order " + o.RefNo + " from " + o.BuyerName + ": " + o.Quantity + " of " + c.Name + ".", "~/Farmer/CropOrders.aspx");
        DemoData.Log(new AppUser { Name = o.BuyerName, Role = "Buyer" }, "Placed order " + o.RefNo + " for " + c.Name + " (" + c.Farmer + ")");
        return true;
    }

    public static bool UpdateCropOrder(AppUser farmer, int id, string status, out string error)
    {
        error = null;
        CropOrder o = DemoData.CropOrders.Find(x => x.Id == id && x.FarmerId == farmer.Id);
        if (o == null) { error = "Order not found."; return false; }
        bool ok = (o.Status == State.BuyPending && (status == State.BuyAccepted || status == State.BuyDeclined))
               || (o.Status == State.BuyAccepted && (status == State.BuyCompleted || status == State.BuyDeclined));
        if (!ok) { error = "This order is already " + o.Status.ToLower() + "."; return false; }
        o.Status = status;
        DemoData.Log(farmer, status + " buyer order " + o.RefNo);
        return true;
    }
}