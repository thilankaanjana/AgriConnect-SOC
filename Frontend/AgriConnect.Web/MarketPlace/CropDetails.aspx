<%@ Page Title="Crop Details" Language="C#" MasterPageFile="~/Public.master" AutoEventWireup="true" CodeBehind="CropDetails.aspx.cs" Inherits="Marketplace_CropDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="wrap">
        <div class="crumb"><a href="~/Default.aspx#market" runat="server">Marketplace</a> / <asp:Literal ID="litCrumb" runat="server" /></div>
        <asp:Panel ID="pnlCrop" runat="server" CssClass="dcard">
            <asp:Image ID="imgCrop" runat="server" CssClass="dimg" />
            <div class="dbody">
                <span class="eyebrow"><asp:Literal ID="litCat" runat="server" /></span>
                <h1><asp:Literal ID="litName" runat="server" /></h1>
                <div class="dprice"><asp:Literal ID="litPrice" runat="server" /> / kg</div>
                <div class="drow"><%= UIHelper.Icon("user") %> Farmer: <strong><asp:Literal ID="litFarmer" runat="server" /></strong></div>
                <div class="drow"><%= UIHelper.Icon("pin") %> <asp:Literal ID="litLocation" runat="server" /></div>
                <div class="drow">Available quantity: <strong><asp:Literal ID="litQty" runat="server" /></strong></div>
                <div class="actions" style="margin-top:22px">
                    <a class="btn coral" href="#buy"><%= UIHelper.Icon("cart") %> Order this crop</a>
                    <asp:HyperLink ID="lnkCall" runat="server" CssClass="btn ghost" />
                    <a class="btn ghost" href="~/Default.aspx#market" runat="server">Back to market</a>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBuy" runat="server" CssClass="dcard" style="display:block;padding:26px;margin-top:22px">
            <a id="buy"></a>
            <h2 style="margin-top:0">Place your order</h2>
            <p class="muted">No account needed. Your order goes straight to the farmer, who will call you to confirm the price, payment and delivery.</p>
            <div class="grid2">
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="txtBuyerName" Text="Your name" />
                    <asp:TextBox ID="txtBuyerName" runat="server" placeholder="e.g. Sunil Fernando" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBuyerName" ValidationGroup="buy" ErrorMessage="Your name is required" CssClass="err" Display="Dynamic" />
                </div>
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="txtBuyerPhone" Text="Mobile number" />
                    <asp:TextBox ID="txtBuyerPhone" runat="server" TextMode="Phone" placeholder="0771234567" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBuyerPhone" ValidationGroup="buy" ErrorMessage="Mobile number is required" CssClass="err" Display="Dynamic" />
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txtBuyerPhone" ValidationGroup="buy" ValidationExpression="^(\+?94|0)?7\d{8}$" ErrorMessage="Enter a valid mobile number, e.g. 0771234567" CssClass="err" Display="Dynamic" />
                </div>
                <div class="field">
                    <asp:Label ID="lblQtyLabel" runat="server" AssociatedControlID="txtBuyQty" Text="Quantity (kg)" />
                    <asp:TextBox ID="txtBuyQty" runat="server" placeholder="e.g. 25" TextMode="Number" step="any" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBuyQty" ValidationGroup="buy" ErrorMessage="Quantity is required" CssClass="err" Display="Dynamic" />
                    <asp:RangeValidator runat="server" ControlToValidate="txtBuyQty" ValidationGroup="buy" Type="Double" MinimumValue="0.1" MaximumValue="999999" ErrorMessage="Enter a quantity greater than 0" CssClass="err" Display="Dynamic" />
                </div>
                <div class="field">
                    <asp:Label runat="server" AssociatedControlID="txtBuyerAddress" Text="Delivery address (optional)" />
                    <asp:TextBox ID="txtBuyerAddress" runat="server" placeholder="e.g. 10, Main Street, Colombo" />
                </div>
            </div>
            <div class="field">
                <asp:Label runat="server" AssociatedControlID="txtNote" Text="Note to the farmer (optional)" />
                <asp:TextBox ID="txtNote" runat="server" placeholder="Any message for the farmer (optional)" TextMode="MultiLine" Rows="2" />
            </div>
            <asp:Label ID="lblBuyError" runat="server" CssClass="err" EnableViewState="false" />
            <asp:Button ID="btnBuy" runat="server" Text="Send order to farmer" CssClass="btn coral" ValidationGroup="buy" OnClick="btnBuy_Click" />
        </asp:Panel>
        <asp:Panel ID="pnlDone" runat="server" Visible="false" CssClass="dcard" style="display:block;padding:26px;margin-top:22px">
            <h2 style="margin-top:0">Order sent &#10003;</h2>
            <p><asp:Literal ID="litDone" runat="server" /></p>
            <a class="btn ghost" href="~/Default.aspx#market" runat="server">Continue shopping</a>
        </asp:Panel>
        <asp:Panel ID="pnlNone" runat="server" Visible="false" CssClass="empty-box" style="margin:40px 0">No crops available.</asp:Panel>
    </section>
</asp:Content>