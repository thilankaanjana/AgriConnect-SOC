using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web.Configuration;

/// <summary>
/// Sends the OTP text message to the user's mobile number.
/// Provider is chosen in Web.config (Sms:Provider): Demo | NotifyLk | Twilio.
/// In "Demo" mode nothing is sent (the page shows the code in a pop-up so you can test).
/// When the real backend is ready, the backend can send the SMS instead and this class can be removed.
/// </summary>
public static class SmsService
{
    static string Cfg(string key) { return WebConfigurationManager.AppSettings[key] ?? ""; }

    public static string Provider
    {
        get { string p = Cfg("Sms:Provider").Trim(); return p == "" ? "Demo" : p; }
    }

    public static bool IsDemo { get { return string.Equals(Provider, "Demo", StringComparison.OrdinalIgnoreCase); } }

    /// <summary>0771234567 / +94771234567 / 771234567  ->  94771234567</summary>
    public static string Normalize(string phone)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char c in phone ?? "") if (char.IsDigit(c)) sb.Append(c);
        string d = sb.ToString();
        if (d.StartsWith("94") && d.Length == 11) return d;
        if (d.StartsWith("0") && d.Length == 10) return "94" + d.Substring(1);
        if (d.Length == 9) return "94" + d;
        return d;
    }

    /// <summary>94771234567 -> 077***4567 (shown on the screen instead of the full number)</summary>
    public static string Mask(string phone)
    {
        string n = Normalize(phone);
        if (n.Length != 11) return "your mobile number";
        string national = "0" + n.Substring(2);
        return national.Substring(0, 3) + "•••" + national.Substring(6);
    }

    public static bool Send(string phone, string message, out string error)
    {
        error = null;
        if (IsDemo) return true;

        string to = Normalize(phone);
        if (to.Length != 11) { error = "The mobile number is not valid."; return false; }
        if (string.Equals(Provider, "NotifyLk", StringComparison.OrdinalIgnoreCase) &&
            (Cfg("Sms:NotifyLk:UserId") == "" || Cfg("Sms:NotifyLk:ApiKey") == "" || Cfg("Sms:NotifyLk:SenderId") == ""))
        { error = "SMS is not set up yet: fill Sms:NotifyLk:UserId, ApiKey and SenderId in Web.config."; return false; }
        if (string.Equals(Provider, "Twilio", StringComparison.OrdinalIgnoreCase) &&
            (Cfg("Sms:Twilio:AccountSid") == "" || Cfg("Sms:Twilio:AuthToken") == "" || Cfg("Sms:Twilio:From") == ""))
        { error = "SMS is not set up yet: fill Sms:Twilio:AccountSid, AuthToken and From in Web.config."; return false; }
        try
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            if (string.Equals(Provider, "NotifyLk", StringComparison.OrdinalIgnoreCase))
            {
                string url = "https://app.notify.lk/api/v1/send"
                    + "?user_id=" + Uri.EscapeDataString(Cfg("Sms:NotifyLk:UserId"))
                    + "&api_key=" + Uri.EscapeDataString(Cfg("Sms:NotifyLk:ApiKey"))
                    + "&sender_id=" + Uri.EscapeDataString(Cfg("Sms:NotifyLk:SenderId"))
                    + "&to=" + to
                    + "&message=" + Uri.EscapeDataString(message);
                using (WebClient wc = new WebClient())
                {
                    string reply = wc.DownloadString(url);
                    if (reply.IndexOf("success", StringComparison.OrdinalIgnoreCase) < 0) { error = "SMS gateway replied: " + reply; return false; }
                    return true;
                }
            }

            if (string.Equals(Provider, "Twilio", StringComparison.OrdinalIgnoreCase))
            {
                string sid = Cfg("Sms:Twilio:AccountSid");
                string token = Cfg("Sms:Twilio:AuthToken");
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(sid + ":" + token));
                    NameValueCollection form = new NameValueCollection();
                    form["To"] = "+" + to;
                    form["From"] = Cfg("Sms:Twilio:From");
                    form["Body"] = message;
                    wc.UploadValues("https://api.twilio.com/2010-04-01/Accounts/" + sid + "/Messages.json", "POST", form);
                    return true;
                }
            }

            error = "Unknown SMS provider '" + Provider + "'. Check Sms:Provider in Web.config.";
            return false;
        }
        catch (WebException ex)
        {
            string body = "";
            try { if (ex.Response != null) using (System.IO.StreamReader r = new System.IO.StreamReader(ex.Response.GetResponseStream())) body = r.ReadToEnd(); } catch { }
            error = ex.Message + (body == "" ? "" : " - " + (body.Length > 200 ? body.Substring(0, 200) : body));
            return false;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}