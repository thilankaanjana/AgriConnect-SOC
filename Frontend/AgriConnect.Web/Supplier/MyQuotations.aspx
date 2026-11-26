<%@ Page Title="My Quotations" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="MyQuotations.aspx.cs" Inherits="Supplier_MyQuotations" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>My Quotations</h1><p>Track the verification result of every quotation you submitted.</p></div></div>
    <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="panel">
        <h2>Edit quotation <asp:Literal ID="litEditRef" runat="server" /></h2>
        <p class="muted">A quotation can be changed only while it is waiting for the officer's verification.</p>
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtPrice" Text="Total quoted price (Rs.)" />
                <asp:TextBox ID="txtPrice" runat="server" placeholder="e.g. 120.00" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="qedit" ControlToValidate="txtPrice" ErrorMessage="Price is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ValidationGroup="qedit" ControlToValidate="txtPrice" Type="Double" MinimumValue="1" MaximumValue="999999999" ErrorMessage="Enter a price of 1 or more" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtStock" Text="Available stock quantity" />
                <asp:TextBox ID="txtStock" runat="server" placeholder="e.g. 500" TextMode="Number" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="qedit" ControlToValidate="txtStock" ErrorMessage="Stock is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ValidationGroup="qedit" ControlToValidate="txtStock" Type="Integer" MinimumValue="0" MaximumValue="999999999" ErrorMessage="Enter a whole number of 0 or more" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtDelivery" Text="Delivery estimate" />
            <asp:TextBox ID="txtDelivery" runat="server" placeholder="e.g. 3 days" />
            <asp:RequiredFieldValidator runat="server" ValidationGroup="qedit" ControlToValidate="txtDelivery" ErrorMessage="Delivery estimate is required" CssClass="err" Display="Dynamic" />
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="err" EnableViewState="false" />
        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn" ValidationGroup="qedit" OnClick="btnSave_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn alt" CausesValidation="false" OnClick="btnCancel_Click" />
    </asp:Panel>
    <div class="panel tablewrap">
        <asp:GridView ID="gvQuotes" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="You have not sent any quotations yet" OnRowCommand="gvQuotes_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Quote" DataField="RefNo" />
                <asp:BoundField HeaderText="Request" DataField="RequestRef" />
                <asp:BoundField HeaderText="Item" DataField="ItemText" />
                <asp:TemplateField HeaderText="Price"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Stock" DataField="Stock" />
                <asp:BoundField HeaderText="Delivery" DataField="Delivery" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Officer note" DataField="ReviewNote" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" CssClass="btn small" CommandName="EditQuote" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# (bool)Eval("CanEdit") %>' Text="Edit" />
                        <%# (bool)Eval("CanEdit") ? "" : "<span class=\"muted\">Locked</span>" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>