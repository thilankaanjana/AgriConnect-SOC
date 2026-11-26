<%@ Page Title="System Monitor" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SystemMonitor.aspx.cs" Inherits="Admin_SystemMonitor" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>System Monitor</h1><p>Live health of every AgriConnect service. Each service is asked for its <code>/health</code> address; the page checks again every 15 seconds.</p></div>
        <div class="links"><asp:Button ID="btnRefresh" runat="server" Text="Check now" CssClass="btn alt small" CausesValidation="false" OnClick="btnRefresh_Click" /></div></div>
    <div class="cards"><asp:Literal ID="litSummary" runat="server" /></div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvServices" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No services configured">
            <Columns>
                <asp:BoundField HeaderText="Service" DataField="Name" />
                <asp:BoundField HeaderText="Address" DataField="Url" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# StatusTag(Eval("Online")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Response" DataField="ResponseText" />
                <asp:BoundField HeaderText="Detail" DataField="Detail" />
            </Columns>
        </asp:GridView>
    </div>
    <p class="muted">Addresses come from <code>Web.config</code> (<code>Services:Farmer</code>, <code>Services:Supplier</code>, <code>Services:Officer</code>, <code>Services:Admin</code>). A service shows OFFLINE when it is not running or does not answer within 3 seconds. Last checked: <asp:Literal ID="litTime" runat="server" />.</p>
    <script>setTimeout(function () { document.getElementById('<%= btnRefresh.ClientID %>').click(); }, 15000);</script>
</asp:Content>