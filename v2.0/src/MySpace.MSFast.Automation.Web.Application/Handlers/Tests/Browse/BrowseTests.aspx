<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/BrowseResults.Master"  CodeBehind="BrowseTests.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Handlers.Tests.Browse.BrowseTests" %>
<%@ Register TagPrefix="Tests" TagName="TestsList" Src="~/Controls/Tests/Browse/TestsList.ascx" %>
<asp:Content id="cntResults" ContentPlaceHolderID="results" runat="Server">
    <Tests:TestsList ID="Tests_TestsList" runat="server" />
</asp:Content>