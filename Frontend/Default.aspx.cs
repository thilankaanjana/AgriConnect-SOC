using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

public partial class _Default : System.Web.UI.Page
{
    string q = "", sort = "newest", cat = "", pr = "", tag = "";

    static readonly string[][] PriceOptions = {
        new[] { "lt200", "Under Rs 200" }, new[] { "200-400", "Rs 200 - 400" },
        new[] { "400-600", "Rs 400 - 600" }, new[] { "gt600", "Over Rs 600" } };
    static readonly string[] Cats = { "Vegetables", "Fruits", "Grains" };
    static readonly string[] Tags = { "Organic", "Pesticide-free" };

    static bool InRange(decimal p, string pr)
    {
        switch (pr)
        {
            case "lt200": return p < 200;
            case "200-400": return p >= 200 && p < 400;
            case "400-600": return p >= 400 && p < 600;
            case "gt600": return p >= 600;
            default: return true;
        }
    }

    // Builds a link to this page with one filter changed (value "" removes the filter).
    string Url(string key, string value)
    {
        string nq = q, ns = sort, nc = cat, np = pr, nt = tag;
        switch (key)
        {
            case "q": nq = value; break;
            case "sort": ns = value; break;
            case "cat": nc = value; break;
            case "pr": np = value; break;
            case "tag": nt = value; break;
        }
        return ResolveUrl("~/Default.aspx") + "?q=" + Server.UrlEncode(nq) + "&sort=" + Server.UrlEncode(ns) + "&cat=" + Server.UrlEncode(nc)
            + "&pr=" + Server.UrlEncode(np) + "&tag=" + Server.UrlEncode(nt) + "#market";
    }

    string Opt(string key, string current, string value, string label, bool radio, int count)
    {
        bool on = current == value;
        return "<a class=\"fopt" + (on ? " on" : "") + "\" href=\"" + Url(key, on ? "" : value) + "\"><span class=\"box" + (radio ? " r" : "") + "\"></span><span class=\"ft\">"
            + HttpUtility.HtmlEncode(label) + "</span>" + (count >= 0 ? "<span class=\"fc\">" + count + "</span>" : "") + "</a>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        q = (Request.QueryString["q"] ?? "").Trim();
        sort = Request.QueryString["sort"] ?? "newest";
        cat = Request.QueryString["cat"] ?? "";
        pr = Request.QueryString["pr"] ?? "";
        tag = Request.QueryString["tag"] ?? "";
        string ql = q.ToLower();

        phJoin.Visible = Auth.Current == null;

        List<Crop> all = AppData.Crops.FindAll(x => x.Status == "Available");
        litLive.Text = all.Count.ToString();
        litSListings.Text = all.Count.ToString();
        litSFarmers.Text = AppData.Current.CountActiveUsers("Farmer").ToString();
        litSSuppliers.Text = AppData.Current.CountActiveUsers("Supplier").ToString();

        List<Crop> list = all.FindAll(c =>
            (ql == "" || c.Name.ToLower().Contains(ql) || c.Farmer.ToLower().Contains(ql) || c.Location.ToLower().Contains(ql)) &&
            (cat == "" || c.Category == cat) && (tag == "" || c.Tag == tag) && InRange(c.Price, pr));
        if (sort == "priceasc") list.Sort((a, b) => a.Price.CompareTo(b.Price));
        else if (sort == "pricedesc") list.Sort((a, b) => b.Price.CompareTo(a.Price));
        else list.Sort((a, b) => b.Id.CompareTo(a.Id));   // newest first

        rptCrops.DataSource = list;
        rptCrops.DataBind();
        pnlEmpty.Visible = list.Count == 0;
        litCount.Text = list.Count + " of " + all.Count + " products";

        // sidebar filters
        StringBuilder f = new StringBuilder();
        f.Append("<div class=\"fgroup\"><h4>Product categories</h4>");
        foreach (string c in Cats) f.Append(Opt("cat", cat, c, c, false, all.FindAll(x => x.Category == c).Count));
        f.Append("</div><div class=\"fgroup\"><h4>Price per kg</h4>");
        foreach (string[] o in PriceOptions) f.Append(Opt("pr", pr, o[0], o[1], true, -1));
        f.Append("</div><div class=\"fgroup\"><h4>Quality</h4>");
        foreach (string t in Tags) f.Append(Opt("tag", tag, t, t, false, all.FindAll(x => x.Tag == t).Count));
        f.Append("</div>");
        litFilters.Text = f.ToString();

        bool any = q != "" || cat != "" || pr != "" || tag != "";
        litClear.Text = any ? "<a class=\"clear\" href=\"" + ResolveUrl("~/Default.aspx") + "#market\">Clear all</a>" : "";

        // active filter chips
        StringBuilder chips = new StringBuilder();
        if (q != "") chips.Append("<a class=\"achip\" href=\"" + Url("q", "") + "\">&ldquo;" + HttpUtility.HtmlEncode(q) + "&rdquo; <i>&times;</i></a>");
        if (cat != "") chips.Append("<a class=\"achip\" href=\"" + Url("cat", "") + "\">" + HttpUtility.HtmlEncode(cat) + " <i>&times;</i></a>");
        if (pr != "") { foreach (string[] o in PriceOptions) if (o[0] == pr) chips.Append("<a class=\"achip\" href=\"" + Url("pr", "") + "\">" + o[1] + " <i>&times;</i></a>"); }
        if (tag != "") chips.Append("<a class=\"achip\" href=\"" + Url("tag", "") + "\">" + HttpUtility.HtmlEncode(tag) + " <i>&times;</i></a>");
        litActive.Text = chips.ToString();

        // category tabs
        StringBuilder t2 = new StringBuilder();
        t2.Append("<a class=\"tab" + (cat == "" ? " on" : "") + "\" href=\"" + Url("cat", "") + "\">All Products</a>");
        foreach (string c in Cats) t2.Append("<a class=\"tab" + (cat == c ? " on" : "") + "\" href=\"" + Url("cat", c) + "\">" + c + "</a>");
        litTabs.Text = t2.ToString();

        List<Guideline> guides = new List<Guideline>(AppData.Guidelines);
        guides.Sort((x, y) => y.Id.CompareTo(x.Id));
        if (guides.Count > 4) guides = guides.GetRange(0, 4);
        rptGuides.DataSource = guides;
        rptGuides.DataBind();
    }
}