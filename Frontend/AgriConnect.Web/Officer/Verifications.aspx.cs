using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

public partial class Officer_Verifications : System.Web.UI.Page
{
    public class RequestRow
    {
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public string RefNo { get; set; }
        public string Farmer { get; set; }
        public string ItemText { get; set; }
        public string District { get; set; }
        public string CreatedText { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public bool CanReview { get; set; }
    }

    public class QuoteRow
    {
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public string RefNo { get; set; }
        public string RequestRef { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Delivery { get; set; }
        public string Status { get; set; }
        public bool CanReview { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrids(); }

    void BindGrids()
    {
        int me = Auth.Current.Id;
        List<SupplyRequest> mine = AppData.Requests.FindAll(r => r.AssignedOfficerId == me);

        List<RequestRow> reqRows = new List<RequestRow>();
        foreach (SupplyRequest r in mine)
            reqRows.Add(new RequestRow { Created = r.Created, Id = r.Id, RefNo = r.RefNo, Farmer = r.Farmer, ItemText = r.Item + " (" + r.Qty + ")", District = r.District, CreatedText = UIHelper.ShortDate(r.Created), Status = r.Status, Note = r.ReviewNote, CanReview = r.Status == State.Pending });
        reqRows.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvRequests.DataSource = reqRows;
        gvRequests.DataBind();

        List<QuoteRow> quoteRows = new List<QuoteRow>();
        foreach (Quotation q in AppData.Quotes.FindAll(x => mine.Exists(r => r.Id == x.RequestId)))
            quoteRows.Add(new QuoteRow { Created = q.Created, Id = q.Id, RefNo = q.RefNo, RequestRef = q.RequestRef, Supplier = q.Supplier, Price = q.Price, Stock = q.Stock, Delivery = q.Delivery, Status = q.Status, CanReview = q.Status == State.Pending });
        quoteRows.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvQuotes.DataSource = quoteRows;
        gvQuotes.DataBind();
    }

    // ---------- open the review panel ----------
    void StartReview(string type, int id)
    {
        int me = Auth.Current.Id;
        StringBuilder d = new StringBuilder();
        string[] checks;
        if (type == "Request")
        {
            SupplyRequest r = AppData.Requests.Find(x => x.Id == id && x.AssignedOfficerId == me);
            if (r == null || r.Status != State.Pending) return;
            litTitle.Text = "Review " + r.RefNo;
            d.Append("<b>Farmer:</b> " + HttpUtility.HtmlEncode(r.Farmer) + "<br /><b>Item:</b> " + HttpUtility.HtmlEncode(r.Item) + " &middot; <b>Quantity:</b> " + HttpUtility.HtmlEncode(r.Qty));
            d.Append("<br /><b>Delivery:</b> " + HttpUtility.HtmlEncode(r.Location) + ", " + HttpUtility.HtmlEncode(r.District) + " &middot; <b>Preferred date:</b> " + HttpUtility.HtmlEncode(r.Date));
            d.Append("<br /><b>Quotations received:</b> " + AppData.Quotes.FindAll(q => q.RequestId == r.Id).Count);
            checks = Workflow.RequestChecks;
        }
        else
        {
            Quotation q = AppData.Quotes.Find(x => x.Id == id);
            SupplyRequest r = q == null ? null : AppData.Requests.Find(x => x.Id == q.RequestId && x.AssignedOfficerId == me);
            if (q == null || r == null || q.Status != State.Pending) return;
            litTitle.Text = "Review " + q.RefNo + " (" + r.RefNo + ")";
            decimal wanted = UIHelper.QtyNumber(r.Qty);
            d.Append("<b>Supplier:</b> " + HttpUtility.HtmlEncode(q.Supplier) + " &middot; <b>Price:</b> " + UIHelper.Money(q.Price));
            d.Append("<br /><b>Requested:</b> " + HttpUtility.HtmlEncode(r.Item) + " (" + HttpUtility.HtmlEncode(r.Qty) + ") &middot; <b>Supplier stock:</b> " + q.Stock + (wanted > 0 ? (q.Stock >= wanted ? " (covers the request)" : " (LESS than requested)") : ""));
            d.Append("<br /><b>Delivery estimate:</b> " + HttpUtility.HtmlEncode(q.Delivery));
            List<Quotation> others = AppData.Quotes.FindAll(x => x.RequestId == r.Id && x.Id != q.Id);
            foreach (Quotation o in others) d.Append("<br /><span class=\"muted\">Other quotation: " + HttpUtility.HtmlEncode(o.Supplier) + " " + UIHelper.Money(o.Price) + " (" + HttpUtility.HtmlEncode(o.Delivery) + ")</span>");
            checks = Workflow.QuoteChecks;
        }
        litDetail.Text = d.ToString();
        cblChecks.Items.Clear();
        foreach (string c in checks) cblChecks.Items.Add(new ListItem(c));
        txtNote.Text = "";
        hdnType.Value = type;
        hdnId.Value = id.ToString();
        pnlReview.Visible = true;
    }

    protected void gvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Review") StartReview("Request", int.Parse(e.CommandArgument.ToString()));
    }

    protected void gvQuotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Review") StartReview("Quotation", int.Parse(e.CommandArgument.ToString()));
    }

    // ---------- decide ----------
    void Decide(bool approve)
    {
        int passed = 0;
        foreach (ListItem c in cblChecks.Items) if (c.Selected) passed++;
        string note = txtNote.Text.Trim();
        if (approve && passed < cblChecks.Items.Count) { lblError.Text = "Tick every compliance check before approving, or reject the item with a note."; return; }
        if (!approve && note == "") { lblError.Text = "Write a note explaining why the item is rejected."; return; }

        string error;
        int id = int.Parse(hdnId.Value);
        bool ok = hdnType.Value == "Request"
            ? AppData.Current.ReviewRequest(Auth.Current, id, approve, note, passed, out error)
            : AppData.Current.ReviewQuotation(Auth.Current, id, approve, note, passed, out error);
        pnlReview.Visible = false;
        BindGrids();
        Master.Notify(ok ? (approve ? "Approved. The farmer / supplier was notified." : "Rejected. The farmer / supplier was notified.") : error);
    }

    protected void btnApprove_Click(object sender, EventArgs e) { Decide(true); }
    protected void btnReject_Click(object sender, EventArgs e) { Decide(false); }
    protected void btnCancel_Click(object sender, EventArgs e) { pnlReview.Visible = false; }
}