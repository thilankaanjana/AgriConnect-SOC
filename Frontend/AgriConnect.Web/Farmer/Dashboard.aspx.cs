using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class Farmer_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int me = Auth.Current.Id;
        List<SupplyRequest> mine = AppData.Requests.FindAll(x => x.FarmerId == me);
        int toCompare = AppData.Quotes.FindAll(q => q.Status == State.Approved &&
            mine.Exists(r => r.Id == q.RequestId && r.Status == State.Approved)).Count;

        litStats.Text =
            UIHelper.Stat("My Requests", mine.Count) +
            UIHelper.Stat("Awaiting Verification", mine.FindAll(x => x.Status == State.Pending).Count) +
            UIHelper.Stat("Quotations To Compare", toCompare) +
            UIHelper.Stat("Confirmed Orders", AppData.Orders.FindAll(x => x.FarmerId == me).Count) +
            UIHelper.Stat("Buyer Orders Waiting", AppData.Current.GetCropOrders(Auth.Current).FindAll(x => x.Status == State.BuyPending).Count) +
            UIHelper.Stat("Active Crop Listings", AppData.Crops.FindAll(x => x.FarmerId == me && x.Status == "Available").Count);

        // workflow tracker for the latest requests
        mine.Sort((x, y) => y.Created.CompareTo(x.Created));
        StringBuilder flow = new StringBuilder();
        for (int i = 0; i < mine.Count && i < 5; i++)
        {
            SupplyRequest r = mine[i];
            flow.Append("<div class=\"mini\"><div><b>" + r.RefNo + "</b> &middot; " + HttpUtility.HtmlEncode(r.Item) + " (" + HttpUtility.HtmlEncode(r.Qty) + ") &nbsp;" + UIHelper.Tag(r.Status) + "<br />");
            flow.Append(r.Status == State.Rejected ? "<span class=\"muted\">Rejected by the officer. " + HttpUtility.HtmlEncode(r.ReviewNote ?? "") + "</span>" : UIHelper.Stepper(Workflow.ProgressLabels, Workflow.Progress(r)));
            flow.Append("</div></div>");
        }
        litFlow.Text = mine.Count == 0 ? "<p>You have not made any supply requests yet.</p>" : flow.ToString();

        // latest notifications
        List<Notification> notes = AppData.Notifications(me);
        notes.Sort((x, y) => y.Created.CompareTo(x.Created));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < notes.Count && i < 4; i++)
            sb.Append("<div class=\"nitem" + (notes[i].Read ? "" : " new") + "\"><span class=\"dot\"></span><div><a href=\"" + ResolveUrl(notes[i].Link) + "\">" + HttpUtility.HtmlEncode(notes[i].Text) + "</a><small>" + UIHelper.DateTimeText(notes[i].Created) + "</small></div></div>");
        litNotes.Text = notes.Count == 0 ? "<p>No notifications yet.</p>" : sb.ToString();
    }
}