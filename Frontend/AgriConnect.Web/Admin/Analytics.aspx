<%@ Page Title="Analytics" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="Admin_Analytics" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Platform Analytics</h1><p>Summary of users, the supply workflow, deliveries and the marketplace.</p></div></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="grid2">
        <div class="panel"><h2>Users by role</h2><asp:Literal ID="litRoles" runat="server" /></div>
        <div class="panel"><h2>Supply requests by status</h2><asp:Literal ID="litRequests" runat="server" /></div>
        <div class="panel"><h2>Quotations by status</h2><asp:Literal ID="litQuotes" runat="server" /></div>
        <div class="panel"><h2>Deliveries</h2><asp:Literal ID="litDeliveries" runat="server" /></div>
        <div class="panel"><h2>Requests by district</h2><asp:Literal ID="litDistricts" runat="server" /></div>
        <div class="panel"><h2>Marketplace listings</h2><asp:Literal ID="litMarket" runat="server" /></div>
    </div>
</asp:Content>