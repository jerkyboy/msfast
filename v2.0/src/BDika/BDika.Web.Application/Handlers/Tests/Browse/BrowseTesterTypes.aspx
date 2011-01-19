<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/BrowseResults.Master" CodeBehind="BrowseTesterTypes.aspx.cs" Inherits="BDika.Web.Application.Handlers.Tests.Browse.BrowseTesterTypes" %>
<%@ Register TagPrefix="Tests" TagName="TesterTypesList" Src="~/Controls/Tests/Browse/TesterTypesList.ascx" %>
<asp:Content id="cntResults" ContentPlaceHolderID="results" runat="Server">
    <Tests:TesterTypesList ID="Tests_TesterTypesList" runat="server" />
</asp:Content>