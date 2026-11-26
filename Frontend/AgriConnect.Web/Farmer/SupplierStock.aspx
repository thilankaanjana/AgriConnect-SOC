<%@ Page Title="Supplier Stock" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SupplierStock.aspx.cs" Inherits="Farmer_SupplierStock" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Supplier Stock</h1><p>See what the suppliers have in stock, where they are and the price per unit. Pick an item to start a supply request.</p></div></div>
    <div class="panel">
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtSearch" Text="Search item or supplier" />
                <asp:TextBox ID="txtSearch" runat="server" placeholder="e.g. fertilizer" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlDistrict" Text="District" />
                <asp:DropDownList ID="ddlDistrict" runat="server" />
            </div>
        </div>
        <asp:Button ID="btnFilter" runat="server" Text="Search" CssClass="btn" CausesValidation="false" OnClick="btnFilter_Click" />
        <asp:Label ID="lblCount" runat="server" CssClass="muted" style="margin-left:12px" />
    </div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No supplier stock matches your search">
            <Columns>
                <asp:BoundField HeaderText="Item" DataField="Item" />
                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                <asp:BoundField HeaderText="District" DataField="District" />
                <asp:BoundField HeaderText="Available" DataField="Available" />
                <asp:BoundField HeaderText="Price per unit" DataField="PriceText" />
                <asp:TemplateField HeaderText="Stock"><ItemTemplate><%# UIHelper.Tag(Eval("Level")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Contact" DataField="Contact" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate><a class="btn alt small" href='<%# ResolveUrl((string)Eval("RequestUrl")) %>'>Request this</a></ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>