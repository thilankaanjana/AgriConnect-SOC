<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MyOrders.aspx.cs" Inherits="Farmer_MyOrders" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>My Orders</h1><p>Track your confirmed supply orders until they are delivered.</p></div></div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No confirmed orders yet">
            <Columns>
                <asp:BoundField HeaderText="Order" DataField="RefNo" />
                <asp:BoundField HeaderText="Request" DataField="RequestRef" />
                <asp:BoundField HeaderText="Item" DataField="ItemText" />
                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                <asp:TemplateField HeaderText="Price"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Delivery Status"><ItemTemplate><%# UIHelper.Tag(Eval("DeliveryStatus")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Tracking"><ItemTemplate><%# Eval("Tracker") %></ItemTemplate></asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>