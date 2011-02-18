<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="BrowseTestsForTriggerAndTesterTypeTestsListControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.Browse.BrowseTestsForTriggerAndTesterTypeTestsListControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="TestsList" Src="~/Controls/Tests/Browse/TestsList.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:TestsList ID="Tests_TestsList" runat="server" />
</asp:Content>