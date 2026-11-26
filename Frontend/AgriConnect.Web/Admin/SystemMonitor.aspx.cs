using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;

/// <summary>Admin: ONLINE / OFFLINE status of every service (calls GET {service}/health).</summary>
public partial class Admin_SystemMonitor : System.Web.UI.Page
{
    public class ServiceRow
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Online { get; set; }
        public long Ms { get; set; }
        public string Detail { get; set; }
        public string ResponseText { get { return Online ? Ms + " ms" : "-"; } }
    }

    // name, Web.config key, default address
    static readonly string[][] Services =
    {
        new[] { "Farmer Service", "Services:Farmer", "http://localhost:5001" },
        new[] { "Supplier Service", "Services:Supplier", "http://localhost:5002" },
        new[] { "Agriculture Officer Service", "Services:Officer", "http://localhost:5003" },
        new[] { "Admin Service", "Services:Admin", "http://localhost:5004" }
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        Check();
    }

    protected void btnRefresh_Click(object sender, EventArgs e) { Check(); }

    void Check()
    {
        List<Task<ServiceRow>> tasks = new List<Task<ServiceRow>>();
        foreach (string[] s in Services)
        {
            string name = s[0];
            string url = ConfigurationManager.AppSettings[s[1]];
            if (string.IsNullOrEmpty(url)) url = s[2];
            url = url.TrimEnd('/');
            tasks.Add(Task.Factory.StartNew(() => Ping(name, url)));   // all services are checked at the same time
        }
        Task.WaitAll(tasks.ToArray());

        List<ServiceRow> rows = new List<ServiceRow>();
        foreach (Task<ServiceRow> t in tasks) rows.Add(t.Result);
        gvServices.DataSource = rows;
        gvServices.DataBind();

        int online = rows.FindAll(r => r.Online).Count;
        litSummary.Text = UIHelper.Stat("Services Online", online) + UIHelper.Stat("Services Offline", rows.Count - online) + UIHelper.Stat("Services Total", rows.Count);
        litTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    static ServiceRow Ping(string name, string url)
    {
        ServiceRow row = new ServiceRow { Name = name, Url = url + "/health" };
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(row.Url);
            req.Method = "GET";
            req.Timeout = 3000;
            using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
            {
                row.Online = (int)res.StatusCode >= 200 && (int)res.StatusCode < 300;
                row.Detail = "HTTP " + (int)res.StatusCode;
            }
        }
        catch (WebException ex)
        {
            HttpWebResponse r = ex.Response as HttpWebResponse;
            row.Online = false;
            row.Detail = r != null ? "HTTP " + (int)r.StatusCode : (ex.Status == WebExceptionStatus.Timeout ? "No answer (timeout)" : "Not running");
        }
        catch (Exception ex)
        {
            row.Online = false;
            row.Detail = ex.Message;
        }
        sw.Stop();
        row.Ms = sw.ElapsedMilliseconds;
        return row;
    }

    protected static string StatusTag(object online)
    {
        bool on = Convert.ToBoolean(online);
        return "<span class=\"tag" + (on ? "" : " rejected") + "\">" + (on ? "ONLINE" : "OFFLINE") + "</span>";
    }
}