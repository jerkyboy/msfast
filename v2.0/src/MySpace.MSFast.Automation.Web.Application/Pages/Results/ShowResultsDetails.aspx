<%@ Page Language="C#" MasterPageFile="~/Masters/MainWithSideMenu.Master"  AutoEventWireup="true" CodeBehind="ShowResultsDetails.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Pages.Results.ShowResultsDetails" %>
<%@ Register TagPrefix="Results" TagName="ResultsGraph" Src="~/Controls/Results/ResultsGraph.ascx" %>
<%@ Register TagPrefix="Results" TagName="ResultsThumbnails" Src="~/Controls/Results/ResultsThumbnails.ascx" %>

<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <asp:PlaceHolder id="phValidResults" runat="server">
        <eyf:Box runat="server">
            <eyf:SubTitle runat="server" ID="sbTestURL" runat="server" />
            <Results:ResultsGraph runat="server" id="Results_ResultsGraph" />
        </eyf:Box>
        <Results:ResultsThumbnails runat="server" id="Results_ResultsThumbnails" />
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phInvalidResults" runat="server">
        <asp:Literal runat="server" Text="<%$MSFAResources: Pages.Results.ShowResultsDetails, invalidresults %>" />
    </asp:PlaceHolder>
</asp:Content>
