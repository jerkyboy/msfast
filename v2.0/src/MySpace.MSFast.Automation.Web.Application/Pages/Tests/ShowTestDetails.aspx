<%@ Page Language="C#" MasterPageFile="~/Masters/MainWithSideMenu.Master" AutoEventWireup="true" CodeBehind="ShowTestDetails.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Pages.Tests.ShowTestDetails" %>
<%@ Register TagPrefix="Results" TagName="ResultsDisplayAndFilter" Src="~/Controls/Results/ResultsDisplayAndFilter.ascx" %>
<%@ Register TagPrefix="Tests" TagName="UpdateOrCreateTest" Src="~/Controls/Tests/UpdateOrCreateTest.ascx" %>
<%@ Register TagPrefix="Tests" TagName="TesterTypesPaging" Src="~/Controls/Tests/Browse/TesterTypesPaging.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <asp:PlaceHolder id="phValidTest" runat="server">
        <eyf:SubTitle runat="server" ID="sbTestURL" runat="server" />
        <Results:ResultsDisplayAndFilter ID="Results_ResultsDisplayAndFilter" runat="server" />
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phInvalidTest" runat="server">
        <asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.ShowTestDetails, invalidtest %>" />
    </asp:PlaceHolder>
</asp:Content>
