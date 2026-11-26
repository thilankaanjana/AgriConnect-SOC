<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="UserProfile" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>My Profile</h1><p>Update your contact details and password. Your mobile number is also your login username.</p></div></div>
    <div class="panel">
        <h2>Account</h2>
        <div class="rv"><asp:Literal ID="litAccount" runat="server" /></div>
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtName" Text="Full name / Business name" />
                <asp:TextBox ID="txtName" runat="server" placeholder="e.g. Kamal Silva" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="profile" ControlToValidate="txtName" ErrorMessage="Name is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email (cannot be changed)" />
                <asp:TextBox ID="txtEmail" runat="server" ReadOnly="true" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtContact" Text="Mobile number (login username)" />
                <asp:TextBox ID="txtContact" runat="server" placeholder="e.g. 0771234567" TextMode="Phone" />
                <asp:RequiredFieldValidator runat="server" ValidationGroup="profile" ControlToValidate="txtContact" ErrorMessage="Mobile number is required" CssClass="err" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ValidationGroup="profile" ControlToValidate="txtContact" ValidationExpression="^(\+?94|0)?7\d{8}$" ErrorMessage="Enter a valid mobile number, e.g. 0771234567" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlDistrict" Text="District" />
                <asp:DropDownList ID="ddlDistrict" runat="server" />
            </div>
        </div>
        <div class="field">
            <asp:Label runat="server" AssociatedControlID="txtAddress" Text="Address" />
            <asp:TextBox ID="txtAddress" runat="server" placeholder="e.g. 25, Temple Road, Kurunegala" />
        </div>
        <asp:Label ID="lblProfileError" runat="server" CssClass="err" EnableViewState="false" />
        <asp:Button ID="btnSaveProfile" runat="server" Text="Save Profile" CssClass="btn" ValidationGroup="profile" OnClick="btnSaveProfile_Click" />
    </div>
    <div class="panel">
        <h2>Change Password</h2>
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtCurrent" Text="Current password" />
                <div class="pw">
                    <asp:TextBox ID="txtCurrent" runat="server" placeholder="Enter current password" TextMode="Password" autocomplete="current-password" />
                    <button type="button" class="eye" title="Show password" onclick="var t=this.previousElementSibling,s=t.type==='password';t.type=s?'text':'password';this.classList.toggle('on',s);this.title=s?'Hide password':'Show password';"><%= UIHelper.Icon("eye", 20) %></button>
                </div>
                <asp:RequiredFieldValidator runat="server" ValidationGroup="pwd" ControlToValidate="txtCurrent" ErrorMessage="Enter your current password" CssClass="err" Display="Dynamic" />
            </div>
            <div></div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtNew" Text="New password" />
                <div class="pw">
                    <asp:TextBox ID="txtNew" runat="server" placeholder="At least 6 characters" TextMode="Password" autocomplete="new-password" />
                    <button type="button" class="eye" title="Show password" onclick="var t=this.previousElementSibling,s=t.type==='password';t.type=s?'text':'password';this.classList.toggle('on',s);this.title=s?'Hide password':'Show password';"><%= UIHelper.Icon("eye", 20) %></button>
                </div>
                <asp:RequiredFieldValidator runat="server" ValidationGroup="pwd" ControlToValidate="txtNew" ErrorMessage="Enter a new password" CssClass="err" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ValidationGroup="pwd" ControlToValidate="txtNew" ValidationExpression=".{6,}" ErrorMessage="Password must be at least 6 characters" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtConfirm" Text="Confirm new password" />
                <div class="pw">
                    <asp:TextBox ID="txtConfirm" runat="server" placeholder="Re-enter new password" TextMode="Password" autocomplete="new-password" />
                    <button type="button" class="eye" title="Show password" onclick="var t=this.previousElementSibling,s=t.type==='password';t.type=s?'text':'password';this.classList.toggle('on',s);this.title=s?'Hide password':'Show password';"><%= UIHelper.Icon("eye", 20) %></button>
                </div>
                <asp:CompareValidator runat="server" ValidationGroup="pwd" ControlToValidate="txtConfirm" ControlToCompare="txtNew" ErrorMessage="The two passwords do not match" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <asp:Label ID="lblPwdError" runat="server" CssClass="err" EnableViewState="false" />
        <asp:Button ID="btnChangePwd" runat="server" Text="Change Password" CssClass="btn" ValidationGroup="pwd" OnClick="btnChangePwd_Click" />
    </div>
</asp:Content>