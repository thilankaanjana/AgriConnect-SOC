using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Supplier_MyQuotations : System.Web.UI.Page
{
    public class QuoteRow
    {
        public int Id { get; set; }
        public bool CanEdit { get; set; }
        public DateTime Created { get; set; }
        public string RefNo { get; set; }
        public string RequestRef { get; set; }
        public string ItemText { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Delivery { get; set; }
        public string Status { get; set; }
        public string ReviewNote { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        BindGrid();
    }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        List<QuoteRow> rows = new List<QuoteRow>();
        foreach (Quotation q in AppData.Quotes.FindAll(x => x.SupplierId == me))
        {
            int requestId = q.RequestId;
            SupplyRequest r = AppData.Requests.Find(x => x.Id == requestId);
            rows.Add(new QuoteRow
            {
                Id = q.Id,
                CanEdit = q.Status == State.Pending,   // only while the officer has not verified it yet
                Created = q.Created,
                RefNo = q.RefNo,
                RequestRef = q.RequestRef,
                ItemText = r == null ? "" : r.Item + " (" + r.Qty + ")",
                Price = q.Price,
                Stock = q.Stock,
                Delivery = q.Delivery,
                Status = q.Status,
                ReviewNote = q.ReviewNote
            });
        }
        rows.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvQuotes.DataSource = rows;
        gvQuotes.DataBind();
    }

    int EditId
    {
        get { return ViewState["EditId"] == null ? 0 : (int)ViewState["EditId"]; }
        set { ViewState["EditId"] = value; }
    }

    protected void gvQuotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditQuote") return;
        int id;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
        Quotation q = AppData.Quotes.Find(x => x.Id == id && x.SupplierId == Auth.Current.Id);
        if (q == null || q.Status != State.Pending) { Master.Notify("This quotation can no longer be edited."); return; }
        EditId = q.Id;
        litEditRef.Text = q.RefNo;
        txtPrice.Text = q.Price.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
        txtStock.Text = q.Stock.ToString();
        txtDelivery.Text = q.Delivery;
        pnlEdit.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || EditId == 0) return;
        string error;
        if (!AppData.Current.UpdateQuotation(Auth.Current, EditId, decimal.Parse(txtPrice.Text, System.Globalization.CultureInfo.InvariantCulture), int.Parse(txtStock.Text), txtDelivery.Text.Trim(), out error))
        {
            lblError.Text = error;
            return;
        }
        EditId = 0;
        pnlEdit.Visible = false;
        BindGrid();
        Master.Notify("Quotation updated. The officer was notified.");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        EditId = 0;
        pnlEdit.Visible = false;
    }
}