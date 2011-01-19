<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master"  AutoEventWireup="true" CodeBehind="ShowResultsDetails.aspx.cs" Inherits="BDika.Web.Application.Pages.Results.ShowResultsDetails" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <asp:PlaceHolder id="phValidResults" runat="server">
        <eyf:SubTitle runat="server" ID="sbTestURL" runat="server" />
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phInvalidResults" runat="server">
        <asp:Literal runat="server" Text="<%$BDikaResources: Pages.Results.ShowResultsDetails, invalidresults %>" />
    </asp:PlaceHolder>
</asp:Content>
