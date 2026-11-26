using System;
using System.Collections.Generic;

public partial class Farmer_MyOrders : System.Web.UI.Page
{
    public class OrderRow
    {
        public string RefNo { get; set; }
        public string RequestRef { get; set; }
        public string ItemText { get; set; }
        public string Supplier { get; set; }
        public decimal Price { get; set; }
        public string DeliveryStatus { get; set; }
        public string Tracker { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        int me = Auth.Current.Id;
        List<OrderRow> rows = new List<OrderRow>();
        foreach (Order o in AppData.Orders.FindAll(x => x.FarmerId == me))
        {
            int step = Array.IndexOf(Workflow.DeliverySteps, o.DeliveryStatus);
            rows.Add(new OrderRow
            {
                RefNo = o.RefNo,
                RequestRef = o.RequestRef,
                ItemText = o.Item + " (" + o.Qty + ")",
                Supplier = o.Supplier,
                Price = o.Price,
                DeliveryStatus = o.DeliveryStatus,
                Tracker = UIHelper.Stepper(new string[] { "Confirmed", "Dispatched", "Delivered" }, new bool[] { true, step >= 1, step >= 2 })
            });
        }
        gvOrders.DataSource = rows;
        gvOrders.DataBind();
    }
}