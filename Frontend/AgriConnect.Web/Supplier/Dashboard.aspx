<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Supplier_Dashboard" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="banner"><h1>Supplier Dashboard</h1><p>Manage stock, answer regional supply requests with quotations and update deliveries.</p></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="grid2">
        <div class="panel">
            <h2>Regional requests waiting for your quotation</h2>
            <asp:Literal ID="litOpen" runat="server" />
        </div>
        <div class="panel">
            <h2>Orders to deliver</h2>
            <asp:Literal ID="litOrders" runat="server" />
        </div>
    </div>
    <div class="panel">
        <h2>Quick Actions</h2>
        <div class="actions">
            <asp:HyperLink runat="server" NavigateUrl="~/Supplier/SupplyRequests.aspx" CssClass="btn" Text="View Supply Requests" />
            <asp:HyperLink runat="server" NavigateUrl="~/Supplier/Inventory.aspx" CssClass="btn alt" Text="Inventory" />
            <asp:HyperLink runat="server" NavigateUrl="~/Supplier/Deliveries.aspx" CssClass="btn alt" Text="Deliveries" />
        </div>
    </div>
</asp:Content>