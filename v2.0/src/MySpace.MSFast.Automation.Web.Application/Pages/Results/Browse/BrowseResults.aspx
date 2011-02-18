<%@ Page Language="C#" MasterPageFile="~/Masters/MainWithSideMenu.Master" AutoEventWireup="true" CodeBehind="BrowseResults.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse.BrowseResults" %>
<%@ Register TagPrefix="Results" TagName="ResultsDisplayAndFilter" Src="~/Controls/Results/ResultsDisplayAndFilter.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Results:ResultsDisplayAndFilter ID="Results_ResultsDisplayAndFilter" runat="server" />
</asp:Content>
