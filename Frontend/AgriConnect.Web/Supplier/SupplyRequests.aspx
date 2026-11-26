<%@ Page Title="Supply Requests" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SupplyRequests.aspx.cs" Inherits="Supplier_SupplyRequests" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Farmer Supply Requests</h1><p>Requests from your region. Review them and send a quotation with price, stock and delivery estimate.</p></div></div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No supply requests in your region yet">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="RefNo" />
                <asp:BoundField HeaderText="Farmer" DataField="Farmer" />
                <asp:BoundField HeaderText="Item" DataField="Item" />
                <asp:BoundField HeaderText="Quantity" DataField="Qty" />
                <asp:BoundField HeaderText="Delivery location" DataField="Location" />
                <asp:BoundField HeaderText="Preferred date" DataField="Date" />
                <asp:TemplateField HeaderText="Request status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="My quotation"><ItemTemplate><%# Eval("MyQuote").ToString() == "" ? "<span class=\"muted\">Not sent</span>" : UIHelper.Tag(Eval("MyQuote")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" CssClass="btn small" Text="Send quotation" NavigateUrl='<%# "~/Supplier/CreateQuotation.aspx?req=" + Eval("Id") %>' Visible='<%# (bool)Eval("CanQuote") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>