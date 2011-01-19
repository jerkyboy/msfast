<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="UpdateTestPagesControlLoader.aspx.cs" Inherits="BDika.Web.Application.ControlLoaders.Tests.UpdateTestPagesControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="UpdateTestPages" Src="~/Controls/Tests/UpdateTestPages.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:UpdateTestPages runat="server" id="Tests_UpdateTestPages"/>
</asp:Content>