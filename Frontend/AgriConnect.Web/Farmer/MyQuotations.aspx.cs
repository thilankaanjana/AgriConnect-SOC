using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Farmer_MyQuotations : System.Web.UI.Page
{
    public class QuoteRow
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string RequestRef { get; set; }
        public string Item { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Delivery { get; set; }
        public string Status { get; set; }
        public bool IsLowest { get; set; }
        public bool CanConfirm { get; set; }
        public string ActionText { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrid(); }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        List<QuoteRow> rows = new List<QuoteRow>();
        int waiting = 0;
        foreach (SupplyRequest r in AppData.Requests.FindAll(x => x.FarmerId == me))
        {
            int requestId = r.Id;
            List<Quotation> quotes = AppData.Quotes.FindAll(q => q.RequestId == requestId);
            waiting += quotes.FindAll(q => q.Status == State.Pending).Count;
            // the farmer only sees quotations that the officer has verified
            List<Quotation> visible = quotes.FindAll(q => q.Status == State.Approved || q.Status == State.Selected || q.Status == State.NotSelected);
            decimal lowest = 0;
            foreach (Quotation q in visible) if (lowest == 0 || q.Price < lowest) lowest = q.Price;
            visible.Sort((x, y) => x.Price.CompareTo(y.Price));
            foreach (Quotation q in visible)
            {
                bool can = q.Status == State.Approved && r.Status == State.Approved;
                string text = q.Status == State.Selected ? "Order placed" : q.Status == State.NotSelected ? "Not selected" : "Waiting for request approval";
                rows.Add(new QuoteRow
                {
                    Id = q.Id,
                    RequestId = r.Id,
                    RequestRef = r.RefNo,
                    Item = r.Item + " (" + r.Qty + ")",
                    Supplier = q.Supplier,
                    Price = q.Price,
                    Stock = q.Stock,
                    Delivery = q.Delivery,
                    Status = q.Status,
                    IsLowest = visible.Count > 1 && q.Price == lowest,
                    CanConfirm = can,
                    ActionText = text
                });
            }
        }
        gvQuotes.DataSource = rows;
        gvQuotes.DataBind();
        pnlWaiting.Visible = waiting > 0;
        litWaiting.Text = waiting + " quotation(s) for your requests are waiting for the officer's verification. You will be notified when they are approved.";
    }

    protected void gvQuotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Confirm") return;
        string error;
        if (!AppData.Current.ConfirmOrder(Auth.Current, int.Parse(e.CommandArgument.ToString()), out error))
        {
            Master.Notify(error);
            BindGrid();
            return;
        }
        Response.Redirect("~/Farmer/MyOrders.aspx");
    }
}