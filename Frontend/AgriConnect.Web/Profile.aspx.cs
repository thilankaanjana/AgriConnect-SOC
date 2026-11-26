using System;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>Profile management for every role (Farmer, Supplier, Agriculture Officer, Administrator).</summary>
public partial class UserProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        ddlDistrict.DataSource = Lookups.Districts;
        ddlDistrict.DataBind();
        ShowProfile();
    }

    void ShowProfile()
    {
        AppUser me = Auth.Current;
        litAccount.Text = "<b>" + HttpUtility.HtmlEncode(Auth.RoleLabel(me.Role)) + "</b> &middot; status: " + UIHelper.Tag(me.Status);
        txtName.Text = me.Name;
        txtEmail.Text = me.Email;
        txtContact.Text = me.Contact;
        txtAddress.Text = me.Address;
        ddlDistrict.ClearSelection();
        ListItem li = ddlDistrict.Items.FindByValue(me.District ?? "");
        if (li != null) li.Selected = true;
    }

    protected void btnSaveProfile_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string error;
        if (!AppData.Current.UpdateProfile(Auth.Current, txtName.Text.Trim(), txtContact.Text.Trim(), txtAddress.Text.Trim(), ddlDistrict.SelectedValue, out error))
        {
            lblProfileError.Text = error;
            return;
        }
        ShowProfile();
        Master.Notify("Your profile was updated.");
    }

    protected void btnChangePwd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        string error;
        if (!AppData.Current.ChangePassword(Auth.Current, txtCurrent.Text, txtNew.Text, out error))
        {
            lblPwdError.Text = error;
            return;
        }
        Master.Notify("Your password was changed. Use the new password the next time you log in.");
    }
}