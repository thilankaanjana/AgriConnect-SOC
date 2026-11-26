using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Farmer_CropOrders : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrid(); }

    void BindGrid()
    {
        List<CropOrder> list = new List<CropOrder>(AppData.Current.GetCropOrders(Auth.Current));
        list.Sort((x, y) => y.Created.CompareTo(x.Created));
        decimal total = 0;
        foreach (CropOrder o in list) if (o.Status == State.BuyAccepted || o.Status == State.BuyCompleted) total += o.Total;
        litStats.Text =
            UIHelper.Stat("Buyer orders", list.Count) +
            UIHelper.Stat("Waiting for you", list.FindAll(o => o.Status == State.BuyPending).Count) +
            UIHelper.Stat("Accepted", list.FindAll(o => o.Status == State.BuyAccepted).Count) +
            UIHelper.StatText("Accepted + completed value", UIHelper.Money(total));
        gvOrders.DataSource = list;
        gvOrders.DataBind();
    }

    protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != State.BuyAccepted && e.CommandName != State.BuyDeclined && e.CommandName != State.BuyCompleted) return;
        string error;
        if (AppData.Current.UpdateCropOrder(Auth.Current, int.Parse(e.CommandArgument.ToString()), e.CommandName, out error))
            Master.Notify("Order marked as " + e.CommandName.ToLower() + ". Please contact the buyer by phone to arrange the sale.");
        else
            Master.Notify(error);
        BindGrid();
    }
}