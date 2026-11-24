<%@ Page Title="Notifications" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Notifications.aspx.cs" Inherits="Notifications" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Notifications</h1><p>Messages about your requests, quotations, verifications, orders and deliveries.</p></div></div>
    <div class="panel"><asp:Literal ID="litList" runat="server" /></div>
</asp:Content>