using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Supplier_Inventory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { if (!IsPostBack) BindGrid(); }

    void BindGrid()
    {
        int me = Auth.Current.Id;
        gvInventory.DataSource = AppData.Inventory.FindAll(x => x.SupplierId == me);
        gvInventory.DataBind();
        List<StockMovement> moves = new List<StockMovement>(AppData.Current.GetStockMovements(Auth.Current));
        moves.Sort((a, b) => b.Created.CompareTo(a.Created));
        gvMoves.DataSource = moves;
        gvMoves.DataBind();
    }

    InventoryItem ItemAt(int rowIndex)
    {
        int id = (int)gvInventory.DataKeys[rowIndex].Value;
        return AppData.Inventory.Find(x => x.Id == id && x.SupplierId == Auth.Current.Id);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        AppData.Current.AddInventory(Auth.Current, new InventoryItem { Item = txtItem.Text.Trim(), Qty = int.Parse(txtQty.Text), Unit = ddlUnit.SelectedValue, Price = decimal.Parse(txtPrice.Text) });
        BindGrid();
        Master.Notify("Inventory item added.");
        txtItem.Text = txtQty.Text = txtPrice.Text = "";
    }

    protected void gvInventory_RowEditing(object sender, GridViewEditEventArgs e) { gvInventory.EditIndex = e.NewEditIndex; BindGrid(); }

    protected void gvInventory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e) { gvInventory.EditIndex = -1; BindGrid(); }

    protected void gvInventory_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        InventoryItem item = ItemAt(e.RowIndex);
        int qty; decimal price;
        if (item == null || !int.TryParse(Convert.ToString(e.NewValues["Qty"]), out qty) || qty < 0 || !decimal.TryParse(Convert.ToString(e.NewValues["Price"]), out price) || price < 1)
        {
            Master.Notify("Enter a whole quantity of 0 or more and a price of 1 or more.");
            return;
        }
        AppData.Current.UpdateInventory(Auth.Current, item.Id, qty, price);
        gvInventory.EditIndex = -1;
        BindGrid();
        Master.Notify(item.Item + " updated.");
    }

    protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        InventoryItem item = ItemAt(e.RowIndex);
        if (item != null)
        {
            AppData.Current.DeleteInventory(Auth.Current, item.Id);
        }
        gvInventory.EditIndex = -1;
        BindGrid();
    }
}