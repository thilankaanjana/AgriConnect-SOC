<%@ Page Title="Marketplace" Language="C#" MasterPageFile="~/Public.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero">
        <div class="hero-in">
            <div>
                <span class="tagline">&#127793; Farm-fresh marketplace</span>
                <h1>Rooted in<br />nature <span class="leaf"><%= UIHelper.Icon("sprout", 54) %></span></h1>
                <p>Browse live crop listings from registered farmers, call them directly, and follow seasonal guidance from agriculture officers.</p>
                <div class="actions">
                    <a class="btn coral" href="#market">Browse products</a>
                    <asp:PlaceHolder ID="phJoin" runat="server"><button type="button" class="btn outline-w" onclick="openModal('register')">Join AgriConnect</button></asp:PlaceHolder>
                </div>
            </div>
            <div class="hero-pics">
                <img class="hp p1" src="<%= ResolveUrl("~/Content/img/brinjal.jpg") %>" alt="Fresh vegetables" />
                <img class="hp p2" src="<%= ResolveUrl("~/Content/img/mango.jpg") %>" alt="Fresh fruits" />
                <img class="hp p3" src="<%= ResolveUrl("~/Content/img/corn.jpg") %>" alt="Grains" />
                <div class="rate"><b><asp:Literal ID="litLive" runat="server" /></b><span>live listings</span></div>
            </div>
        </div>
        <svg class="hills" viewBox="0 0 1440 110" preserveAspectRatio="none" aria-hidden="true"><path d="M0,70 C240,10 480,110 760,60 C1040,10 1240,90 1440,40 L1440,110 L0,110 Z" fill="#0f3d2e" /></svg>
    </section>
    <div class="featbar"><div class="featbar-in">
        <span class="fi"><%= UIHelper.Icon("sprout", 20) %> Farm-fresh crops</span>
        <span class="fi"><%= UIHelper.Icon("shield", 20) %> Officer-verified supply</span>
        <span class="fi"><%= UIHelper.Icon("user", 20) %> Direct from farmers</span>
        <span class="fi"><%= UIHelper.Icon("cart", 20) %> Order tracking</span>
    </div></div>

    <section class="market" id="market"><div class="wrap">
        <div class="m-head">
            <h2 class="sec-title">Pure goodness, grown right here</h2>
            <span class="m-count"><asp:Literal ID="litCount" runat="server" /></span>
        </div>
        <div class="mk">
            <aside class="filters">
                <div class="fh"><span><%= UIHelper.Icon("filter", 18) %> Filter and Sort</span><asp:Literal ID="litClear" runat="server" /></div>
                <asp:Literal ID="litFilters" runat="server" />
            </aside>
            <div>
                <div class="achips"><asp:Literal ID="litActive" runat="server" /></div>
                <div class="tabs"><asp:Literal ID="litTabs" runat="server" /></div>
                <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-box">No crops match your search or filters.</asp:Panel>
                <div class="pgrid">
                    <asp:Repeater ID="rptCrops" runat="server">
                        <ItemTemplate>
                            <article class="pc">
                                <div class="im">
                                    <img src='<%# ResolveUrl((string)Eval("ImageUrl")) %>' alt='<%# Server.HtmlEncode((string)Eval("Name")) %>' />
                                    <span class="badge"><%# Server.HtmlEncode((string)Eval("Category")) %></span>
                                </div>
                                <div class="pb">
                                    <div class="pt">
                                        <div class="pn"><%# Server.HtmlEncode((string)Eval("Name")) %></div>
                                        <div class="pp"><%# "Rs " + Eval("Price", "{0:N0}") %>/kg</div>
                                    </div>
                                    <div class="pm">
                                        <span><%# UIHelper.Icon("user", 14) %> <%# Server.HtmlEncode((string)Eval("Farmer")) %></span>
                                        <span><%# UIHelper.Icon("pin", 14) %> <%# Server.HtmlEncode((string)Eval("Location")) %></span>
                                    </div>
                                    <div class="pa">
                                        <span>Available: <b><%# Server.HtmlEncode((string)Eval("Qty")) %></b></span>
                                        <%# string.IsNullOrEmpty((string)Eval("Tag")) ? "" : "<span class=\"tagp\">" + Server.HtmlEncode((string)Eval("Tag")) + "</span>" %>
                                    </div>
                                    <div class="pbtn">
                                        <a class="btn buy" href='<%# ResolveUrl("~/Marketplace/CropDetails.aspx?id=" + Eval("Id")) %>'><%# UIHelper.Icon("cart") %> Buy Now</a>
                                        <a class="btn ghost sq" title="Call farmer" href='tel:<%# Server.HtmlEncode((string)Eval("Contact")) %>'><%# UIHelper.Icon("phone") %></a>
                                    </div>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div></section>

    <section class="cream purpose"><div class="wrap pg2">
        <div>
            <h2 class="sec-title">Growing with purpose!</h2>
            <p>AgriConnect links farmers, suppliers and agriculture officers on one platform. Farmers publish their harvest, suppliers quote farm inputs, and officers verify every request.</p>
            <a class="more" href="#market">Visit the market &rarr;</a>
            <div class="pstats">
                <div class="ps"><b><asp:Literal ID="litSFarmers" runat="server" /></b><span>Farmers</span></div>
                <div class="ps"><b><asp:Literal ID="litSSuppliers" runat="server" /></b><span>Suppliers</span></div>
                <div class="ps"><b><asp:Literal ID="litSListings" runat="server" /></b><span>Listings</span></div>
            </div>
        </div>
        <div class="pphotos">
            <img src="<%= ResolveUrl("~/Content/img/beans.jpg") %>" alt="Fresh vegetables" />
            <img src="<%= ResolveUrl("~/Content/img/fruits.jpg") %>" alt="Fresh fruits" />
        </div>
    </div></section>

    <section class="care"><div class="wrap">
        <h2 class="sec-title">We care about quality</h2>
        <div class="bento">
            <div class="bt photo" style="background-image:url('<%= ResolveUrl("~/Content/img/paddy.jpg") %>')"><span class="seal">100%<br />Fresh</span><h3>Straight from the farm</h3><p>Listings come only from registered farmers.</p></div>
            <div class="bt photo" style="background-image:url('<%= ResolveUrl("~/Content/img/carrot.jpg") %>')"><h3>Best quality</h3><p>Officers verify supply requests and quotations before orders go ahead.</p></div>
            <div class="bt photo" style="background-image:url('<%= ResolveUrl("~/Content/img/grains.jpg") %>')"><h3>Grains &amp; cereals</h3><p>Harvested locally.</p></div>
            <div class="bt photo" style="background-image:url('<%= ResolveUrl("~/Content/img/veggies.jpg") %>')"><h3>Locally grown</h3><p>Support farmers near you.</p></div>
        </div>
    </div></section>

    <section class="guides" id="guidelines"><div class="wrap">
        <h2 class="sec-title">Latest agricultural guidelines</h2>
        <div class="gg">
            <asp:Repeater ID="rptGuides" runat="server">
                <ItemTemplate>
                    <div class="gc">
                        <h3><%# Server.HtmlEncode((string)Eval("Title")) %></h3>
                        <div class="gm"><%# Server.HtmlEncode((string)Eval("Author")) %> &middot; <%# Server.HtmlEncode((string)Eval("Date")) %></div>
                        <p><%# Server.HtmlEncode((string)Eval("Text")) %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div></section>
</asp:Content>