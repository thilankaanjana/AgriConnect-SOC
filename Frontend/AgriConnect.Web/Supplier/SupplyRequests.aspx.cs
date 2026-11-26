using System;
using System.Collections.Generic;

public partial class Supplier_SupplyRequests : System.Web.UI.Page
{
    public class RequestRow
    {
        public DateTime Created { get; set; }
        public int Id { get; set; }
        public string RefNo { get; set; }
        public string Farmer { get; set; }
        public string Item { get; set; }
        public string Qty { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string MyQuote { get; set; }
        public bool CanQuote { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        AppUser me = Auth.Current;
        List<RequestRow> rows = new List<RequestRow>();
        foreach (SupplyRequest r in AppData.RequestsForSupplier(me))
        {
            Quotation mine = Workflow.QuoteBy(me, r.Id);
            rows.Add(new RequestRow
            {
                Created = r.Created,
                Id = r.Id,
                RefNo = r.RefNo,
                Farmer = r.Farmer,
                Item = r.Item,
                Qty = r.Qty,
                Location = r.Location + ", " + r.District,
                Date = r.Date,
                Status = r.Status,
                MyQuote = mine == null ? "" : mine.Status,
                CanQuote = mine == null && Workflow.IsOpenForQuotes(r)
            });
        }
        rows.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvRequests.DataSource = rows;
        gvRequests.DataBind();
    }
}