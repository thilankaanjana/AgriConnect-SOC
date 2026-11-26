<%@ Page Title="Buyer Orders" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CropOrders.aspx.cs" Inherits="Farmer_CropOrders" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Buyer Orders</h1><p>Buyers who found your crops in the marketplace. Contact them, then accept or decline each order.</p></div></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" DataKeyNames="Id" EmptyDataText="No buyer orders yet. Publish crops under Crop Listings and buyers will find them in the marketplace." OnRowCommand="gvOrders_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Order" DataField="RefNo" />
                <asp:TemplateField HeaderText="Date"><ItemTemplate><%# UIHelper.DateTimeText(Eval("Created")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Crop" DataField="Crop" />
                <asp:BoundField HeaderText="Quantity" DataField="Quantity" />
                <asp:TemplateField HeaderText="Total"><ItemTemplate><%# UIHelper.Money(Eval("Total")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Buyer" DataField="BuyerName" />
                <asp:TemplateField HeaderText="Phone"><ItemTemplate><a href='tel:<%# Eval("BuyerPhone") %>'><%# Eval("BuyerPhone") %></a></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Address" DataField="BuyerAddress" />
                <asp:BoundField HeaderText="Note" DataField="Note" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CssClass="btn small" CommandName="Accepted" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Accept" Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                        <asp:LinkButton runat="server" CssClass="btn small" CommandName="Completed" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Mark completed" Visible='<%# Eval("Status").ToString() == "Accepted" %>' />
                        <asp:LinkButton runat="server" CssClass="btn alt small" CommandName="Declined" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Decline" Visible='<%# Eval("Status").ToString() == "Pending" || Eval("Status").ToString() == "Accepted" %>' OnClientClick="return confirm('Decline this order?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>