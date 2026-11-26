<%@ Page Title="Deliveries" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Deliveries.aspx.cs" Inherits="Supplier_Deliveries" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Delivery Management</h1><p>Update the delivery status of confirmed orders. The farmer is notified of every change.</p></div></div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" DataKeyNames="Id" EmptyDataText="No confirmed orders yet">
            <Columns>
                <asp:BoundField HeaderText="Order" DataField="RefNo" />
                <asp:BoundField HeaderText="Farmer" DataField="FarmerName" />
                <asp:BoundField HeaderText="Item" DataField="ItemText" />
                <asp:BoundField HeaderText="Deliver to" DataField="Location" />
                <asp:TemplateField HeaderText="Amount"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Current status"><ItemTemplate><%# UIHelper.Tag(Eval("DeliveryStatus")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Update">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_Changed" Visible='<%# Eval("DeliveryStatus").ToString() != "Delivered" %>'>
                            <asp:ListItem Value="" Text="Choose status" />
                            <asp:ListItem>Processing</asp:ListItem>
                            <asp:ListItem>Dispatched</asp:ListItem>
                            <asp:ListItem>Delivered</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label runat="server" Text="Completed" Visible='<%# Eval("DeliveryStatus").ToString() == "Delivered" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>