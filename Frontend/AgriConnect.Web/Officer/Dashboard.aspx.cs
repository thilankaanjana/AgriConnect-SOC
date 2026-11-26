using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class Officer_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int me = Auth.Current.Id;
        List<SupplyRequest> mine = AppData.Requests.FindAll(r => r.AssignedOfficerId == me);
        List<Quotation> quotes = AppData.Quotes.FindAll(q => mine.Exists(r => r.Id == q.RequestId));
        List<SupplyRequest> pendingR = mine.FindAll(r => r.Status == State.Pending);
        List<Quotation> pendingQ = quotes.FindAll(q => q.Status == State.Pending);

        litStats.Text =
            UIHelper.Stat("Pending Requests", pendingR.Count) +
            UIHelper.Stat("Pending Quotations", pendingQ.Count) +
            UIHelper.Stat("Approved Items", mine.FindAll(r => r.Status == State.Approved || r.Status == State.Confirmed).Count + quotes.FindAll(q => q.Status == State.Approved || q.Status == State.Selected || q.Status == State.NotSelected).Count) +
            UIHelper.Stat("Rejected Items", mine.FindAll(r => r.Status == State.Rejected).Count + quotes.FindAll(q => q.Status == State.Rejected).Count) +
            UIHelper.Stat("Recommendations", AppData.Recommendations.FindAll(x => x.OfficerId == me).Count);

        StringBuilder sb = new StringBuilder();
        foreach (SupplyRequest r in pendingR)
            sb.Append("<div class=\"mini\"><div><b>" + r.RefNo + "</b> &middot; " + HttpUtility.HtmlEncode(r.Item) + " (" + HttpUtility.HtmlEncode(r.Qty) + ") from " + HttpUtility.HtmlEncode(r.Farmer) + " &middot; " + HttpUtility.HtmlEncode(r.District) + "</div></div>");
        foreach (Quotation q in pendingQ)
            sb.Append("<div class=\"mini\"><div><b>" + q.RefNo + "</b> &middot; quotation from " + HttpUtility.HtmlEncode(q.Supplier) + " for " + q.RequestRef + " &middot; " + UIHelper.Money(q.Price) + "</div></div>");
        litPending.Text = sb.Length == 0 ? "<p>Nothing is waiting for verification.</p>" : sb.ToString();
    }
}