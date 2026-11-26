using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class Supplier_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AppUser me = Auth.Current;
        List<SupplyRequest> regional = AppData.RequestsForSupplier(me);
        List<SupplyRequest> open = regional.FindAll(r => Workflow.IsOpenForQuotes(r) && Workflow.QuoteBy(me, r.Id) == null);
        List<Quotation> mine = AppData.Quotes.FindAll(q => q.SupplierId == me.Id);
        List<Order> orders = AppData.Orders.FindAll(o => o.SupplierId == me.Id);
        List<Order> active = orders.FindAll(o => o.DeliveryStatus != "Delivered");

        litStats.Text =
            UIHelper.Stat("Stock Items", AppData.Inventory.FindAll(x => x.SupplierId == me.Id).Count) +
            UIHelper.Stat("Low / Out of Stock", AppData.Inventory.FindAll(x => x.SupplierId == me.Id && x.Qty <= 20).Count) +
            UIHelper.Stat("Open Requests", open.Count) +
            UIHelper.Stat("My Quotations", mine.Count) +
            UIHelper.Stat("Approved Quotations", mine.FindAll(q => q.Status == State.Approved || q.Status == State.Selected).Count) +
            UIHelper.Stat("Active Deliveries", active.Count);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < open.Count && i < 5; i++)
            sb.Append("<div class=\"mini\"><div><b>" + open[i].RefNo + "</b> &middot; " + HttpUtility.HtmlEncode(open[i].Item) + " (" + HttpUtility.HtmlEncode(open[i].Qty) + ") &middot; " + HttpUtility.HtmlEncode(open[i].District) +
                " <a href=\"" + ResolveUrl("~/Supplier/CreateQuotation.aspx?req=" + open[i].Id) + "\">Send quotation</a></div></div>");
        litOpen.Text = open.Count == 0 ? "<p>No open requests in your region right now.</p>" : sb.ToString();

        sb = new StringBuilder();
        for (int i = 0; i < active.Count && i < 5; i++)
            sb.Append("<div class=\"mini\"><div><b>" + active[i].RefNo + "</b> &middot; " + HttpUtility.HtmlEncode(active[i].Item) + " for " + HttpUtility.HtmlEncode(active[i].FarmerName) + " " + UIHelper.Tag(active[i].DeliveryStatus) + "</div></div>");
        litOrders.Text = active.Count == 0 ? "<p>No orders waiting for delivery.</p>" : sb.ToString();
    }
}