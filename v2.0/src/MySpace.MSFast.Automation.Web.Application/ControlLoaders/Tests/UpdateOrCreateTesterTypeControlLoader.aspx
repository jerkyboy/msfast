<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="UpdateOrCreateTesterTypeControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTesterTypeControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="UpdateOrCreateTesterType" Src="~/Controls/Tests/UpdateOrCreateTesterType.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:UpdateOrCreateTesterType runat="server" id="Tests_UpdateOrCreateTesterType"/>
</asp:Content>
