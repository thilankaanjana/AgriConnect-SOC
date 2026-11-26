using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Admin_ActivityLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrid(); }

    void BindGrid()
    {
        string role = ddlRole.SelectedValue;
        List<ActivityEntry> log = AppData.Activity.FindAll(a => role == "" || a.Role == role);
        log.Sort((x, y) => y.Created.CompareTo(x.Created));
        gvLog.DataSource = log;
        gvLog.DataBind();
    }

    protected void ddlRole_Changed(object sender, EventArgs e) { gvLog.PageIndex = 0; BindGrid(); }

    protected void gvLog_PageIndexChanging(object sender, GridViewPageEventArgs e) { gvLog.PageIndex = e.NewPageIndex; BindGrid(); }
}