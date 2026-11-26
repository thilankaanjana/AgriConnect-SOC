<%@ Page Title="Compliance Monitoring" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Compliance.aspx.cs" Inherits="Officer_Compliance" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Compliance Monitoring</h1><p>Review the verification history and the compliance record of suppliers in your region.</p></div></div>
    <div class="cards"><asp:Literal ID="litStats" runat="server" /></div>
    <h2>Verification history</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvReviews" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No verifications recorded yet">
            <Columns>
                <asp:TemplateField HeaderText="Date"><ItemTemplate><%# UIHelper.DateTimeText(Eval("Created")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Type" DataField="Type" />
                <asp:BoundField HeaderText="Reference" DataField="Ref" />
                <asp:TemplateField HeaderText="Decision"><ItemTemplate><%# UIHelper.Tag(Eval("Decision")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Checks passed" DataField="Score" />
                <asp:BoundField HeaderText="Note" DataField="Note" />
            </Columns>
        </asp:GridView>
    </div>
    <h2>Supplier compliance</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvSuppliers" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No supplier quotations yet">
            <Columns>
                <asp:BoundField HeaderText="Supplier" DataField="Supplier" />
                <asp:BoundField HeaderText="Quotations" DataField="Total" />
                <asp:BoundField HeaderText="Approved" DataField="Approved" />
                <asp:BoundField HeaderText="Rejected" DataField="Rejected" />
                <asp:BoundField HeaderText="Waiting" DataField="Waiting" />
                <asp:BoundField HeaderText="Approval rate" DataField="Rate" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>