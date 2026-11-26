<%@ Page Title="Verifications" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Verifications.aspx.cs" Inherits="Officer_Verifications" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Request and Quotation Verification</h1><p>Review compliance and suitability, then approve or reject. Only items assigned to you are listed.</p></div></div>

    <asp:Panel ID="pnlReview" runat="server" Visible="false" CssClass="panel">
        <h2><asp:Literal ID="litTitle" runat="server" /></h2>
        <div class="rv"><asp:Literal ID="litDetail" runat="server" /></div>
        <h2>Compliance checklist</h2>
        <asp:CheckBoxList ID="cblChecks" runat="server" CssClass="checks" />
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtNote" Text="Officer note (required when rejecting)" />
            <asp:TextBox ID="txtNote" runat="server" placeholder="Reason or note for the supplier (optional)" TextMode="MultiLine" Rows="3" />
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="err" EnableViewState="false" />
        <div class="actions" style="margin-top:8px">
            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn" OnClick="btnApprove_Click" CausesValidation="false" />
            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn warn" OnClick="btnReject_Click" CausesValidation="false" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn alt" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
        <asp:HiddenField ID="hdnType" runat="server" />
        <asp:HiddenField ID="hdnId" runat="server" />
    </asp:Panel>

    <h2>Supply Requests</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No requests assigned to you" OnRowCommand="gvRequests_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Request" DataField="RefNo" />
                <asp:BoundField HeaderText="Farmer" DataField="Farmer" />
                <asp:BoundField HeaderText="Item" DataField="ItemText" />
                <asp:BoundField HeaderText="District" DataField="District" />
                <asp:BoundField HeaderText="Submitted" DataField="CreatedText" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Note" DataField="Note" />
                <asp:TemplateField HeaderText="Decision"><ItemTemplate><asp:LinkButton runat="server" Text="Review" CssClass="btn small" CommandName="Review" CommandArgument='<%# Eval("Id") %>' Visible='<%# (bool)Eval("CanReview") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <h2>Supplier Quotations</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvQuotes" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No quotations for your requests yet" OnRowCommand="gvQuotes_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Quote" DataField="RefNo" />
                <asp:BoundField HeaderText="Request" DataField="RequestRef" />
                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                <asp:TemplateField HeaderText="Price"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Stock" DataField="Stock" />
                <asp:BoundField HeaderText="Delivery" DataField="Delivery" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Decision"><ItemTemplate><asp:LinkButton runat="server" Text="Review" CssClass="btn small" CommandName="Review" CommandArgument='<%# Eval("Id") %>' Visible='<%# (bool)Eval("CanReview") %>' /></ItemTemplate></asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>