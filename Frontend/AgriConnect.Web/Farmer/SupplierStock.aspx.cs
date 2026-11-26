using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

public partial class Farmer_SupplierStock : System.Web.UI.Page
{
    public class StockRow
    {
        public string Item { get; set; }
        public string Supplier { get; set; }
        public string District { get; set; }
        public string Available { get; set; }
        public string PriceText { get; set; }
        public string Level { get; set; }
        public string Contact { get; set; }
        public string RequestUrl { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ddlDistrict.Items.Add(new ListItem("All districts", ""));
        foreach (string d in Lookups.Districts) ddlDistrict.Items.Add(d);
        ListItem home = ddlDistrict.Items.FindByValue(Auth.Current.District ?? "");
        if (home != null) home.Selected = true;   // start with suppliers of the farmer's own district
        BindGrid();
    }

    void BindGrid()
    {
        string q = txtSearch.Text.Trim().ToLower(), district = ddlDistrict.SelectedValue;
        List<InventoryItem> all = AppData.Current.GetSupplierStock();
        List<StockRow> rows = new List<StockRow>();
        foreach (InventoryItem i in all)
        {
            if (district != "" && i.District != district) continue;
            if (q != "" && (i.Item ?? "").ToLower().IndexOf(q) < 0 && (i.Supplier ?? "").ToLower().IndexOf(q) < 0) continue;
            rows.Add(new StockRow
            {
                Item = i.Item,
                Supplier = i.Supplier,
                District = i.District,
                Available = i.Qty + " " + i.Unit,
                PriceText = UIHelper.Money(i.Price) + " / " + i.Unit,
                Contact = i.Contact,
                Level = i.Qty <= 0 ? "Out of stock" : (i.Qty <= 20 ? "Low stock" : "In stock"),
                RequestUrl = "~/Farmer/SupplyRequest.aspx?item=" + HttpUtility.UrlEncode(i.Item) + "&unit=" + HttpUtility.UrlEncode(i.Unit) + "&district=" + HttpUtility.UrlEncode(i.District)
            });
        }
        rows.Sort((a, b) => string.Compare(a.Item, b.Item, StringComparison.OrdinalIgnoreCase));
        gvStock.DataSource = rows;
        gvStock.DataBind();
        lblCount.Text = rows.Count + " item(s)";
    }

    protected void btnFilter_Click(object sender, EventArgs e) { BindGrid(); }
}