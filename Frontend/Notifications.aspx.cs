using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class Notifications : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int me = Auth.Current.Id;
        List<Notification> list = AppData.Notifications(me);
        list.Sort((x, y) => y.Created.CompareTo(x.Created));
        StringBuilder sb = new StringBuilder();
        foreach (Notification n in list)
            sb.Append("<div class=\"nitem" + (n.Read ? "" : " new") + "\"><span class=\"dot\"></span><div><a href=\"" + ResolveUrl(n.Link) + "\">" + HttpUtility.HtmlEncode(n.Text) + "</a><small>" + UIHelper.DateTimeText(n.Created) + "</small></div></div>");
        litList.Text = list.Count == 0 ? "<p>You have no notifications.</p>" : sb.ToString();
        AppData.Current.MarkNotificationsRead(me);   // opening the page marks them as read
    }
}