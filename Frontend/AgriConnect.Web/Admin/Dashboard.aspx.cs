using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class Admin_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        decimal value = 0;
        foreach (Order o in AppData.Orders) value += o.Price;
        litStats.Text =
            UIHelper.Stat("Users", AppData.Users.Count) +
            UIHelper.Stat("Farmers", AppData.Users.FindAll(x => x.Role == "Farmer").Count) +
            UIHelper.Stat("Suppliers", AppData.Users.FindAll(x => x.Role == "Supplier").Count) +
            UIHelper.Stat("Officers", AppData.Users.FindAll(x => x.Role == "Officer").Count) +
            UIHelper.Stat("Waiting for Approval", AppData.Users.FindAll(x => x.Status == "Pending").Count) +
            UIHelper.Stat("Supply Requests", AppData.Requests.Count) +
            UIHelper.Stat("Quotations", AppData.Quotes.Count) +
            UIHelper.Stat("Confirmed Orders", AppData.Orders.Count) +
            UIHelper.StatText("Order Value", UIHelper.Money(value));

        List<ActivityEntry> log = new List<ActivityEntry>(AppData.Activity);
        log.Sort((x, y) => y.Created.CompareTo(x.Created));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < log.Count && i < 8; i++)
            sb.Append("<div class=\"mini\"><div><b>" + HttpUtility.HtmlEncode(log[i].User) + "</b> (" + HttpUtility.HtmlEncode(log[i].Role) + ") &middot; " + HttpUtility.HtmlEncode(log[i].Action) + "<br /><span class=\"muted\">" + UIHelper.DateTimeText(log[i].Created) + "</span></div></div>");
        litActivity.Text = sb.ToString();
    }
}