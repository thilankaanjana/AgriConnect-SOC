<%@ Page Title="Create Quotation" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CreateQuotation.aspx.cs" Inherits="Supplier_CreateQuotation" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Create Supplier Quotation</h1><p>Submit price, available stock and delivery estimate. The quotation is forwarded to the assigned agriculture officer for verification.</p></div></div>
    <asp:Panel ID="pnlNone" runat="server" Visible="false" CssClass="panel">
        <h2>No open requests</h2>
        <p>There are no open supply requests in your region that you have not already quoted.</p>
        <asp:HyperLink runat="server" NavigateUrl="~/Supplier/SupplyRequests.aspx" CssClass="btn alt" Text="View all requests" />
    </asp:Panel>
    <asp:Panel ID="pnlForm" runat="server" CssClass="panel">
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlRequest" Text="Supply request" />
            <asp:DropDownList ID="ddlRequest" runat="server" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlRequest_Changed" />
        </div>
        <div class="rv"><asp:Literal ID="litRequest" runat="server" /></div>
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtPrice" Text="Total quoted price (Rs.)" />
                <asp:TextBox ID="txtPrice" runat="server" placeholder="e.g. 120.00" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrice" ErrorMessage="Price is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtPrice" Type="Double" MinimumValue="1" MaximumValue="999999999" ErrorMessage="Enter a price of 1 or more" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtStock" Text="Available stock quantity" />
                <asp:TextBox ID="txtStock" runat="server" placeholder="e.g. 500" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStock" ErrorMessage="Stock is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtStock" Type="Integer" MinimumValue="0" MaximumValue="999999999" ErrorMessage="Enter a whole number of 0 or more" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtDelivery" Text="Delivery estimate" />
            <asp:TextBox ID="txtDelivery" runat="server" placeholder="e.g. 3 days" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDelivery" ErrorMessage="Delivery estimate is required" CssClass="err" Display="Dynamic" />
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="err" EnableViewState="false" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Quotation" CssClass="btn" OnClick="btnSubmit_Click" />
    </asp:Panel>
</asp:Content>