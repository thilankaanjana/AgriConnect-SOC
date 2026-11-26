using System;
using System.Collections.Generic;

/// <summary>Data layer used by all pages: runs on the in-memory DemoData / Workflow.</summary>
public class LocalData : IAppData
{
    // ----- accounts -----
    public AppUser Login(string phone, string password, out string error)
    {
        error = null;
        AppUser u = DemoData.FindUserByPhone(phone);
        if (u == null || u.Password != password) { error = "Invalid mobile number or password."; return null; }
        if (u.Status == "Pending") { error = "Your account is waiting for administrator approval. You can log in after it is approved."; return null; }
        if (u.Status == "Rejected") { error = "Your registration was rejected. Please contact the administrator."; return null; }
        if (u.Status != "Active") { error = "This account is suspended. Please contact the administrator."; return null; }
        return u;
    }
    public bool EmailExists(string email) { return DemoData.FindUserByEmail(email) != null; }
    public bool CreateUser(AppUser actor, AppUser user, out string error)
    {
        error = null;
        if (EmailExists(user.Email)) { error = "An account with this email already exists."; return false; }
        if (DemoData.FindUserByPhone(user.Contact) != null) { error = "An account with this mobile number already exists."; return false; }
        if (user.Id == 0) user.Id = DemoData.NextId();
        if (actor == null) user.Status = "Pending";   // self-registration: an administrator must approve it first
        DemoData.Users.Add(user);
        if (actor != null) DemoData.Log(actor, "Created " + Auth.RoleLabel(user.Role) + " account for " + user.Name);
        else
        {
            DemoData.Log(user, "Registered a new " + Auth.RoleLabel(user.Role) + " account (waiting for approval)");
            foreach (AppUser admin in DemoData.Users.FindAll(a => a.Role == "Admin" && a.Status == "Active"))
                DemoData.Notify(admin.Id, "New " + Auth.RoleLabel(user.Role) + " registration: " + user.Name + " is waiting for your approval.", "~/Admin/Users.aspx");
        }
        return true;
    }
    public void SetUserStatus(AppUser actor, int userId, string status)
    {
        AppUser u = DemoData.FindUser(userId);
        if (u == null) return;
        string before = u.Status;
        u.Status = status;
        string verb = status == "Active" ? (before == "Pending" ? "Approved " : "Activated ") : status == "Rejected" ? "Rejected " : "Suspended ";
        DemoData.Log(actor, verb + "account of " + u.Name);
    }
    public void OnSignedIn(AppUser user) { DemoData.Log(user, "Logged in"); }
    public int CountActiveUsers(string role) { return DemoData.Users.FindAll(x => x.Role == role && x.Status == "Active").Count; }

    // ----- reads -----
    public List<AppUser> GetUsers() { return DemoData.Users; }
    public List<SupplyRequest> GetRequests() { return DemoData.Requests; }
    public List<SupplyRequest> GetRequestsForSupplier(AppUser supplier) { return Workflow.RequestsForSupplier(supplier); }
    public List<Quotation> GetQuotes() { return DemoData.Quotes; }
    public List<Order> GetOrders() { return DemoData.Orders; }
    public List<Crop> GetCrops() { return DemoData.Crops; }
    public List<InventoryItem> GetInventory() { return DemoData.Inventory; }
    public List<Recommendation> GetRecommendations() { return DemoData.Recommendations; }
    public List<Guideline> GetGuidelines() { return DemoData.Guidelines; }
    public List<Review> GetReviews() { return DemoData.Reviews; }
    public List<Notification> GetNotifications(int userId) { return DemoData.Notifications.FindAll(n => n.UserId == userId); }
    public int GetUnreadCount(int userId) { return DemoData.UnreadCount(userId); }
    public List<ActivityEntry> GetActivity() { return DemoData.Activity; }
    public List<InventoryItem> GetSupplierStock()
    {
        List<InventoryItem> list = new List<InventoryItem>();
        foreach (InventoryItem i in DemoData.Inventory)
        {
            AppUser s = DemoData.FindUser(i.SupplierId);
            if (s == null || s.Status != "Active") continue;
            list.Add(new InventoryItem { Id = i.Id, SupplierId = i.SupplierId, Item = i.Item, Qty = i.Qty, Unit = i.Unit, Price = i.Price, Supplier = s.Name, District = s.District, Contact = s.Contact });
        }
        return list;
    }
    public List<StockMovement> GetStockMovements(AppUser supplier) { return DemoData.StockMovements.FindAll(m => m.SupplierId == supplier.Id); }
    public List<CropOrder> GetCropOrders(AppUser user)
    {
        return DemoData.CropOrders.FindAll(o => user == null || user.Role == "Admin" || o.FarmerId == user.Id);
    }

    // ----- farmer -----
    public SupplyRequest SubmitRequest(AppUser farmer, string item, decimal amount, string unit, string location, string district, string preferredDate)
    { return Workflow.SubmitRequest(farmer, item, amount, unit, location, district, preferredDate); }
    public bool PlaceCropOrder(CropOrder order, out string error) { return Workflow.PlaceCropOrder(order, out error); }
    public bool UpdateCropOrder(AppUser farmer, int orderId, string status, out string error) { return Workflow.UpdateCropOrder(farmer, orderId, status, out error); }
    public bool ConfirmOrder(AppUser farmer, int quoteId, out string error) { return Workflow.ConfirmOrder(farmer, quoteId, out error); }
    public void AddCrop(AppUser farmer, Crop crop)
    {
        crop.Id = DemoData.NextId();
        DemoData.Crops.Add(crop);
        DemoData.Log(farmer, "Published crop listing: " + crop.Name);
    }
    public void SetCropStatus(AppUser farmer, int cropId, string status)
    {
        Crop c = DemoData.Crops.Find(x => x.Id == cropId && x.FarmerId == farmer.Id);
        if (c == null) return;
        c.Status = status;
        DemoData.Log(farmer, "Marked crop listing " + c.Name + " as " + c.Status);
    }
    public void DeleteCrop(AppUser farmer, int cropId)
    {
        Crop c = DemoData.Crops.Find(x => x.Id == cropId && x.FarmerId == farmer.Id);
        if (c == null) return;
        DemoData.Crops.Remove(c);
        DemoData.Log(farmer, "Deleted crop listing: " + c.Name);
    }

    // ----- supplier -----
    public bool SubmitQuotation(AppUser supplier, int requestId, decimal price, int stock, string delivery, out string error)
    { return Workflow.SubmitQuotation(supplier, requestId, price, stock, delivery, out error); }
    public bool UpdateDelivery(AppUser supplier, int orderId, string status, out string error)
    { return Workflow.UpdateDelivery(supplier, orderId, status, out error); }
    public void AddInventory(AppUser supplier, InventoryItem item)
    {
        item.Id = DemoData.NextId();
        item.SupplierId = supplier.Id;
        int qty = item.Qty; item.Qty = 0;
        DemoData.Inventory.Add(item);
        Workflow.RecordStock(item, qty, "Item added to inventory");
        DemoData.Log(supplier, "Added inventory item " + item.Item);
    }
    public void UpdateInventory(AppUser supplier, int itemId, int qty, decimal price)
    {
        InventoryItem it = DemoData.Inventory.Find(x => x.Id == itemId && x.SupplierId == supplier.Id);
        if (it == null) return;
        Workflow.RecordStock(it, qty, "Stock updated manually");
        it.Price = price;
        DemoData.Log(supplier, "Updated inventory item " + it.Item);
    }
    public void DeleteInventory(AppUser supplier, int itemId)
    {
        InventoryItem it = DemoData.Inventory.Find(x => x.Id == itemId && x.SupplierId == supplier.Id);
        if (it == null) return;
        Workflow.RecordStock(it, 0, "Item removed from inventory");
        DemoData.Inventory.Remove(it);
        DemoData.Log(supplier, "Removed inventory item " + it.Item);
    }

    // ----- officer -----
    public bool ReviewRequest(AppUser officer, int requestId, bool approve, string note, int passed, out string error)
    { return Workflow.ReviewRequest(officer, requestId, approve, note, passed, out error); }
    public bool ReviewQuotation(AppUser officer, int quoteId, bool approve, string note, int passed, out string error)
    { return Workflow.ReviewQuotation(officer, quoteId, approve, note, passed, out error); }
    public void AddRecommendation(AppUser officer, int requestId, string text) { Workflow.AddRecommendation(officer, requestId, text); }
    public void AddGuideline(AppUser officer, string title, string text)
    {
        DemoData.Guidelines.Add(new Guideline { Id = DemoData.NextId(), Title = title, Author = officer.Name, Date = DateTime.Now.ToString("yyyy-MM-dd"), Text = text });
        DemoData.Log(officer, "Published guideline: " + title);
    }

    // ----- profile -----
    public bool UpdateProfile(AppUser user, string name, string contact, string address, string district, out string error)
    {
        error = null;
        AppUser u = DemoData.FindUser(user.Id);
        if (u == null) { error = "Account not found."; return false; }
        AppUser other = DemoData.FindUserByPhone(contact);
        if (other != null && other.Id != u.Id) { error = "Another account already uses this mobile number."; return false; }
        u.Name = name; u.Contact = contact; u.Address = address; u.District = district;
        user.Name = name; user.Contact = contact; user.Address = address; user.District = district;   // the signed-in copy
        DemoData.Log(u, "Updated profile");
        return true;
    }

    public bool ChangePassword(AppUser user, string currentPassword, string newPassword, out string error)
    {
        error = null;
        AppUser u = DemoData.FindUser(user.Id);
        if (u == null) { error = "Account not found."; return false; }
        if (u.Password != currentPassword) { error = "The current password is not correct."; return false; }
        if (string.IsNullOrEmpty(newPassword) || newPassword.Length < 6) { error = "The new password must be at least 6 characters."; return false; }
        u.Password = newPassword;
        DemoData.Log(u, "Changed password");
        return true;
    }

    // ----- edit / delete -----
    public bool UpdateCrop(AppUser farmer, Crop crop, out string error)
    {
        error = null;
        Crop c = DemoData.Crops.Find(x => x.Id == crop.Id && x.FarmerId == farmer.Id);
        if (c == null) { error = "Crop listing not found."; return false; }
        c.Name = crop.Name; c.Qty = crop.Qty; c.Price = crop.Price; c.Location = crop.Location; c.Contact = crop.Contact; c.Tag = crop.Tag;
        if (!string.IsNullOrEmpty(crop.Photo)) c.Photo = crop.Photo;   // keep the old photo when no new one was chosen
        DemoData.Log(farmer, "Edited crop listing: " + c.Name);
        return true;
    }
    public bool UpdateRequest(AppUser farmer, int requestId, string item, decimal amount, string unit, string location, string district, string preferredDate, out string error)
    { return Workflow.UpdateRequest(farmer, requestId, item, amount, unit, location, district, preferredDate, out error); }
    public bool DeleteRequest(AppUser farmer, int requestId, out string error)
    { return Workflow.DeleteRequest(farmer, requestId, out error); }
    public bool UpdateQuotation(AppUser supplier, int quoteId, decimal price, int stock, string delivery, out string error)
    { return Workflow.UpdateQuotation(supplier, quoteId, price, stock, delivery, out error); }
    public bool UpdateGuideline(AppUser officer, int guidelineId, string title, string text, out string error)
    {
        error = null;
        Guideline g = DemoData.Guidelines.Find(x => x.Id == guidelineId && x.Author == officer.Name);
        if (g == null) { error = "Guideline not found (you can only change your own guidelines)."; return false; }
        g.Title = title; g.Text = text;
        DemoData.Log(officer, "Edited guideline: " + title);
        return true;
    }
    public bool DeleteGuideline(AppUser officer, int guidelineId, out string error)
    {
        error = null;
        Guideline g = DemoData.Guidelines.Find(x => x.Id == guidelineId && x.Author == officer.Name);
        if (g == null) { error = "Guideline not found (you can only change your own guidelines)."; return false; }
        DemoData.Guidelines.Remove(g);
        DemoData.Log(officer, "Deleted guideline: " + g.Title);
        return true;
    }

    // ----- notifications / demo -----
    public void MarkNotificationsRead(int userId) { foreach (Notification n in DemoData.Notifications) if (n.UserId == userId) n.Read = true; }
    public void ResetDemoData() { DemoData.Reset(); }
}