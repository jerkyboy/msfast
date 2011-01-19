<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="BrowseResults.aspx.cs" Inherits="BDika.Web.Application.Pages.Results.Browse.BrowseResults" %>
<%@ Register TagPrefix="Results" TagName="ResultsDisplayAndFilter" Src="~/Controls/Results/ResultsDisplayAndFilter.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Results:ResultsDisplayAndFilter ID="Results_ResultsDisplayAndFilter" runat="server" />
</asp:Content>
