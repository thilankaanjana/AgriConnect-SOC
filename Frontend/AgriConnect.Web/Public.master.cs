using System;
using System.Security.Cryptography;
using System.Web;

public partial class PublicMaster : System.Web.UI.MasterPage
{
    const int OtpMinutes = 5;
    const int MaxTries = 5;
    const int ResendSeconds = 30;

    protected void Page_Load(object sender, EventArgs e)
    {
        AppUser u = Auth.Current;
        phGuest.Visible = (u == null);
        phUser.Visible = (u != null);
        if (u != null)
        {
            litUser.Text = HttpUtility.HtmlEncode(u.Name + " (" + Auth.RoleLabel(u.Role) + ")");
            lnkDash.NavigateUrl = Auth.HomeUrl(u.Role);
        }

        if (!IsPostBack)
        {
            ddlRegDistrict.Items.Add(new System.Web.UI.WebControls.ListItem("Select district", ""));
            foreach (string d in Lookups.Districts) ddlRegDistrict.Items.Add(d);
            txtSearch.Text = Request.QueryString["q"] ?? "";
            string sort = Request.QueryString["sort"];
            if (sort == "priceasc" || sort == "pricedesc") ddlSort.SelectedValue = sort;
        }
    }

    // ---------- search / sort ----------
    void GoSearch()
    {
        Response.Redirect("~/Default.aspx?q=" + Server.UrlEncode(txtSearch.Text.Trim()) + "&sort=" + ddlSort.SelectedValue
            + "&cat=" + Server.UrlEncode(Request.QueryString["cat"] ?? "") + "&pr=" + Server.UrlEncode(Request.QueryString["pr"] ?? "")
            + "&tag=" + Server.UrlEncode(Request.QueryString["tag"] ?? "") + "#market");
    }
    protected void btnSearch_Click(object sender, EventArgs e) { GoSearch(); }
    protected void ddlSort_Changed(object sender, EventArgs e) { GoSearch(); }

    // ---------- modal helpers ----------
    void Open(string name)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "open_" + name, "openModal('" + name + "');", true);
    }

    static string NewCode()
    {
        byte[] b = new byte[4];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) { rng.GetBytes(b); }
        uint n = BitConverter.ToUInt32(b, 0);
        return (100000 + (n % 900000)).ToString();
    }

    // Starts OTP verification. isNew = true for a new registration (account is created only after the code is verified).
    void StartOtp(AppUser u, bool isNew)
    {
        Session["PendingUser"] = u;
        Session["PendingIsNew"] = isNew;
        SendOtp(u, false);
    }

    // Generates a code and sends it by SMS to the user's own mobile number.
    void SendOtp(AppUser u, bool isResend)
    {
        string code = NewCode();
        Session["Otp"] = code;
        Session["OtpExpires"] = DateTime.Now.AddMinutes(OtpMinutes);
        Session["OtpTries"] = 0;
        Session["OtpSentAt"] = DateTime.Now;

        string masked = SmsService.Mask(u.Contact);
        lblOtpPhone.Text = "Enter the 6-digit code sent to " + masked + ".";

        string error;
        bool sent = SmsService.Send(u.Contact, "Your AgriConnect verification code is " + code + ". It expires in " + OtpMinutes + " minutes.", out error);
        if (!sent)
        {
            lblOtpError.Text = "Could not send the SMS: " + error + " Click Resend code to try again.";
            Page.ClientScript.RegisterStartupScript(GetType(), "open_otp", "openModal('otp');", true);
            return;
        }

        lblOtpInfo.Text = (isResend ? "A new code was sent to " : "A code was sent to ") + masked + ". Any older code no longer works.";
        string script = "openModal('otp');startResendTimer(" + ResendSeconds + ");";
        if (SmsService.IsDemo)   // demo mode: show the code on screen because no real SMS is sent
            script += "showToast(" + HttpUtility.JavaScriptStringEncode("Demo mode - OTP for " + masked + ": " + code, true) + ");";
        Page.ClientScript.RegisterStartupScript(GetType(), "open_otp", script, true);
    }

    // ---------- login ----------
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) { Open("login"); return; }
        string error;
        AppUser u = AppData.Current.Login(txtLoginPhone.Text.Trim(), txtLoginPass.Text, out error);   // the API returns the user's role
        if (u == null) { lblLoginError.Text = error; Open("login"); return; }
        StartOtp(u, false);
    }

    // ---------- register ----------
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) { Open("register"); return; }
        if (txtRegPass.Text != txtRegPass2.Text) { lblRegError.Text = "The two passwords do not match."; Open("register"); return; }
        string email = txtRegEmail.Text.Trim();
        if (AppData.Current.EmailExists(email)) { lblRegError.Text = "An account with this email already exists."; Open("register"); return; }
        if (DemoData.FindUserByPhone(txtRegContact.Text) != null) { lblRegError.Text = "An account with this mobile number already exists."; Open("register"); return; }
        AppUser u = new AppUser
        {
            Name = txtRegName.Text.Trim(),
            Email = email,
            Contact = txtRegContact.Text.Trim(),
            Address = txtRegAddress.Text.Trim(),
            District = ddlRegDistrict.SelectedValue,
            Password = txtRegPass.Text,
            Role = hdnRegRole.Value == "Supplier" ? "Supplier" : "Farmer",
            Status = "Pending"
        };
        StartOtp(u, true);   // the account is saved only after the phone number is verified
    }

    // ---------- verify / resend ----------
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        AppUser u = Session["PendingUser"] as AppUser;
        if (u == null || Session["Otp"] == null) { lblLoginError.Text = "Session expired. Please log in again."; Open("login"); return; }

        string code = Convert.ToString(Session["Otp"]);
        DateTime expires = (DateTime)Session["OtpExpires"];
        int tries = (int)Session["OtpTries"];

        if (DateTime.Now > expires) { lblOtpError.Text = "This code has expired. Click Resend code."; Open("otp"); return; }
        if (tries >= MaxTries) { lblOtpError.Text = "Too many wrong attempts. Click Resend code to get a new one."; Open("otp"); return; }
        if (hdnOtp.Value != code) { Session["OtpTries"] = tries + 1; lblOtpError.Text = "Incorrect code. Please try again."; Open("otp"); return; }

        bool isNew = Session["PendingIsNew"] != null && (bool)Session["PendingIsNew"];
        if (isNew)
        {
            string createError;
            if (!AppData.Current.CreateUser(null, u, out createError)) { lblRegError.Text = createError; Open("register"); return; }
        }

        Session.Remove("PendingUser"); Session.Remove("PendingIsNew"); Session.Remove("Otp");
        Session.Remove("OtpExpires"); Session.Remove("OtpTries"); Session.Remove("OtpSentAt");

        if (isNew)
        {
            // new accounts start as "Pending": an administrator must approve them before the first login
            lblLoginInfo.Text = "Registration successful. Your account is waiting for administrator approval - you can log in once it is approved.";
            Open("login");
            return;
        }

        Auth.SignIn(u);
        Response.Redirect(Auth.HomeUrl(u.Role));   // Farmer / Supplier / AG Officer / Admin dashboard, detected from the account
    }

    protected void btnResend_Click(object sender, EventArgs e)
    {
        AppUser u = Session["PendingUser"] as AppUser;
        if (u == null) { lblLoginError.Text = "Session expired. Please log in again."; Open("login"); return; }
        if (Session["OtpSentAt"] != null && (DateTime.Now - (DateTime)Session["OtpSentAt"]).TotalSeconds < ResendSeconds)
        {
            int left = ResendSeconds - (int)(DateTime.Now - (DateTime)Session["OtpSentAt"]).TotalSeconds;
            lblOtpError.Text = "Please wait " + left + " seconds before asking for a new code.";
            Page.ClientScript.RegisterStartupScript(GetType(), "open_otp", "openModal('otp');startResendTimer(" + left + ");", true);
            return;
        }
        SendOtp(u, true);
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Auth.SignOut();
        Response.Redirect("~/Default.aspx");
    }
}