<%@ Page Title="Transactions" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="Admin_Transactions" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Transactions and Activities</h1><p>Monitor supply requests, quotations and confirmed orders across the platform.</p></div></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <div class="panel">
        <div class="field" style="max-width:260px">
            <asp:Label runat="server" AssociatedControlID="ddlType" Text="Show" />
            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_Changed">
                <asp:ListItem Value="" Text="All transactions" />
                <asp:ListItem Value="Request" Text="Supply requests" />
                <asp:ListItem Value="Quotation" Text="Quotations" />
                <asp:ListItem Value="Order" Text="Orders" />
                <asp:ListItem Value="Sale" Text="Buyer crop orders" />
            </asp:DropDownList>
        </div>
        <div class="tablewrap">
            <asp:GridView ID="gvTx" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No transactions">
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="DateText" />
                    <asp:BoundField HeaderText="Type" DataField="Type" />
                    <asp:BoundField HeaderText="Reference" DataField="Reference" />
                    <asp:BoundField HeaderText="Parties" DataField="Parties" />
                    <asp:BoundField HeaderText="Details" DataField="Details" />
                    <asp:TemplateField HeaderText="Amount"><ItemTemplate><%# (decimal)Eval("Amount") > 0 ? UIHelper.Money(Eval("Amount")) : "-" %></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>