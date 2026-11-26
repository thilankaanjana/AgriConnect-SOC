using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

public static class UIHelper
{
    static readonly Dictionary<string, string> Icons = new Dictionary<string, string>
    {
        { "search", "<circle cx=\"11\" cy=\"11\" r=\"7\"/><path d=\"m21 21-4.3-4.3\"/>" },
        { "user", "<path d=\"M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2\"/><circle cx=\"12\" cy=\"7\" r=\"4\"/>" },
        { "pin", "<path d=\"M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z\"/><circle cx=\"12\" cy=\"10\" r=\"3\"/>" },
        { "cart", "<circle cx=\"8\" cy=\"21\" r=\"1\"/><circle cx=\"19\" cy=\"21\" r=\"1\"/><path d=\"M2.05 2.05h2l2.66 12.42a2 2 0 0 0 2 1.58h9.78a2 2 0 0 0 1.95-1.57l1.65-7.43H5.12\"/>" },
        { "phone", "<path d=\"M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72c.127.96.361 1.903.7 2.81a2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0 1 22 16.92z\"/>" },
        { "x", "<path d=\"M18 6 6 18\"/><path d=\"m6 6 12 12\"/>" },
        { "eye", "<path d=\"M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12Z\"/><circle cx=\"12\" cy=\"12\" r=\"3\"/>" },
        { "shield", "<path d=\"M20 13c0 5-3.5 7.5-7.66 8.95a1 1 0 0 1-.67-.01C7.5 20.5 4 18 4 13V6a1 1 0 0 1 1-1c2 0 4.5-1.2 6.24-2.72a1.17 1.17 0 0 1 1.52 0C14.51 3.81 17 5 19 5a1 1 0 0 1 1 1z\"/><path d=\"m9 12 2 2 4-4\"/>" },
        { "filter", "<line x1=\"21\" x2=\"14\" y1=\"4\" y2=\"4\"/><line x1=\"10\" x2=\"3\" y1=\"4\" y2=\"4\"/><line x1=\"21\" x2=\"12\" y1=\"12\" y2=\"12\"/><line x1=\"8\" x2=\"3\" y1=\"12\" y2=\"12\"/><line x1=\"21\" x2=\"16\" y1=\"20\" y2=\"20\"/><line x1=\"12\" x2=\"3\" y1=\"20\" y2=\"20\"/><line x1=\"14\" x2=\"14\" y1=\"2\" y2=\"6\"/><line x1=\"8\" x2=\"8\" y1=\"10\" y2=\"14\"/><line x1=\"16\" x2=\"16\" y1=\"18\" y2=\"22\"/>" },
        { "sprout", "<path d=\"M7 20h10\"/><path d=\"M10 20c5.5-2.5.8-6.4 3-10\"/><path d=\"M9.5 9.4c1.1.8 1.8 2.2 2.3 3.7-2 .4-3.5.4-4.8-.3-1.2-.6-2.3-1.9-3-4.2 2.8-.5 4.4 0 5.5.8z\"/><path d=\"M14.1 6a7 7 0 0 0-1.1 4c1.9-.1 3.3-.6 4.3-1.4 1-1 1.6-2.3 1.7-4.6-2.7.1-4 1-4.9 2z\"/>" }
    };

    public static string Icon(string name)
    {
        return Icon(name, 16);
    }

    public static string Icon(string name, int size)
    {
        string p;
        if (!Icons.TryGetValue(name, out p)) p = "";
        return "<svg class=\"ico\" width=\"" + size + "\" height=\"" + size + "\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">" + p + "</svg>";
    }

    public static string Tag(object status)
    {
        string s = Convert.ToString(status);
        string l = s.ToLower();
        string cls = "";
        if (l.Contains("pending") || l.Contains("low stock")) cls = " pending";
        else if (l.Contains("reject") || l.Contains("declined") || l.Contains("out of stock")) cls = " rejected";
        else if (l.Contains("closed") || l.Contains("not selected") || l.Contains("sold") || l.Contains("inactive") || l.Contains("suspend") || l.Contains("unassigned")) cls = " muted";
        return "<span class=\"tag" + cls + "\">" + HttpUtility.HtmlEncode(s) + "</span>";
    }

    public static string Money(object n)
    {
        return "Rs. " + Convert.ToDecimal(n).ToString("N0");
    }

    public static string ShortDate(object d)
    {
        return Convert.ToDateTime(d).ToString("yyyy-MM-dd");
    }

    public static string DateTimeText(object d)
    {
        return Convert.ToDateTime(d).ToString("yyyy-MM-dd HH:mm");
    }

    public static string Stat(string label, int value)
    {
        return "<div class=\"card\"><div class=\"label\">" + HttpUtility.HtmlEncode(label) +
               "</div><div class=\"value\">" + value + "</div><div class=\"muted\">Demo data</div></div>";
    }

    public static string StatText(string label, string value)
    {
        return "<div class=\"card\"><div class=\"label\">" + HttpUtility.HtmlEncode(label) +
               "</div><div class=\"value\">" + HttpUtility.HtmlEncode(value) + "</div><div class=\"muted\">Demo data</div></div>";
    }

    /// <summary>A row of workflow steps; completed steps are highlighted.</summary>
    public static string Stepper(string[] labels, bool[] done)
    {
        StringBuilder sb = new StringBuilder("<div class=\"stepper\">");
        for (int i = 0; i < labels.Length; i++)
            sb.Append("<span class=\"st" + (done[i] ? " done" : "") + "\">" + HttpUtility.HtmlEncode(labels[i]) + "</span>");
        sb.Append("</div>");
        return sb.ToString();
    }

    /// <summary>One horizontal bar of a simple analytics chart.</summary>
    public static string Bar(string label, int value, int max)
    {
        int pct = max <= 0 ? 0 : value * 100 / max;
        return "<div class=\"bar-row\"><span class=\"bl\">" + HttpUtility.HtmlEncode(label) + "</span><div class=\"bar\"><i style=\"width:" + pct + "%\"></i></div><b>" + value + "</b></div>";
    }

    /// <summary>First number found in a text such as "50 kg" (0 when there is none).</summary>
    public static decimal QtyNumber(string text)
    {
        Match m = Regex.Match(text ?? "", @"\d+(\.\d+)?");
        decimal v;
        return m.Success && decimal.TryParse(m.Value, out v) ? v : 0;
    }
}