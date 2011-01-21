<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master"  AutoEventWireup="true" CodeBehind="ShowResultsDetails.aspx.cs" Inherits="BDika.Web.Application.Pages.Results.ShowResultsDetails" %>
<%@ Register TagPrefix="Results" TagName="ResultsGraph" Src="~/Controls/Results/ResultsGraph.ascx" %>
<%@ Register TagPrefix="Results" TagName="ResultsThumbnails" Src="~/Controls/Results/ResultsThumbnails.ascx" %>

<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <asp:PlaceHolder id="phValidResults" runat="server">
        <eyf:SubTitle runat="server" ID="sbTestURL" runat="server" />
        <Results:ResultsGraph runat="server" id="Results_ResultsGraph" />
        <Results:ResultsThumbnails runat="server" id="Results_ResultsThumbnails" />
        
    </asp:PlaceHolder>
    <asp:PlaceHolder id="phInvalidResults" runat="server">
        <asp:Literal runat="server" Text="<%$BDikaResources: Pages.Results.ShowResultsDetails, invalidresults %>" />
    </asp:PlaceHolder>
</asp:Content>
