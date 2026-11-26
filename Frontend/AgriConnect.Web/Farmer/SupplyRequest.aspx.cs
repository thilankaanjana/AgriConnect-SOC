using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Farmer_SupplyRequest : System.Web.UI.Page
{
    public class RequestRow
    {
        public int Id { get; set; }
        public bool CanChange { get; set; }
        public DateTime Created { get; set; }
        public string RefNo { get; set; }
        public string Item { get; set; }
        public string Qty { get; set; }
        public string District { get; set; }
        public string Officer { get; set; }
        public int Quotes { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string Advice { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ddlDistrict.DataSource = Lookups.Districts;
        ddlDistrict.DataBind();
        ListItem home = ddlDistrict.Items.FindByValue(Auth.Current.District ?? "");
        if (home != null) home.Selected = true;
        txtLocation.Text = Auth.Current.Address;
        // coming from "Supplier Stock" -> Request this item
        string item = Request.QueryString["item"], unit = Request.QueryString["unit"], dist = Request.QueryString["district"];
        if (!string.IsNullOrEmpty(item)) txtItem.Text = item;
        if (!string.IsNullOrEmpty(unit) && ddlUnit.Items.FindByValue(unit) != null) ddlUnit.SelectedValue = unit;
        if (!string.IsNullOrEmpty(dist) && ddlDistrict.Items.FindByValue(dist) != null) { ddlDistrict.ClearSelection(); ddlDistrict.SelectedValue = dist; }
        BindGrid();
    }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        List<RequestRow> rows = new List<RequestRow>();
        foreach (SupplyRequest r in AppData.Requests.FindAll(x => x.FarmerId == me))
        {
            int requestId = r.Id;
            Recommendation rec = AppData.Recommendations.FindLast(x => x.RequestId == requestId);
            rows.Add(new RequestRow
            {
                Id = r.Id,
                CanChange = Workflow.CanChangeRequest(r),
                Created = r.Created,
                RefNo = r.RefNo,
                Item = r.Item,
                Qty = r.Qty,
                District = r.District,
                Officer = r.AssignedOfficer,
                Quotes = AppData.Quotes.FindAll(q => q.RequestId == requestId).Count,
                Status = r.Status,
                Note = r.ReviewNote,
                Advice = rec == null ? "" : rec.Text
            });
        }
        rows.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvRequests.DataSource = rows;
        gvRequests.DataBind();
    }

    // 0 = new request, otherwise the id of the request being edited
    int EditId
    {
        get { return ViewState["EditId"] == null ? 0 : (int)ViewState["EditId"]; }
        set { ViewState["EditId"] = value; }
    }

    void ShowMode()
    {
        bool editing = EditId != 0;
        btnSubmit.Text = editing ? "Save Changes" : "Submit Request";
        btnCancel.Visible = editing;
        litMode.Text = editing ? "<div class=\"notice\">Editing a supply request. It can be changed only until a supplier sends a quotation or the officer verifies it.</div>" : "";
    }

    void ClearForm()
    {
        EditId = 0;
        txtItem.Text = txtAmount.Text = txtDate.Text = "";
        ShowMode();
    }

    protected void btnCancel_Click(object sender, EventArgs e) { ClearForm(); }

    protected void gvRequests_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
        SupplyRequest r = AppData.Requests.Find(x => x.Id == id && x.FarmerId == Auth.Current.Id);
        if (r == null) return;
        if (e.CommandName == "EditReq")
        {
            EditId = r.Id;
            txtItem.Text = r.Item;
            txtAmount.Text = r.Amount.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            ddlUnit.ClearSelection();
            if (ddlUnit.Items.FindByValue(r.Unit ?? "") != null) ddlUnit.SelectedValue = r.Unit;
            ddlDistrict.ClearSelection();
            if (ddlDistrict.Items.FindByValue(r.District ?? "") != null) ddlDistrict.SelectedValue = r.District;
            txtLocation.Text = r.Location;
            txtDate.Text = r.Date;
            ShowMode();
        }
        else if (e.CommandName == "DeleteReq")
        {
            string error;
            if (AppData.Current.DeleteRequest(Auth.Current, r.Id, out error)) { if (EditId == r.Id) ClearForm(); Master.Notify("Supply request " + r.RefNo + " was deleted."); }
            else Master.Notify(error);
            BindGrid();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        DateTime wanted;
        if (DateTime.TryParse(txtDate.Text, out wanted) && wanted.Date < DateTime.Today)
        {
            lblError.Text = "The preferred date cannot be in the past.";
            return;
        }
        if (EditId != 0)
        {
            string editError;
            if (!AppData.Current.UpdateRequest(Auth.Current, EditId, txtItem.Text.Trim(), Convert.ToDecimal(txtAmount.Text, System.Globalization.CultureInfo.InvariantCulture), ddlUnit.SelectedValue, txtLocation.Text.Trim(), ddlDistrict.SelectedValue, txtDate.Text, out editError))
            {
                lblError.Text = editError;
                return;
            }
            ClearForm();
            BindGrid();
            Master.Notify("Supply request updated.");
            return;
        }
        SupplyRequest r = AppData.Current.SubmitRequest(Auth.Current, txtItem.Text.Trim(), Convert.ToDecimal(txtAmount.Text, System.Globalization.CultureInfo.InvariantCulture), ddlUnit.SelectedValue, txtLocation.Text.Trim(), ddlDistrict.SelectedValue, txtDate.Text);
        BindGrid();
        Master.Notify("Request " + r.RefNo + " submitted. Suppliers in " + r.District + " were notified and " + r.AssignedOfficer + " will verify it.");
        txtItem.Text = txtAmount.Text = txtDate.Text = "";
    }
}