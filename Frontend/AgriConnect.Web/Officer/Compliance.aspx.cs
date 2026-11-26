using System;
using System.Collections.Generic;

public partial class Officer_Compliance : System.Web.UI.Page
{
    public class SupplierRow
    {
        public string Supplier { get; set; }
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Waiting { get; set; }
        public string Rate { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        int me = Auth.Current.Id;
        List<SupplyRequest> mine = AppData.Requests.FindAll(r => r.AssignedOfficerId == me);
        List<Quotation> quotes = AppData.Quotes.FindAll(q => mine.Exists(r => r.Id == q.RequestId));
        List<Review> reviews = AppData.Reviews.FindAll(r => r.OfficerId == me);
        reviews.Sort((x, y) => y.Created.CompareTo(x.Created));

        int approved = reviews.FindAll(r => r.Decision == State.Approved).Count;
        int waiting = mine.FindAll(r => r.Status == State.Pending).Count + quotes.FindAll(q => q.Status == State.Pending).Count;
        litStats.Text =
            UIHelper.Stat("Items Reviewed", reviews.Count) +
            UIHelper.Stat("Awaiting Review", waiting) +
            UIHelper.Stat("Approved", approved) +
            UIHelper.Stat("Rejected (non-compliant)", reviews.Count - approved);

        gvReviews.DataSource = reviews;
        gvReviews.DataBind();

        List<SupplierRow> rows = new List<SupplierRow>();
        foreach (Quotation q in quotes)
        {
            string name = q.Supplier;
            SupplierRow row = rows.Find(x => x.Supplier == name);
            if (row == null) { row = new SupplierRow { Supplier = name }; rows.Add(row); }
            row.Total++;
            if (q.Status == State.Approved || q.Status == State.Selected || q.Status == State.NotSelected) row.Approved++;
            else if (q.Status == State.Rejected) row.Rejected++;
            else if (q.Status == State.Pending) row.Waiting++;
        }
        foreach (SupplierRow row in rows)
        {
            int decided = row.Approved + row.Rejected;
            row.Rate = decided == 0 ? "-" : (row.Approved * 100 / decided) + "%";
        }
        gvSuppliers.DataSource = rows;
        gvSuppliers.DataBind();
    }
}