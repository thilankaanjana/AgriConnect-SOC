using System.Collections.Generic;

/// <summary>Everything the pages need (accounts, requests, quotations, orders, crops, stock ...). Implemented by LocalData (in-memory demo data).</summary>
public interface IAppData
{
    // ----- accounts / login -----
    /// <summary>Checks email + password. Returns the user (with Role) or null and an error message.</summary>
    AppUser Login(string email, string password, out string error);
    bool EmailExists(string email);
    /// <summary>Registers a Farmer/Supplier (actor = null) or lets the Admin create an Officer/Admin account.</summary>
    bool CreateUser(AppUser actor, AppUser user, out string error);
    void SetUserStatus(AppUser actor, int userId, string status);
    /// <summary>Called once after login + OTP succeeded.</summary>
    void OnSignedIn(AppUser user);
    int CountActiveUsers(string role);

    // ----- reads -----
    List<AppUser> GetUsers();
    List<SupplyRequest> GetRequests();
    List<SupplyRequest> GetRequestsForSupplier(AppUser supplier);
    List<Quotation> GetQuotes();
    List<Order> GetOrders();
    List<Crop> GetCrops();
    List<InventoryItem> GetInventory();
    List<Recommendation> GetRecommendations();
    List<Guideline> GetGuidelines();
    List<Review> GetReviews();
    List<Notification> GetNotifications(int userId);
    int GetUnreadCount(int userId);
    List<ActivityEntry> GetActivity();
    /// <summary>Stock of ALL suppliers (each item with supplier name, district, contact) - farmers browse it.</summary>
    List<InventoryItem> GetSupplierStock();
    List<StockMovement> GetStockMovements(AppUser supplier);
    /// <summary>Farmer: orders placed by buyers for his crops. Admin: all.</summary>
    List<CropOrder> GetCropOrders(AppUser user);

    // ----- farmer -----
    SupplyRequest SubmitRequest(AppUser farmer, string item, decimal amount, string unit, string location, string district, string preferredDate);
    bool UpdateCropOrder(AppUser farmer, int orderId, string status, out string error);
    bool ConfirmOrder(AppUser farmer, int quoteId, out string error);
    void AddCrop(AppUser farmer, Crop crop);
    void SetCropStatus(AppUser farmer, int cropId, string status);
    void DeleteCrop(AppUser farmer, int cropId);

    // ----- buyers (no login) -----
    bool PlaceCropOrder(CropOrder order, out string error);

    // ----- supplier -----
    bool SubmitQuotation(AppUser supplier, int requestId, decimal price, int stock, string delivery, out string error);
    bool UpdateDelivery(AppUser supplier, int orderId, string status, out string error);
    void AddInventory(AppUser supplier, InventoryItem item);
    void UpdateInventory(AppUser supplier, int itemId, int qty, decimal price);
    void DeleteInventory(AppUser supplier, int itemId);

    // ----- officer -----
    bool ReviewRequest(AppUser officer, int requestId, bool approve, string note, int passed, out string error);
    bool ReviewQuotation(AppUser officer, int quoteId, bool approve, string note, int passed, out string error);
    void AddRecommendation(AppUser officer, int requestId, string text);
    void AddGuideline(AppUser officer, string title, string text);

    // ----- profile (every role) -----
    bool UpdateProfile(AppUser user, string name, string contact, string address, string district, out string error);
    bool ChangePassword(AppUser user, string currentPassword, string newPassword, out string error);

    // ----- edit / delete -----
    bool UpdateCrop(AppUser farmer, Crop crop, out string error);
    bool UpdateRequest(AppUser farmer, int requestId, string item, decimal amount, string unit, string location, string district, string preferredDate, out string error);
    bool DeleteRequest(AppUser farmer, int requestId, out string error);
    bool UpdateQuotation(AppUser supplier, int quoteId, decimal price, int stock, string delivery, out string error);
    bool UpdateGuideline(AppUser officer, int guidelineId, string title, string text, out string error);
    bool DeleteGuideline(AppUser officer, int guidelineId, out string error);

    // ----- notifications / demo -----
    void MarkNotificationsRead(int userId);
    void ResetDemoData();
}