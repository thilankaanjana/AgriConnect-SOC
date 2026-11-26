<%@ Page Title="Supply Request" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="SupplyRequest.aspx.cs" Inherits="Farmer_SupplyRequest" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>New Supply Request</h1><p>Suppliers in your district are notified and an agriculture officer is assigned to verify your request. Not sure what to ask for? <a href="SupplierStock.aspx">See what suppliers have in stock</a>.</p></div></div>
    <div class="panel">
        <asp:Literal ID="litMode" runat="server" />
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtItem" Text="Item name" />
                <asp:TextBox ID="txtItem" runat="server" placeholder="e.g. Fertilizer" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtItem" ErrorMessage="Item name is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtAmount" Text="Quantity" />
                <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" placeholder="e.g. 50" step="any" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAmount" ErrorMessage="Quantity is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtAmount" Type="Double" MinimumValue="0.01" MaximumValue="999999" ErrorMessage="Enter a quantity greater than 0" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlUnit" Text="Unit" />
                <asp:DropDownList ID="ddlUnit" runat="server">
                    <asp:ListItem>kg</asp:ListItem><asp:ListItem>bags</asp:ListItem><asp:ListItem>litres</asp:ListItem><asp:ListItem>packets</asp:ListItem><asp:ListItem>units</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlDistrict" Text="District" />
                <asp:DropDownList ID="ddlDistrict" runat="server" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtLocation" Text="Delivery location" />
                <asp:TextBox ID="txtLocation" runat="server" placeholder="Village / town / address" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLocation" ErrorMessage="Delivery location is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtDate" Text="Preferred date" />
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDate" ErrorMessage="Preferred date is required" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <asp:Label ID="lblError" runat="server" CssClass="err" EnableViewState="false" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Request" CssClass="btn" OnClick="btnSubmit_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn alt" CausesValidation="false" Visible="false" OnClick="btnCancel_Click" />
    </div>
    <h2>My Supply Requests</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvRequests" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No requests yet" OnRowCommand="gvRequests_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="RefNo" />
                <asp:BoundField HeaderText="Item" DataField="Item" />
                <asp:BoundField HeaderText="Quantity" DataField="Qty" />
                <asp:BoundField HeaderText="District" DataField="District" />
                <asp:BoundField HeaderText="Officer" DataField="Officer" />
                <asp:BoundField HeaderText="Quotes" DataField="Quotes" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Officer note" DataField="Note" />
                <asp:BoundField HeaderText="Recommendation" DataField="Advice" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <div class="actions">
                            <asp:LinkButton runat="server" CssClass="btn small" CommandName="EditReq" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# (bool)Eval("CanChange") %>' Text="Edit" />
                            <asp:LinkButton runat="server" CssClass="btn warn small" CommandName="DeleteReq" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Visible='<%# (bool)Eval("CanChange") %>' Text="Delete" OnClientClick="return confirm('Delete this supply request?');" />
                            <%# (bool)Eval("CanChange") ? "" : "<span class=\"muted\">Locked</span>" %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>