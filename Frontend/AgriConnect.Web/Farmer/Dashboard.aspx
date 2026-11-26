<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Farmer_Dashboard" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="banner"><h1>Farmer Dashboard</h1><p>Request supplies, compare verified quotations, confirm orders and sell your crops.</p></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="panel">
        <h2>My supply requests &mdash; workflow progress</h2>
        <asp:Literal ID="litFlow" runat="server" />
    </div>
    <div class="grid2">
        <div class="panel">
            <h2>Recent notifications</h2>
            <asp:Literal ID="litNotes" runat="server" />
            <div class="links"><asp:HyperLink runat="server" NavigateUrl="~/Notifications.aspx" CssClass="btn alt small" Text="All notifications" /></div>
        </div>
        <div class="panel">
            <h2>Quick Actions</h2>
            <div class="actions">
                <asp:HyperLink runat="server" NavigateUrl="~/Farmer/SupplyRequest.aspx" CssClass="btn" Text="Create Supply Request" />
                <asp:HyperLink runat="server" NavigateUrl="~/Farmer/MyQuotations.aspx" CssClass="btn alt" Text="Compare Quotations" />
                <asp:HyperLink runat="server" NavigateUrl="~/Farmer/CropListings.aspx" CssClass="btn alt" Text="Publish Crop" />
            </div>
        </div>
    </div>
</asp:Content>