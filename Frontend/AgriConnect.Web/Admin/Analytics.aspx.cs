using System;
using System.Collections.Generic;
using System.Text;

public partial class Admin_Analytics : System.Web.UI.Page
{
    static string Bars(List<KeyValuePair<string, int>> items)
    {
        int max = 0;
        foreach (KeyValuePair<string, int> kv in items) if (kv.Value > max) max = kv.Value;
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, int> kv in items) sb.Append(UIHelper.Bar(kv.Key, kv.Value, max));
        return sb.Length == 0 ? "<p>No data yet.</p>" : sb.ToString();
    }

    static KeyValuePair<string, int> Item(string label, int value) { return new KeyValuePair<string, int>(label, value); }

    protected void Page_Load(object sender, EventArgs e)
    {
        decimal value = 0;
        foreach (Order o in AppData.Orders) value += o.Price;
        int decided = AppData.Quotes.FindAll(q => q.Status != State.Pending).Count;
        int good = AppData.Quotes.FindAll(q => q.Status == State.Approved || q.Status == State.Selected || q.Status == State.NotSelected).Count;
        litStats.Text =
            UIHelper.Stat("Total Users", AppData.Users.Count) +
            UIHelper.Stat("Supply Requests", AppData.Requests.Count) +
            UIHelper.Stat("Quotations", AppData.Quotes.Count) +
            UIHelper.Stat("Confirmed Orders", AppData.Orders.Count) +
            UIHelper.StatText("Order Value", UIHelper.Money(value)) +
            UIHelper.StatText("Quotation Approval Rate", decided == 0 ? "-" : (good * 100 / decided) + "%");

        List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();
        foreach (string r in new string[] { "Farmer", "Supplier", "Officer", "Admin" })
        { string role = r; items.Add(Item(Auth.RoleLabel(role), AppData.Users.FindAll(u => u.Role == role).Count)); }
        litRoles.Text = Bars(items);

        items = new List<KeyValuePair<string, int>>();
        foreach (string s in new string[] { State.Pending, State.Approved, State.Rejected, State.Confirmed })
        { string st = s; items.Add(Item(st, AppData.Requests.FindAll(r => r.Status == st).Count)); }
        litRequests.Text = Bars(items);

        items = new List<KeyValuePair<string, int>>();
        foreach (string s in new string[] { State.Pending, State.Approved, State.Rejected, State.Selected, State.NotSelected, State.Closed })
        { string st = s; items.Add(Item(st, AppData.Quotes.FindAll(q => q.Status == st).Count)); }
        litQuotes.Text = Bars(items);

        items = new List<KeyValuePair<string, int>>();
        foreach (string s in Workflow.DeliverySteps)
        { string st = s; items.Add(Item(st, AppData.Orders.FindAll(o => o.DeliveryStatus == st).Count)); }
        litDeliveries.Text = Bars(items);

        items = new List<KeyValuePair<string, int>>();
        foreach (string d in Lookups.Districts)
        { string dist = d; int n = AppData.Requests.FindAll(r => r.District == dist).Count; if (n > 0) items.Add(Item(dist, n)); }
        litDistricts.Text = Bars(items);

        items = new List<KeyValuePair<string, int>>();
        foreach (string c in new string[] { "Vegetables", "Fruits", "Grains" })
        { string cat = c; items.Add(Item(cat, AppData.Crops.FindAll(x => x.Category == cat && x.Status == "Available").Count)); }
        litMarket.Text = Bars(items) + "<p class=\"muted\">Active listings by category. Sold-out: " + AppData.Crops.FindAll(x => x.Status != "Available").Count + "</p>";
    }
}