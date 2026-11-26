<%@ Page Title="Recommendations" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Recommendations.aspx.cs" Inherits="Officer_Recommendations" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Agricultural Recommendations</h1><p>Advice for individual farmers, and guidelines shown on the public marketplace page.</p></div></div>
    <div class="panel">
        <h2>Recommendation for a supply request</h2>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="ddlRequest" Text="Request" />
            <asp:DropDownList ID="ddlRequest" runat="server" />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtText" Text="Recommendation" />
            <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Rows="3" placeholder="Enter agricultural advice" />
            <asp:RequiredFieldValidator runat="server" ValidationGroup="rec" ControlToValidate="txtText" ErrorMessage="Recommendation is required" CssClass="err" Display="Dynamic" />
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Save Recommendation" CssClass="btn" ValidationGroup="rec" OnClick="btnSave_Click" />
    </div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvRecs" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No recommendations yet">
            <Columns>
                <asp:BoundField HeaderText="Request" DataField="RequestRef" />
                <asp:TemplateField HeaderText="Date"><ItemTemplate><%# UIHelper.ShortDate(Eval("Created")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Recommendation" DataField="Text" />
            </Columns>
        </asp:GridView>
    </div>
    <div class="panel">
        <h2><asp:Literal ID="litGuideTitle" runat="server" Text="Publish a public guideline" /></h2>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtGTitle" Text="Title" />
            <asp:TextBox ID="txtGTitle" runat="server" placeholder="e.g. Pest control for the rainy season" />
            <asp:RequiredFieldValidator runat="server" ValidationGroup="guide" ControlToValidate="txtGTitle" ErrorMessage="Title is required" CssClass="err" Display="Dynamic" />
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtGText" Text="Guideline" />
            <asp:TextBox ID="txtGText" runat="server" TextMode="MultiLine" Rows="3" placeholder="Advice for all farmers" />
            <asp:RequiredFieldValidator runat="server" ValidationGroup="guide" ControlToValidate="txtGText" ErrorMessage="Guideline text is required" CssClass="err" Display="Dynamic" />
        </div>
        <asp:Button ID="btnPublish" runat="server" Text="Publish Guideline" CssClass="btn" ValidationGroup="guide" OnClick="btnPublish_Click" />
        <asp:Button ID="btnCancelGuide" runat="server" Text="Cancel" CssClass="btn alt" CausesValidation="false" Visible="false" OnClick="btnCancelGuide_Click" />
    </div>
    <div class="panel tablewrap">
        <asp:GridView ID="gvGuides" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No guidelines yet" OnRowCommand="gvGuides_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Title" DataField="Title" />
                <asp:BoundField HeaderText="Author" DataField="Author" />
                <asp:BoundField HeaderText="Date" DataField="Date" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <div class="actions">
                            <asp:LinkButton runat="server" CssClass="btn small" CommandName="EditGuide" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# IsMine(Eval("Author")) %>' Text="Edit" />
                            <asp:LinkButton runat="server" CssClass="btn warn small" CommandName="DeleteGuide" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# IsMine(Eval("Author")) %>' Text="Delete" OnClientClick="return confirm('Delete this guideline?');" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>