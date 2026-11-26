using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

public partial class Supplier_CreateQuotation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        AppUser me = Auth.Current;
        List<SupplyRequest> open = AppData.RequestsForSupplier(me).FindAll(r => Workflow.IsOpenForQuotes(r) && Workflow.QuoteBy(me, r.Id) == null);
        pnlForm.Visible = open.Count > 0;
        pnlNone.Visible = open.Count == 0;
        foreach (SupplyRequest r in open)
            ddlRequest.Items.Add(new ListItem(r.RefNo + " - " + r.Item + " (" + r.Qty + "), " + r.District, r.Id.ToString()));

        string wanted = Request.QueryString["req"];
        if (!string.IsNullOrEmpty(wanted))
        {
            ListItem li = ddlRequest.Items.FindByValue(wanted);
            if (li != null) { ddlRequest.ClearSelection(); li.Selected = true; }
        }
        ShowRequest(true);
    }

    // shows the selected request and suggests the stock from the supplier's inventory
    void ShowRequest(bool prefill)
    {
        if (ddlRequest.Items.Count == 0) return;
        int id = int.Parse(ddlRequest.SelectedValue);
        SupplyRequest r = AppData.Requests.Find(x => x.Id == id);
        if (r == null) return;
        litRequest.Text = "<b>" + r.RefNo + "</b> from <b>" + HttpUtility.HtmlEncode(r.Farmer) + "</b><br />Item: " + HttpUtility.HtmlEncode(r.Item) +
            " &middot; Quantity: " + HttpUtility.HtmlEncode(r.Qty) + "<br />Deliver to: " + HttpUtility.HtmlEncode(r.Location) + ", " + HttpUtility.HtmlEncode(r.District) +
            " &middot; Preferred date: " + HttpUtility.HtmlEncode(r.Date);
        if (prefill)
        {
            InventoryItem stock = AppData.Inventory.Find(x => x.SupplierId == Auth.Current.Id && string.Equals(x.Item, r.Item, StringComparison.OrdinalIgnoreCase));
            txtStock.Text = stock == null ? "" : stock.Qty.ToString();
        }
    }

    protected void ddlRequest_Changed(object sender, EventArgs e) { ShowRequest(true); }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || ddlRequest.Items.Count == 0) return;
        string error;
        if (!AppData.Current.SubmitQuotation(Auth.Current, int.Parse(ddlRequest.SelectedValue), decimal.Parse(txtPrice.Text), int.Parse(txtStock.Text), txtDelivery.Text.Trim(), out error))
        {
            lblError.Text = error;
            return;
        }
        Response.Redirect("~/Supplier/MyQuotations.aspx");
    }
}