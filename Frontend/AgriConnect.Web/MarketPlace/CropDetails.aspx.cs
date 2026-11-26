using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Marketplace_CropDetails : System.Web.UI.Page
{
    Crop crop;

    protected void Page_Load(object sender, EventArgs e)
    {
        int id;
        Crop c = null;
        if (int.TryParse(Request.QueryString["id"], out id)) c = AppData.Crops.Find(x => x.Id == id && x.Status == "Available");
        if (c == null) c = AppData.Crops.Find(x => x.Status == "Available");
        if (c == null) { pnlCrop.Visible = false; pnlBuy.Visible = false; pnlNone.Visible = true; return; }
        crop = c;
        lblQtyLabel.Text = "Quantity (kg) - available " + Server.HtmlEncode(c.Qty);
        imgCrop.ImageUrl = c.ImageUrl;
        litName.Text = Server.HtmlEncode(c.Name);
        litCrumb.Text = Server.HtmlEncode(c.Name);
        litCat.Text = Server.HtmlEncode(c.Category);
        litFarmer.Text = Server.HtmlEncode(c.Farmer);
        litQty.Text = Server.HtmlEncode(c.Qty);
        litLocation.Text = Server.HtmlEncode(c.Location);
        litPrice.Text = UIHelper.Money(c.Price);
        lnkCall.Text = "Contact Farmer: " + Server.HtmlEncode(c.Contact);
        lnkCall.NavigateUrl = "tel:" + c.Contact;
    }

    protected void btnBuy_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || crop == null) return;
        CropOrder o = new CropOrder
        {
            CropId = crop.Id,
            BuyerName = txtBuyerName.Text.Trim(),
            BuyerPhone = txtBuyerPhone.Text.Trim(),
            BuyerAddress = txtBuyerAddress.Text.Trim(),
            Quantity = txtBuyQty.Text.Trim() + " kg",
            Note = txtNote.Text.Trim()
        };
        string error;
        if (!AppData.Current.PlaceCropOrder(o, out error)) { lblBuyError.Text = error; return; }
        pnlBuy.Visible = false;
        pnlDone.Visible = true;
        litDone.Text = "Your order for " + Server.HtmlEncode(o.Quantity) + " of " + Server.HtmlEncode(crop.Name) + " was sent to " + Server.HtmlEncode(crop.Farmer)
            + ". The farmer will call you on " + Server.HtmlEncode(o.BuyerPhone) + " to confirm the price and delivery.";
    }
}