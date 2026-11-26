<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Admin_Users" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>User Management</h1><p>Approve new registrations, suspend accounts, and create Agriculture Officer and Administrator accounts.</p></div></div>
    <asp:Literal ID="litPending" runat="server" />
    <div class="panel">
        <h2>Create Account</h2>
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtName" Text="Full name" />
                <asp:TextBox ID="txtName" runat="server" placeholder="e.g. Nimal Perera" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email (used to log in)" />
                <asp:TextBox ID="txtEmail" runat="server" placeholder="e.g. nimal@example.com" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtContact" Text="Mobile number (OTP is sent here)" />
                <asp:TextBox ID="txtContact" runat="server" TextMode="Phone" placeholder="0771234567" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContact" ErrorMessage="Mobile number is required" CssClass="err" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtContact" ValidationExpression="^(\+?94|0)?7\d{8}$" ErrorMessage="Enter a valid mobile number, e.g. 0771234567" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtPassword" Text="Temporary password" />
                <div class="pw">
                    <asp:TextBox ID="txtPassword" runat="server" placeholder="At least 6 characters" TextMode="Password" />
                    <button type="button" class="eye" title="Show password" onclick="var t=this.previousElementSibling,s=t.type==='password';t.type=s?'text':'password';this.classList.toggle('on',s);this.title=s?'Hide password':'Show password';"><%= UIHelper.Icon("eye", 20) %></button>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" CssClass="err" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword" ValidationExpression=".{6,}" ErrorMessage="Password must be at least 6 characters" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlRole" Text="Role" />
                <asp:DropDownList ID="ddlRole" runat="server">
                    <asp:ListItem Value="Officer" Text="Agriculture Officer" />
                    <asp:ListItem Value="Admin" Text="Administrator" />
                </asp:DropDownList>
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlDistrict" Text="Assigned district (officers verify requests of this district)" />
                <asp:DropDownList ID="ddlDistrict" runat="server" />
            </div>
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="err" EnableViewState="false" />
        <div style="margin-top:8px"><asp:Button ID="btnAdd" runat="server" Text="Create Account" CssClass="btn" OnClick="btnAdd_Click" /></div>
    </div>
    <h2>Registered Users</h2>
    <div class="panel">
        <div class="field" style="max-width:260px">
            <asp:Label runat="server" AssociatedControlID="ddlFilter" Text="Show" />
            <asp:DropDownList ID="ddlFilter" runat="server" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlFilter_Changed">
                <asp:ListItem Value="" Text="All roles" />
                <asp:ListItem Value="Pending" Text="Waiting for approval" />
                <asp:ListItem Value="Farmer" Text="Farmers" />
                <asp:ListItem Value="Supplier" Text="Suppliers" />
                <asp:ListItem Value="Officer" Text="Agriculture Officers" />
                <asp:ListItem Value="Admin" Text="Administrators" />
            </asp:DropDownList>
        </div>
        <div class="tablewrap">
            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No users" OnRowCommand="gvUsers_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="User" DataField="Name" />
                    <asp:BoundField HeaderText="Email" DataField="Email" />
                    <asp:BoundField HeaderText="Mobile" DataField="Contact" />
                    <asp:BoundField HeaderText="District" DataField="District" />
                    <asp:TemplateField HeaderText="Role"><ItemTemplate><%# Auth.RoleLabel(Eval("Role")) %></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <div class="actions">
                                <asp:LinkButton runat="server" CssClass="btn small" CommandName="Approve" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# Eval("Status").ToString() == "Pending" %>' Text="Approve" />
                                <asp:LinkButton runat="server" CssClass="btn warn small" CommandName="Reject" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# Eval("Status").ToString() == "Pending" %>' Text="Reject" OnClientClick="return confirm('Reject this registration?');" />
                                <asp:LinkButton runat="server" CssClass="btn alt small" CommandName="Toggle" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# Eval("Status").ToString() != "Pending" %>' Text='<%# Eval("Status").ToString() == "Active" ? "Suspend" : "Activate" %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>