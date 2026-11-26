<%@ Page Title="Crop Listings" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="CropListings.aspx.cs" Inherits="Farmer_CropListings" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="head"><div><h1>My Crop Listings</h1><p>Publish harvested crops. They appear at once in the public Marketplace, where buyers can place orders (see Buyer Orders). <a href="../Default.aspx#market">Open the marketplace</a>.</p></div></div>
    <div class="panel">
        <asp:Literal ID="litMode" runat="server" />
        <div class="grid2">
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtName" Text="Crop name" />
                <asp:TextBox ID="txtName" runat="server" placeholder="e.g. Tomatoes" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" ErrorMessage="Crop name is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtQty" Text="Quantity" />
                <asp:TextBox ID="txtQty" runat="server" placeholder="e.g. 100 kg" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQty" ErrorMessage="Quantity is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtPrice" Text="Price per kg (Rs.)" />
                <asp:TextBox ID="txtPrice" runat="server" TextMode="Number" placeholder="e.g. 280" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPrice" ErrorMessage="Price is required" CssClass="err" Display="Dynamic" />
                <asp:RangeValidator runat="server" ControlToValidate="txtPrice" Type="Double" MinimumValue="1" MaximumValue="999999999" ErrorMessage="Enter a price of 1 or more" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="ddlQuality" Text="Quality label (optional)" />
                <asp:DropDownList ID="ddlQuality" runat="server">
                    <asp:ListItem Value="" Text="None" />
                    <asp:ListItem Value="Organic" Text="Organic" />
                    <asp:ListItem Value="Pesticide-free" Text="Pesticide-free" />
                </asp:DropDownList>
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtLocation" Text="Location" />
                <asp:TextBox ID="txtLocation" runat="server" placeholder="e.g. Kurunegala" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLocation" ErrorMessage="Location is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtContact" Text="Contact number" />
                <asp:TextBox ID="txtContact" runat="server" placeholder="e.g. 0771234567" TextMode="Phone" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtContact" ErrorMessage="Contact number is required" CssClass="err" Display="Dynamic" />
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="fuPhoto" Text="Crop photo (JPG / PNG / WEBP, max 3 MB)" />
                <asp:FileUpload ID="fuPhoto" runat="server" accept=".jpg,.jpeg,.png,.webp" />
                <asp:CustomValidator ID="cvPhoto" runat="server" ControlToValidate="fuPhoto" ValidateEmptyText="true" OnServerValidate="cvPhoto_ServerValidate" ErrorMessage="Please choose a JPG, PNG or WEBP photo under 3 MB" CssClass="err" Display="Dynamic" />
            </div>
        </div>
        <asp:Button ID="btnPublish" runat="server" Text="Publish Crop Listing" CssClass="btn" OnClick="btnPublish_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn alt" CausesValidation="false" Visible="false" OnClick="btnCancel_Click" />
    </div>
    <h2>My Published Crops</h2>
    <div class="panel tablewrap">
        <asp:GridView ID="gvCrops" runat="server" AutoGenerateColumns="false" CssClass="grid" GridLines="None" EmptyDataText="No crop listings yet" OnRowCommand="gvCrops_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Photo"><ItemTemplate><img class="thumb" style="width:56px;height:56px;object-fit:cover;border-radius:8px" src='<%# ResolveUrl((string)Eval("ImageUrl")) %>' alt="" /></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Crop" DataField="Name" />
                <asp:BoundField HeaderText="Quantity" DataField="Qty" />
                <asp:TemplateField HeaderText="Price/kg"><ItemTemplate><%# UIHelper.Money(Eval("Price")) %></ItemTemplate></asp:TemplateField>
                <asp:BoundField HeaderText="Location" DataField="Location" />
                <asp:TemplateField HeaderText="Status"><ItemTemplate><%# UIHelper.Tag(Eval("Status")) %></ItemTemplate></asp:TemplateField>
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <div class="actions">
                            <asp:LinkButton runat="server" CssClass="btn small" CommandName="EditCrop" CommandArgument='<%# Eval("Id") %>' CausesValidation="false" Text="Edit" />
                            <asp:LinkButton runat="server" CssClass="btn alt small" CommandName="Toggle" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Status").ToString() == "Available" ? "Mark sold out" : "Relist" %>' />
                            <asp:LinkButton runat="server" CssClass="btn warn small" CommandName="Remove" CommandArgument='<%# Eval("Id") %>' Text="Delete" OnClientClick="return confirm('Delete this listing?');" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>