<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Buildings._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>Building Info</h1>
            </hgroup>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>&nbsp;</h3>
    <ol class="round">
        <li class="one">
            <h5>Total purchase for Samsung manufacture device:<br />
&nbsp;<asp:Label ID="lbl_Samsung" runat="server"></asp:Label>
            </h5>
        </li>
        <li class="two">
            <h5>Total number of times item 47 was purchased:<br />
                <asp:Label ID="lbl_Item47" runat="server"></asp:Label>
            </h5>
        </li>
        <li class="three">
            <h5>Total purchase cost for item category id 7:<br />
                <asp:Label ID="lbl_ItemCategory7" runat="server"></asp:Label>
            </h5>
        </li>
        <li class="four">
            <h5>Total purchase cost in Ontario:<br />
                <asp:Label ID="lbl_Ontario" runat="server"></asp:Label>
            </h5>
        </li>
        <li class="five">
            <h5>Total purchase cost in United States:<br />
                <asp:Label ID="lbl_US" runat="server"></asp:Label>
            </h5>
        </li>
        <li class="six">
            <h5>Building with the most total purchase cost:<br />
                <asp:Label ID="lbl_HighestPurchaseCost" runat="server"></asp:Label>
            </h5>
        </li>

    </ol>
</asp:Content>
