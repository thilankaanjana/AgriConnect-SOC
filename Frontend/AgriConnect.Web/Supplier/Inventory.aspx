<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Supplier_Inventory" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Inventory Management</h1><p>Add stock items, update quantities and prices, or remove items.</p></div></div>
    <div class="panel">
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtItem" Text="Item name" />
                <asp:TextBox ID="txtItem" runat="server" placeholder="e.g. Urea fertilizer 50kg" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtItem" ErrorMessage="Item name is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtQty" Text="Stock quantity" />
                <asp:TextBox ID="txtQty" runat="server" placeholder="e.g. 100" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQty" ErrorMessage="Quantity is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtQty" Type="Integer" MinimumValue="0" MaximumValue="999999999" ErrorMessage="Enter a whole number of 0 or more" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlUnit" Text="Unit" />
                <asp:DropDownList ID="ddlUnit" runat="server">
                    <asp:ListItem>kg</asp:ListItem><asp:ListItem>units</asp:ListItem><asp:ListItem>bags</asp:ListItem><asp:ListItem>litres</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtPrice" Text="Price per unit (Rs.)" />
                <asp:TextBox ID="txtPrice" runat="server" placeholder="e.g. 8500.00" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrice" ErrorMessage="Price is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtPrice" Type="Double" MinimumValue="1" MaximumValue="999999999" ErrorMessage="Enter a price of 1 or more" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <asp:Button ID="btnAdd" runat="server" Text="Add Inventory Item" CssClass="btn" OnClick="btnAdd_Click" />
    </div>
    <h2>Current Inventory</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" DataKeyNames="Id" EmptyDataText="No inventory items yet"
            OnRowEditing="gvInventory_RowEditing" OnRowCancelingEdit="gvInventory_RowCancelingEdit" OnRowUpdating="gvInventory_RowUpdating" OnRowDeleting="gvInventory_RowDeleting">
            <Columns>
                <asp:BoundField HeaderText="Item" DataField="Item" ReadOnly="true" />
                <asp:BoundField HeaderText="Quantity" DataField="Qty" />
                <asp:BoundField HeaderText="Unit" DataField="Unit" ReadOnly="true" />
                <asp:TemplateField HeaderText="Stock level"><ItemTemplate><%# UIHelper.Tag(Eval("Level")) %></ItemTemplate><EditItemTemplate><%# UIHelper.Tag(Eval("Level")) %></EditItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Price per unit (Rs.)" DataField="Price" DataFormatString="{0:N0}" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ButtonType="Link" EditText="Edit" UpdateText="Save" CancelText="Cancel" DeleteText="Delete" />
            </Columns>
        </asp:GridView>
    </div>
    <h2>Stock history</h2>
    <p class="muted">Every change of your stock: farmer orders reduce it automatically, manual updates are recorded too.</p>
    <div class="panel tablewrap">
        <asp:GridView ID="gvMoves" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No stock changes yet">
            <Columns>
                <asp:TemplateField HeaderText="Date"><ItemTemplate><%# UIHelper.DateTimeText(Eval("Created")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Item" DataField="Item" />
                <asp:TemplateField HeaderText="Change"><ItemTemplate><b style='color:<%# (int)Eval("Change") < 0 ? "#c8402f" : "#2f7d4a" %>'><%# ((int)Eval("Change") > 0 ? "+" : "") + Eval("Change") %> <%# Eval("Unit") %></b></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Stock after"><ItemTemplate><%# Eval("Balance") %> <%# Eval("Unit") %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Reason" DataField="Note" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>