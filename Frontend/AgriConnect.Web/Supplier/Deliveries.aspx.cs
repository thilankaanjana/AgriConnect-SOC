using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Supplier_Deliveries : System.Web.UI.Page
{
    public class OrderRow
    {
        public int Id { get; set; }
        public string RefNo { get; set; }
        public string FarmerName { get; set; }
        public string ItemText { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public string DeliveryStatus { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrid(); }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        List<OrderRow> rows = new List<OrderRow>();
        foreach (Order o in AppData.Orders.FindAll(x => x.SupplierId == me))
            rows.Add(new OrderRow { Id = o.Id, RefNo = o.RefNo, FarmerName = o.FarmerName, ItemText = o.Item + " (" + o.Qty + ")", Location = o.Location, Price = o.Price, DeliveryStatus = o.DeliveryStatus });
        gvOrders.DataSource = rows;
        gvOrders.DataBind();
    }

    protected void ddlStatus_Changed(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        if (ddl.SelectedValue == "") return;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        int id = (int)gvOrders.DataKeys[row.RowIndex].Value;
        string error;
        if (AppData.Current.UpdateDelivery(Auth.Current, id, ddl.SelectedValue, out error))
            Master.Notify("Delivery updated and the farmer was notified.");
        else
            Master.Notify(error);
        BindGrid();
    }
}