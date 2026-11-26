<%@ Page Title="My Quotations" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MyQuotations.aspx.cs" Inherits="Farmer_MyQuotations" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Supplier Quotations</h1><p>Compare quotations approved by the agriculture officer, select a supplier and confirm your order.</p></div></div>
    <asp:Panel ID="pnlWaiting" runat="server" Visible="false" CssClass="notice"><asp:Literal ID="litWaiting" runat="server" /></asp:Panel>
    <div class="panel tablewrap">
        <asp:GridView ID="gvQuotes" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No verified quotations yet" OnRowCommand="gvQuotes_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Request" DataField="RequestRef" />
                <asp:BoundField HeaderText="Item" DataField="Item" />
                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                <asp:TemplateField HeaderText="Price"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %> <%# (bool)Eval("IsLowest") ? "<span class=\"tag\">Lowest</span>" : "" %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Stock" DataField="Stock" />
                <asp:BoundField HeaderText="Delivery" DataField="Delivery" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" Text="Confirm Order" CssClass="btn small" CommandName="Confirm" CommandArgument='<%# Eval("Id") %>' Visible='<%# (bool)Eval("CanConfirm") %>' OnClientClick="return confirm('Confirm this order with the selected supplier?');" />
                        <asp:Label runat="server" Text='<%# Eval("ActionText") %>' Visible='<%# !(bool)Eval("CanConfirm") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>