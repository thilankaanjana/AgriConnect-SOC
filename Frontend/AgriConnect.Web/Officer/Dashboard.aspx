<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Officer_Dashboard" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="banner"><h1>Agriculture Officer Dashboard</h1><p>Verify supply requests and supplier quotations assigned to you, monitor compliance and advise farmers.</p></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="panel">
        <h2>Waiting for your verification</h2>
        <asp:Literal ID="litPending" runat="server" />
    </div>
    <div class="panel">
        <h2>Quick Actions</h2>
        <div class="actions">
            <asp:HyperLink runat="server" NavigateUrl="~/Officer/Verifications.aspx" CssClass="btn" Text="Review Requests" />
            <asp:HyperLink runat="server" NavigateUrl="~/Officer/Compliance.aspx" CssClass="btn alt" Text="Compliance Monitoring" />
            <asp:HyperLink runat="server" NavigateUrl="~/Officer/Recommendations.aspx" CssClass="btn alt" Text="Recommendations" />
        </div>
    </div>
</asp:Content>