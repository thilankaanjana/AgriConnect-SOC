using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Admin_Users : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ddlDistrict.DataSource = Lookups.Districts;
        ddlDistrict.DataBind();
        BindGrid();
    }

    void BindGrid()
    {
        string filter = ddlFilter.SelectedValue;
        List<AppUser> list = AppData.Users.FindAll(u => filter == "" || (filter == "Pending" ? u.Status == "Pending" : u.Role == filter));
        list.Sort((x, y) => (x.Status == "Pending" ? 0 : 1).CompareTo(y.Status == "Pending" ? 0 : 1));   // waiting accounts first
        gvUsers.DataSource = list;
        gvUsers.DataBind();
        int waiting = AppData.Users.FindAll(u => u.Status == "Pending").Count;
        litPending.Text = waiting == 0 ? "" : "<div class=\"notice\"><b>" + waiting + "</b> account(s) waiting for approval. Choose <i>Approve</i> or <i>Reject</i> in the list below.</div>";
    }

    protected void ddlFilter_Changed(object sender, EventArgs e) { BindGrid(); }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string role = ddlRole.SelectedValue == "Admin" ? "Admin" : "Officer";
        AppUser u = new AppUser { Name = txtName.Text.Trim(), Email = txtEmail.Text.Trim(), Contact = txtContact.Text.Trim(), Address = "", District = ddlDistrict.SelectedValue, Password = txtPassword.Text, Role = role, Status = "Active" };
        string error;
        if (!AppData.Current.CreateUser(Auth.Current, u, out error)) { lblError.Text = error; return; }
        BindGrid();
        Master.Notify(Auth.RoleLabel(role) + " account created for " + u.Name + ".");
        txtName.Text = txtEmail.Text = txtContact.Text = "";
    }

    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Toggle" && e.CommandName != "Approve" && e.CommandName != "Reject") return;
        AppUser u = AppData.Users.Find(x => x.Id == int.Parse(e.CommandArgument.ToString()));
        if (u == null) return;
        if (u.Id == Auth.Current.Id) { Master.Notify("You cannot change your own account."); return; }
        string status;
        if (e.CommandName == "Approve") status = "Active";
        else if (e.CommandName == "Reject") status = "Rejected";
        else status = u.Status == "Active" ? "Suspended" : "Active";
        AppData.Current.SetUserStatus(Auth.Current, u.Id, status);
        BindGrid();
        Master.Notify(e.CommandName == "Approve" ? u.Name + " was approved and can log in now." : e.CommandName == "Reject" ? "Registration of " + u.Name + " was rejected." : u.Name + " is now " + status.ToLower() + ".");
    }
}