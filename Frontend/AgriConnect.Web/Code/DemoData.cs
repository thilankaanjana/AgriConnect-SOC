using System;
using System.Collections.Generic;

// ---------- Simple model classes (later: replace with your backend DTOs) ----------
public static class State
{
    public const string Pending = "Pending Verification";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Confirmed = "Order Confirmed";   // request: a supplier was chosen
    public const string Selected = "Selected";            // quotation: chosen by the farmer
    public const string NotSelected = "Not Selected";     // quotation: another supplier was chosen
    public const string Closed = "Closed";                // quotation: request was rejected
    public const string BuyPending = "Pending";           // buyer order: waiting for the farmer
    public const string BuyAccepted = "Accepted";
    public const string BuyDeclined = "Declined";
    public const string BuyCompleted = "Completed";
}

public class AppUser { public int Id { get; set; } public string Name { get; set; } public string Email { get; set; } public string Contact { get; set; } public string Address { get; set; } public string District { get; set; } public string Password { get; set; } public string Role { get; set; } public string Status { get; set; } }
public class SupplyRequest { public int Id { get; set; } public int FarmerId { get; set; } public string Farmer { get; set; } public string Item { get; set; } public string Qty { get; set; } public string Location { get; set; } public string District { get; set; } public decimal Amount { get; set; } public string Unit { get; set; } public string Date { get; set; } public DateTime Created { get; set; } public string Status { get; set; } public int AssignedOfficerId { get; set; } public string AssignedOfficer { get; set; } public string ReviewNote { get; set; } public string RefNo { get { return "REQ" + Id; } } }
public class Quotation { public int Id { get; set; } public int RequestId { get; set; } public int SupplierId { get; set; } public string Supplier { get; set; } public decimal Price { get; set; } public int Stock { get; set; } public string Delivery { get; set; } public DateTime Created { get; set; } public string Status { get; set; } public string ReviewNote { get; set; } public string RefNo { get { return "QUO" + Id; } } public string RequestRef { get { return "REQ" + RequestId; } } }
public class Order { public int Id { get; set; } public int QuoteId { get; set; } public int RequestId { get; set; } public int FarmerId { get; set; } public string FarmerName { get; set; } public int SupplierId { get; set; } public string Supplier { get; set; } public string Item { get; set; } public string Qty { get; set; } public string Location { get; set; } public decimal Price { get; set; } public DateTime Created { get; set; } public string DeliveryStatus { get; set; } public string RefNo { get { return "ORD" + Id; } } public string RequestRef { get { return "REQ" + RequestId; } } }
public class InventoryItem { public int Id { get; set; } public int SupplierId { get; set; } public string Item { get; set; } public int Qty { get; set; } public string Unit { get; set; } public decimal Price { get; set; } public string Supplier { get; set; } public string District { get; set; } public string Contact { get; set; } public string QtyText { get { return Qty + " " + Unit; } } public string Level { get { return Qty <= 0 ? "Out of stock" : (Qty <= 20 ? "Low stock" : "In stock"); } } }
public class Recommendation { public int Id { get; set; } public int RequestId { get; set; } public int OfficerId { get; set; } public string Officer { get; set; } public string Text { get; set; } public DateTime Created { get; set; } public string RequestRef { get { return "REQ" + RequestId; } } }
public class Guideline { public int Id { get; set; } public string Title { get; set; } public string Author { get; set; } public string Date { get; set; } public string Text { get; set; } }
public class Review { public int Id { get; set; } public string Type { get; set; } public int TargetId { get; set; } public string Ref { get; set; } public int OfficerId { get; set; } public string Officer { get; set; } public string Decision { get; set; } public string Note { get; set; } public int Passed { get; set; } public int Total { get; set; } public DateTime Created { get; set; } public string Score { get { return Passed + " / " + Total; } } }
/// <summary>Every change of a supplier's stock (orders, manual updates) - so the supplier can see why the quantity changed.</summary>
public class StockMovement { public int Id { get; set; } public int SupplierId { get; set; } public string Item { get; set; } public string Unit { get; set; } public int Change { get; set; } public int Balance { get; set; } public string Note { get; set; } public DateTime Created { get; set; } }
/// <summary>A buyer's order for a farmer's crop (placed from the public marketplace, no login needed).</summary>
public class CropOrder { public int Id { get; set; } public int CropId { get; set; } public string Crop { get; set; } public int FarmerId { get; set; } public string Farmer { get; set; } public string BuyerName { get; set; } public string BuyerPhone { get; set; } public string BuyerAddress { get; set; } public string Quantity { get; set; } public decimal Total { get; set; } public string Note { get; set; } public DateTime Created { get; set; } public string Status { get; set; } public string RefNo { get { return "BUY" + Id; } } }
public class Notification { public int Id { get; set; } public int UserId { get; set; } public string Text { get; set; } public string Link { get; set; } public DateTime Created { get; set; } public bool Read { get; set; } }
public class ActivityEntry { public int Id { get; set; } public DateTime Created { get; set; } public string User { get; set; } public string Role { get; set; } public string Action { get; set; } }

public class Crop
{
    public Crop() { Status = "Available"; }
    public int Id { get; set; }
    public int FarmerId { get; set; }
    public string Farmer { get; set; }
    public string Name { get; set; }
    public string Qty { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; }
    public string Contact { get; set; }
    public string Tag { get; set; }      // "Organic", "Pesticide-free" or empty
    public string Status { get; set; }   // "Available" or "Sold Out"

    public string Category
    {
        get
        {
            string n = (Name ?? "").ToLower();
            if (n.Contains("rice") || n.Contains("paddy")) return "Grains";
            if (n.Contains("strawberr") || n.Contains("apple") || n.Contains("blueberr") || n.Contains("peach")) return "Fruits";
            return "Vegetables";
        }
    }

    // The farmer's uploaded photo if there is one, otherwise a default picture from Content/img by crop name.
    public string Photo { get; set; }    // uploaded photo path, e.g. ~/Uploads/crops/abc.jpg (empty = default picture)

    public string ImageUrl
    {
        get
        {
            if (!string.IsNullOrEmpty(Photo)) return Photo;
            string n = (Name ?? "").ToLower();
            if (n.Contains("beans")) return "~/Content/img/beans.jpg";
            if (n.Contains("rice") || n.Contains("paddy")) return "~/Content/img/paddy.jpg";
            if (n.Contains("brinjal")) return "~/Content/img/brinjal.jpg";
            if (n.Contains("carrot")) return "~/Content/img/carrot.jpg";
            if (n.Contains("corn")) return "~/Content/img/corn.jpg";
            if (n.Contains("fruits")) return "~/Content/img/fruits.jpg";
            if (n.Contains("grains")) return "~/Content/img/grains.jpg";
            if (n.Contains("mango")) return "~/Content/img/mango.jpg";
            return "~/Content/img/veggies.jpg";
        }
    }
}

public class DemoStore
{
    public int Counter = 100;
    public List<AppUser> Users = new List<AppUser>();
    public List<SupplyRequest> Requests = new List<SupplyRequest>();
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public List<Quotation> Quotes = new List<Quotation>();
    public List<Order> Orders = new List<Order>();
    public List<Crop> Crops = new List<Crop>();
    public List<Recommendation> Recommendations = new List<Recommendation>();
    public List<Guideline> Guidelines = new List<Guideline>();
    public List<Review> Reviews = new List<Review>();
    public List<Notification> Notifications = new List<Notification>();
    public List<ActivityEntry> Activity = new List<ActivityEntry>();
    public List<StockMovement> StockMovements = new List<StockMovement>();
    public List<CropOrder> CropOrders = new List<CropOrder>();
}

/// <summary>
/// TEMPORARY sample data (kept in memory while the site is running) so the UI works without a backend.
/// When the backend is ready, replace the members of this class with calls to your API / service layer.
/// </summary>
public static class DemoData
{
    static DemoStore store = Seed();

    public static string[] Districts { get { return Lookups.Districts; } }

    static DemoStore Seed()
    {
        DemoStore s = new DemoStore();
        const string pw = "agri1234";

        // ----- login accounts only (password for all: agri1234) -----
        s.Users.Add(new AppUser { Id = 1, Name = "Sunil Perera", Email = "farmer@example.com", Contact = "0771234567", Address = "Nuwara Eliya", District = "Nuwara Eliya", Password = pw, Role = "Farmer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 2, Name = "GreenField Supplies", Email = "supplier@example.com", Contact = "0712345678", Address = "Nuwara Eliya Town", District = "Nuwara Eliya", Password = pw, Role = "Supplier", Status = "Active" });
        s.Users.Add(new AppUser { Id = 3, Name = "Dr. A. Wickrama", Email = "officer@example.com", Contact = "0701112223", Address = "Agriculture Office, Nuwara Eliya", District = "Nuwara Eliya", Password = pw, Role = "Officer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 4, Name = "System Admin", Email = "admin@example.com", Contact = "0719998887", Address = "Colombo", District = "Colombo", Password = pw, Role = "Admin", Status = "Active" });
        s.Users.Add(new AppUser { Id = 5, Name = "Kamala Silva", Email = "kamala@example.com", Contact = "0762223334", Address = "Polonnaruwa", District = "Polonnaruwa", Password = pw, Role = "Farmer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 6, Name = "Ruwan Jay", Email = "ruwan@example.com", Contact = "0753334445", Address = "Bandarawela", District = "Badulla", Password = pw, Role = "Farmer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 7, Name = "Nimal Fernando", Email = "nimal@example.com", Contact = "0724445556", Address = "Anuradhapura", District = "Anuradhapura", Password = pw, Role = "Farmer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 8, Name = "AgroMart Lanka", Email = "agromart@example.com", Contact = "0725556667", Address = "Polonnaruwa Town", District = "Polonnaruwa", Password = pw, Role = "Supplier", Status = "Active" });
        s.Users.Add(new AppUser { Id = 9, Name = "Ms. N. Jayasinghe", Email = "officer2@example.com", Contact = "0703334445", Address = "Agriculture Office, Polonnaruwa", District = "Polonnaruwa", Password = pw, Role = "Officer", Status = "Active" });
        s.Users.Add(new AppUser { Id = 10, Name = "Highland Agro Traders", Email = "highland@example.com", Contact = "0716667778", Address = "Nuwara Eliya", District = "Nuwara Eliya", Password = pw, Role = "Supplier", Status = "Active" });

        // No sample crops, requests, quotations, orders, stock or notifications: only the login accounts above.





        return s;
    }

    public static List<AppUser> Users { get { return store.Users; } }
    public static List<SupplyRequest> Requests { get { return store.Requests; } }
    public static List<InventoryItem> Inventory { get { return store.Inventory; } }
    public static List<Quotation> Quotes { get { return store.Quotes; } }
    public static List<Order> Orders { get { return store.Orders; } }
    public static List<Crop> Crops { get { return store.Crops; } }
    public static List<Recommendation> Recommendations { get { return store.Recommendations; } }
    public static List<Guideline> Guidelines { get { return store.Guidelines; } }
    public static List<Review> Reviews { get { return store.Reviews; } }
    public static List<Notification> Notifications { get { return store.Notifications; } }
    public static List<ActivityEntry> Activity { get { return store.Activity; } }
    public static List<StockMovement> StockMovements { get { return store.StockMovements; } }
    public static List<CropOrder> CropOrders { get { return store.CropOrders; } }

    public static int NextId() { return ++store.Counter; }
    public static void Reset() { store = Seed(); }

    public static AppUser FindUserByEmail(string email)
    {

        return Users.Find(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>"+94 77 123 4567", "077-123-4567", "771234567" -> "0771234567"</summary>
    public static string NormalizePhone(string phone)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (char c in phone ?? "") if (c >= '0' && c <= '9') sb.Append(c);
        string d = sb.ToString();
        if (d.StartsWith("94") && d.Length == 11) d = "0" + d.Substring(2);
        else if (d.Length == 9) d = "0" + d;
        return d;
    }

    public static AppUser FindUserByPhone(string phone)
    {
        string key = NormalizePhone(phone);
        return Users.Find(u => NormalizePhone(u.Contact) == key);
    }

    public static AppUser FindUser(int id) { return Users.Find(u => u.Id == id); }

    // ---------- notifications and activity log ----------
    public static void Notify(int userId, string text, string link)
    {
        Notifications.Add(new Notification { Id = NextId(), UserId = userId, Text = text, Link = link, Created = DateTime.Now, Read = false });
    }

    public static int UnreadCount(int userId)
    {
        return Notifications.FindAll(n => n.UserId == userId && !n.Read).Count;
    }

    public static void Log(AppUser user, string action)
    {
        Activity.Add(new ActivityEntry { Id = NextId(), Created = DateTime.Now, User = user == null ? "System" : user.Name, Role = user == null ? "System" : Auth.RoleLabel(user.Role), Action = action });
    }

    // ---------- regional routing ----------
    /// <summary>The active Agriculture Officer of the district with the smallest queue (any officer if the district has none).</summary>
    public static AppUser AssignOfficer(string district)
    {
        List<AppUser> officers = Users.FindAll(u => u.Role == "Officer" && u.Status == "Active");
        if (officers.Count == 0) return null;
        List<AppUser> local = officers.FindAll(u => u.District == district);
        List<AppUser> pool = local.Count > 0 ? local : officers;
        AppUser best = null;
        int bestLoad = int.MaxValue;
        foreach (AppUser o in pool)
        {
            int officerId = o.Id;
            int load = Requests.FindAll(r => r.AssignedOfficerId == officerId && r.Status == State.Pending).Count;
            if (load < bestLoad) { best = o; bestLoad = load; }
        }
        return best;
    }

    /// <summary>Active suppliers of the district (all active suppliers if the district has none).</summary>
    public static List<AppUser> SuppliersFor(string district)
    {
        List<AppUser> all = Users.FindAll(u => u.Role == "Supplier" && u.Status == "Active");
        List<AppUser> local = all.FindAll(u => u.District == district);
        return local.Count > 0 ? local : all;
    }
}