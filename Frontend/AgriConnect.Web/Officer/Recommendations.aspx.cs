using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Officer_Recommendations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        int me = Auth.Current.Id;
        foreach (SupplyRequest r in AppData.Requests.FindAll(x => x.AssignedOfficerId == me))
            ddlRequest.Items.Add(new ListItem(r.RefNo + " - " + r.Item + " (" + r.Farmer + ")", r.Id.ToString()));
        BindGrids();
    }

    void BindGrids()
    {
        int me = Auth.Current.Id;
        List<Recommendation> recs = AppData.Recommendations.FindAll(x => x.OfficerId == me);
        recs.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvRecs.DataSource = recs; gvRecs.DataBind();
        List<Guideline> g = new List<Guideline>(AppData.Guidelines);
        g.Sort((x, y) => y.Id.CompareTo(x.Id));
        gvGuides.DataSource = g; gvGuides.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid || ddlRequest.Items.Count == 0) return;
        AppData.Current.AddRecommendation(Auth.Current, int.Parse(ddlRequest.SelectedValue), txtText.Text.Trim());
        BindGrids();
        Master.Notify("Recommendation saved and the farmer was notified.");
        txtText.Text = "";
    }

    // 0 = new guideline, otherwise the id of the guideline being edited
    int EditId
    {
        get { return ViewState["EditId"] == null ? 0 : (int)ViewState["EditId"]; }
        set { ViewState["EditId"] = value; }
    }

    /// <summary>Officers can change only the guidelines they published themselves.</summary>
    protected bool IsMine(object author) { return Convert.ToString(author) == Auth.Current.Name; }

    void ShowMode()
    {
        bool editing = EditId != 0;
        litGuideTitle.Text = editing ? "Edit guideline" : "Publish a public guideline";
        btnPublish.Text = editing ? "Save Guideline" : "Publish Guideline";
        btnCancelGuide.Visible = editing;
    }

    protected void btnCancelGuide_Click(object sender, EventArgs e)
    {
        EditId = 0;
        txtGTitle.Text = txtGText.Text = "";
        ShowMode();
    }

    protected void gvGuides_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id;
        if (!int.TryParse(Convert.ToString(e.CommandArgument), out id)) return;
        Guideline g = AppData.Guidelines.Find(x => x.Id == id && x.Author == Auth.Current.Name);
        if (g == null) return;
        if (e.CommandName == "EditGuide")
        {
            EditId = g.Id;
            txtGTitle.Text = g.Title;
            txtGText.Text = g.Text;
            ShowMode();
        }
        else if (e.CommandName == "DeleteGuide")
        {
            string error;
            if (AppData.Current.DeleteGuideline(Auth.Current, g.Id, out error))
            {
                if (EditId == g.Id) { EditId = 0; txtGTitle.Text = txtGText.Text = ""; ShowMode(); }
                Master.Notify("Guideline deleted.");
            }
            else Master.Notify(error);
            BindGrids();
        }
    }

    protected void btnPublish_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (EditId != 0)
        {
            string editError;
            if (!AppData.Current.UpdateGuideline(Auth.Current, EditId, txtGTitle.Text.Trim(), txtGText.Text.Trim(), out editError)) { Master.Notify(editError); return; }
            EditId = 0;
            ShowMode();
            BindGrids();
            Master.Notify("Guideline updated on the public marketplace page.");
            txtGTitle.Text = txtGText.Text = "";
            return;
        }
        AppData.Current.AddGuideline(Auth.Current, txtGTitle.Text.Trim(), txtGText.Text.Trim());
        BindGrids();
        Master.Notify("Guideline published on the public marketplace page.");
        txtGTitle.Text = txtGText.Text = "";
    }
}