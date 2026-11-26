<%@ Page Title="Audit Logs" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="ActivityLog.aspx.cs" Inherits="Admin_ActivityLog" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>Audit Logs</h1><p>Every important action of every user (logins, approvals, edits, orders), newest first.</p></div></div>
    <div class="panel">
        <div class="field" style="max-width:260px">
            <asp:Label runat="server" AssociatedControlID="ddlRole" Text="Show" />
            <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRole_Changed">
                <asp:ListItem Value="" Text="All roles" />
                <asp:ListItem Value="Farmer" Text="Farmers" />
                <asp:ListItem Value="Supplier" Text="Suppliers" />
                <asp:ListItem Value="Agriculture Officer" Text="Agriculture Officers" />
                <asp:ListItem Value="Administrator" Text="Administrators" />
            </asp:DropDownList>
        </div>
        <div class="tablewrap">
            <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No activity" AllowPaging="true" PageSize="25" OnPageIndexChanging="gvLog_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="Time"><ItemTemplate><%# UIHelper.DateTimeText(Eval("Created")) %></ItemTemplate></asp:TemplateField>
                    <asp:BoundField HeaderText="User" DataField="User" />
                    <asp:BoundField HeaderText="Role" DataField="Role" />
                    <asp:BoundField HeaderText="Action" DataField="Action" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>