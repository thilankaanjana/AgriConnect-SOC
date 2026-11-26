using System;
using System.Collections.Generic;

/// <summary>Entry point used by every page:  AppData.Requests, AppData.Current.SubmitRequest(...), ...</summary>
public static class AppData
{
    static IAppData current;
    static readonly object sync = new object();

    public static IAppData Current
    {
        get
        {
            if (current == null)
                lock (sync)
                    if (current == null)
                    {
                        current = new LocalData();
                    }
            return current;
        }
    }

    public static List<AppUser> Users { get { return Current.GetUsers(); } }
    public static List<SupplyRequest> Requests { get { return Current.GetRequests(); } }
    public static List<Quotation> Quotes { get { return Current.GetQuotes(); } }
    public static List<Order> Orders { get { return Current.GetOrders(); } }
    public static List<Crop> Crops { get { return Current.GetCrops(); } }
    public static List<InventoryItem> Inventory { get { return Current.GetInventory(); } }
    public static List<Recommendation> Recommendations { get { return Current.GetRecommendations(); } }
    public static List<Guideline> Guidelines { get { return Current.GetGuidelines(); } }
    public static List<Review> Reviews { get { return Current.GetReviews(); } }
    public static List<ActivityEntry> Activity { get { return Current.GetActivity(); } }
    public static List<Notification> Notifications(int userId) { return Current.GetNotifications(userId); }
    public static List<SupplyRequest> RequestsForSupplier(AppUser supplier) { return Current.GetRequestsForSupplier(supplier); }
}

/// <summary>Fixed lists the UI itself needs (not demo data).</summary>
public static class Lookups
{
    public static readonly string[] Districts = {
        "Ampara", "Anuradhapura", "Badulla", "Batticaloa", "Colombo", "Galle", "Gampaha", "Hambantota", "Jaffna", "Kalutara",
        "Kandy", "Kegalle", "Kilinochchi", "Kurunegala", "Mannar", "Matale", "Matara", "Monaragala", "Mullaitivu", "Nuwara Eliya",
        "Polonnaruwa", "Puttalam", "Ratnapura", "Trincomalee", "Vavuniya" };
}