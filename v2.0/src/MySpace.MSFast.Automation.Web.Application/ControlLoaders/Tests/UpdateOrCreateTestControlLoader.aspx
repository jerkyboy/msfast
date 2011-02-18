<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="UpdateOrCreateTestControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTestControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="UpdateOrCreateTest" Src="~/Controls/Tests/UpdateOrCreateTest.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:UpdateOrCreateTest runat="server" id="Tests_UpdateOrCreateTest"/>
</asp:Content>