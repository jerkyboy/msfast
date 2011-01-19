<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/BrowseResults.Master" CodeBehind="BrowseResults.aspx.cs" Inherits="BDika.Web.Application.Handlers.Results.Browse.BrowseResults" %>
<%@ Register TagPrefix="Results" TagName="ResultsList" Src="~/Controls/Results/Browse/ResultsList.ascx" %>
<asp:Content id="cntResults" ContentPlaceHolderID="results" runat="Server">
    <Results:ResultsList runat="server" id="Results_ResultsList" />
</asp:Content>