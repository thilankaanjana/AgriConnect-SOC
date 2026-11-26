using System;
using System.Collections.Generic;

public partial class Admin_Transactions : System.Web.UI.Page
{
    public class TxRow
    {
        public DateTime Created { get; set; }
        public string DateText { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        public string Parties { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        decimal orderValue = 0;
        foreach (Order o in AppData.Orders) orderValue += o.Price;
        litStats.Text =
            UIHelper.Stat("Supply Requests", AppData.Requests.Count) +
            UIHelper.Stat("Quotations", AppData.Quotes.Count) +
            UIHelper.Stat("Confirmed Orders", AppData.Orders.Count) +
            UIHelper.StatText("Order Value", UIHelper.Money(orderValue));
        BindGrid();
    }

    void BindGrid()
    {
        string type = ddlType.SelectedValue;
        List<TxRow> rows = new List<TxRow>();
        if (type == "" || type == "Request")
            foreach (SupplyRequest r in AppData.Requests)
                rows.Add(new TxRow { Created = r.Created, Type = "Supply request", Reference = r.RefNo, Parties = r.Farmer + " (officer: " + r.AssignedOfficer + ")", Details = r.Item + " (" + r.Qty + "), " + r.District, Amount = 0, Status = r.Status });
        if (type == "" || type == "Quotation")
            foreach (Quotation q in AppData.Quotes)
            {
                int requestId = q.RequestId;
                SupplyRequest req = AppData.Requests.Find(x => x.Id == requestId);
                rows.Add(new TxRow { Created = q.Created, Type = "Quotation", Reference = q.RefNo, Parties = q.Supplier + " > " + (req == null ? "" : req.Farmer), Details = q.RequestRef + ", delivery " + q.Delivery, Amount = q.Price, Status = q.Status });
            }
        if (type == "" || type == "Order")
            foreach (Order o in AppData.Orders)
                rows.Add(new TxRow { Created = o.Created, Type = "Order", Reference = o.RefNo, Parties = o.FarmerName + " / " + o.Supplier, Details = o.Item + " (" + o.Qty + ")", Amount = o.Price, Status = o.DeliveryStatus });
        if (type == "" || type == "Sale")
            foreach (CropOrder c in AppData.Current.GetCropOrders(Auth.Current))
                rows.Add(new TxRow { Created = c.Created, Type = "Buyer crop order", Reference = c.RefNo, Parties = c.BuyerName + " > " + c.Farmer, Details = c.Crop + " (" + c.Quantity + ")", Amount = c.Total, Status = c.Status });
        rows.Sort((x, y) => y.Created.CompareTo(x.Created));
        foreach (TxRow row in rows) row.DateText = UIHelper.DateTimeText(row.Created);
        gvTx.DataSource = rows;
        gvTx.DataBind();
    }

    protected void ddlType_Changed(object sender, EventArgs e) { BindGrid(); }
}