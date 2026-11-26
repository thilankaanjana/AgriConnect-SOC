using System;
using System.Web;

/// <summary>
/// Who is signed in (kept in the Session after login + OTP). With the real backend the login API also
/// returns a token, which HttpApi keeps in Session["ApiToken"].
/// </summary>
public static class Auth
{
    public static AppUser Current
    {
        get { return HttpContext.Current.Session["User"] as AppUser; }
    }

    public static void SignIn(AppUser u)
    {
        HttpContext.Current.Session["User"] = u;
        AppData.Current.OnSignedIn(u);
    }

    public static void SignOut() { HttpContext.Current.Session.Abandon(); }

    public static string HomeUrl(string role)
    {
        switch (role)
        {
            case "Supplier": return "~/Supplier/Dashboard.aspx";
            case "Officer": return "~/Officer/Dashboard.aspx";
            case "Admin": return "~/Admin/Dashboard.aspx";
            default: return "~/Farmer/Dashboard.aspx";
        }
    }

    public static string RoleLabel(object role)
    {
        string r = Convert.ToString(role);
        if (r == "Officer") return "Agriculture Officer";
        if (r == "Admin") return "Administrator";
        return r;
    }
}