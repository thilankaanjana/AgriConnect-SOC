using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

public partial class Farmer_CropListings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        BindGrid();
        txtContact.Text = Auth.Current.Contact;
        txtLocation.Text = Auth.Current.Address;
    }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        List<Crop> mine = AppData.Crops.FindAll(x => x.FarmerId == me);
        mine.Sort((x, y) => y.Id.CompareTo(x.Id));
        gvCrops.DataSource = mine;
        gvCrops.DataBind();
    }

    static readonly string[] AllowedExt = { ".jpg", ".jpeg", ".png", ".webp" };
    const int MaxBytes = 3 * 1024 * 1024;

    protected void cvPhoto_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!fuPhoto.HasFile) { args.IsValid = true; return; }   // photo is optional (a default picture is used)
        string ext = Path.GetExtension(fuPhoto.FileName).ToLowerInvariant();
        args.IsValid = Array.IndexOf(AllowedExt, ext) >= 0 && fuPhoto.PostedFile.ContentLength <= MaxBytes && fuPhoto.PostedFile.ContentType.StartsWith("image/");
    }

    // Saves the uploaded photo under ~/Uploads/crops with a random name and returns its app-relative path ("" if none).
    string SavePhoto()
    {
        if (!fuPhoto.HasFile) return "";
        string ext = Path.GetExtension(fuPhoto.FileName).ToLowerInvariant();
        string name = Guid.NewGuid().ToString("N") + ext;
        string dir = Server.MapPath("~/Uploads/crops");
        Directory.CreateDirectory(dir);
        fuPhoto.SaveAs(Path.Combine(dir, name));
        return "~/Uploads/crops/" + name;
    }

    // 0 = adding a new crop, otherwise the id of the crop being edited
    int EditId
    {
        get { return ViewState["EditId"] == null ? 0 : (int)ViewState["EditId"]; }
        set { ViewState["EditId"] = value; }
    }

    void ShowMode()
    {
        bool editing = EditId != 0;
        btnPublish.Text = editing ? "Save Changes" : "Publish Crop Listing";
        btnCancel.Visible = editing;
        litMode.Text = editing ? "<div class=\"notice\">Editing a crop listing. Choose a new photo only if you want to replace the old one.</div>" : "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        EditId = 0;
        txtName.Text = txtQty.Text = txtPrice.Text = "";
        ShowMode();
    }

    protected void btnPublish_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (EditId != 0)
        {
            Crop edited = new Crop
            {
                Id = EditId,
                Name = txtName.Text.Trim(),
                Qty = txtQty.Text.Trim(),
                Price = decimal.Parse(txtPrice.Text),
                Location = txtLocation.Text.Trim(),
                Contact = txtContact.Text.Trim(),
                Tag = ddlQuality.SelectedValue,
                Photo = SavePhoto()
            };
            string error;
            if (!AppData.Current.UpdateCrop(Auth.Current, edited, out error)) { Master.Notify(error); return; }
            EditId = 0;
            BindGrid();
            ShowMode();
            txtName.Text = txtQty.Text = txtPrice.Text = "";
            Master.Notify("Crop listing updated. The Marketplace shows the new details.");
            return;
        }
        Crop c = new Crop
        {
            FarmerId = Auth.Current.Id,
            Farmer = Auth.Current.Name,
            Name = txtName.Text.Trim(),
            Qty = txtQty.Text.Trim(),
            Price = decimal.Parse(txtPrice.Text),
            Location = txtLocation.Text.Trim(),
            Contact = txtContact.Text.Trim(),
            Tag = ddlQuality.SelectedValue
        };
        c.Photo = SavePhoto();
        AppData.Current.AddCrop(Auth.Current, c);
        BindGrid();
        Master.Notify("Crop listing published. Buyers can now see it in the Marketplace (Buy Crops) and place orders - you will find them under Buyer Orders.");
        txtName.Text = txtQty.Text = txtPrice.Text = "";
    }

    protected void gvCrops_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
        Crop c = AppData.Crops.Find(x => x.Id == id && x.FarmerId == Auth.Current.Id);
        if (c == null) return;
        if (e.CommandName == "EditCrop")
        {
            EditId = c.Id;
            txtName.Text = c.Name;
            txtQty.Text = c.Qty;
            txtPrice.Text = c.Price.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            txtLocation.Text = c.Location;
            txtContact.Text = c.Contact;
            ddlQuality.ClearSelection();
            ListItem q = ddlQuality.Items.FindByValue(c.Tag ?? "");
            if (q != null) q.Selected = true;
            ShowMode();
            return;
        }
        if (e.CommandName == "Toggle")
        {
            AppData.Current.SetCropStatus(Auth.Current, c.Id, c.Status == "Available" ? "Sold Out" : "Available");
        }
        else if (e.CommandName == "Remove")
        {
            AppData.Current.DeleteCrop(Auth.Current, c.Id);
            if (EditId == c.Id) { EditId = 0; ShowMode(); }
        }
        BindGrid();
    }
}