using System;
using System.Text;
using System.Web;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    // First item = section title, rest = "Text|URL"
    static readonly string[] FarmerMenu = { "Farmer", "Dashboard|~/Farmer/Dashboard.aspx", "Supply Request|~/Farmer/SupplyRequest.aspx", "Supplier Stock|~/Farmer/SupplierStock.aspx", "My Quotations|~/Farmer/MyQuotations.aspx", "My Orders|~/Farmer/MyOrders.aspx", "Crop Listings|~/Farmer/CropListings.aspx", "Buyer Orders|~/Farmer/CropOrders.aspx" };
    static readonly string[] SupplierMenu = { "Supplier", "Dashboard|~/Supplier/Dashboard.aspx", "Inventory|~/Supplier/Inventory.aspx", "Supply Requests|~/Supplier/SupplyRequests.aspx", "Create Quotation|~/Supplier/CreateQuotation.aspx", "My Quotations|~/Supplier/MyQuotations.aspx", "Deliveries|~/Supplier/Deliveries.aspx" };
    static readonly string[] OfficerMenu = { "Agriculture Officer", "Dashboard|~/Officer/Dashboard.aspx", "Verifications|~/Officer/Verifications.aspx", "Compliance Monitoring|~/Officer/Compliance.aspx", "Recommendations|~/Officer/Recommendations.aspx" };
    static readonly string[] AdminMenu = { "Administrator", "Dashboard|~/Admin/Dashboard.aspx", "User Management|~/Admin/Users.aspx", "Transactions|~/Admin/Transactions.aspx", "Analytics|~/Admin/Analytics.aspx", "System Monitor|~/Admin/SystemMonitor.aspx", "Audit Logs|~/Admin/ActivityLog.aspx" };
    static readonly string[] MarketMenu = { "Marketplace", "Marketplace (Buy Crops)|~/Default.aspx" };

    // Access control runs before the page's own Page_Load.
    protected void Page_Init(object sender, EventArgs e)
    {
        string path = Request.AppRelativeCurrentExecutionFilePath.ToLower();
        AppUser u = Auth.Current;

        string required = null;
        if (path.StartsWith("~/farmer/")) required = "Farmer";
        else if (path.StartsWith("~/supplier/")) required = "Supplier";
        else if (path.StartsWith("~/officer/")) required = "Officer";
        else if (path.StartsWith("~/admin/")) required = "Admin";

        if (u == null) Response.Redirect("~/Default.aspx");
        else if (required != null && u.Role != required) Response.Redirect(Auth.HomeUrl(u.Role));
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AppUser u = Auth.Current;
        string current = Request.AppRelativeCurrentExecutionFilePath;

        StringBuilder sb = new StringBuilder();
        if (u != null)
        {
            switch (u.Role)
            {
                case "Farmer": AppendSection(sb, FarmerMenu, current); break;
                case "Supplier": AppendSection(sb, SupplierMenu, current); break;
                case "Officer": AppendSection(sb, OfficerMenu, current); break;
                case "Admin": AppendSection(sb, AdminMenu, current); break;
            }
            int unread = AppData.Current.GetUnreadCount(u.Id);
            AppendSection(sb, new string[] { "Account", "My Profile|~/Profile.aspx", "Notifications" + (unread > 0 ? " (" + unread + ")" : "") + "|~/Notifications.aspx" }, current);
            lnkBell.Visible = true;
            lnkBell.Text = unread > 0 ? "Notifications (" + unread + ")" : "Notifications";
        }
        AppendSection(sb, MarketMenu, current);
        litNav.Text = sb.ToString();
        litCrumb.Text = HttpUtility.HtmlEncode(Page.Title);

        if (u != null)
        {
            litUser.Text = HttpUtility.HtmlEncode(u.Name + " (" + Auth.RoleLabel(u.Role) + ")");
            btnLogout.Visible = true;
            btnReset.Visible = u.Role == "Admin";
        }
    }

    static void AppendSection(StringBuilder sb, string[] sec, string current)
    {
        sb.Append("<h5>" + HttpUtility.HtmlEncode(sec[0]) + "</h5>");
        for (int i = 1; i < sec.Length; i++)
        {
            string[] p = sec[i].Split('|');
            bool active = string.Equals(p[1], current, StringComparison.OrdinalIgnoreCase);
            sb.Append("<a class=\"nav" + (active ? " active" : "") + "\" href=\"" + VirtualPathUtility.ToAbsolute(p[1]) + "\">" + HttpUtility.HtmlEncode(p[0]) + "</a>");
        }
    }

    /// <summary>Shows a message bar (replaces the toast in the HTML prototype).</summary>
    public void Notify(string message)
    {
        pnlMsg.Visible = true;
        litMsg.Text = HttpUtility.HtmlEncode(message);
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Auth.SignOut();
        Response.Redirect("~/Default.aspx");
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        AppData.Current.ResetDemoData();
        Response.Redirect("~/Admin/Dashboard.aspx");
    }
}