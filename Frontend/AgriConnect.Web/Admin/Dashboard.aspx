<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Admin_Dashboard" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="banner"><h1>Administrator Dashboard</h1><p>Monitor the platform, manage user accounts and follow all activity.</p></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="grid2">
        <div class="panel"><h2>Platform activity (latest)</h2><asp:Literal ID="litActivity" runat="server" />
            <div class="links"><asp:HyperLink runat="server" NavigateUrl="~/Admin/ActivityLog.aspx" CssClass="btn alt small" Text="Full audit log" /></div></div>
        <div class="panel"><h2>Quick Actions</h2>
            <div class="actions">
                <asp:HyperLink runat="server" NavigateUrl="~/Admin/Users.aspx" CssClass="btn" Text="Manage Users" />
                <asp:HyperLink runat="server" NavigateUrl="~/Admin/Transactions.aspx" CssClass="btn alt" Text="Transactions" />
                <asp:HyperLink runat="server" NavigateUrl="~/Admin/Analytics.aspx" CssClass="btn alt" Text="View Analytics" />
            </div></div>
    </div>
</asp:Content>