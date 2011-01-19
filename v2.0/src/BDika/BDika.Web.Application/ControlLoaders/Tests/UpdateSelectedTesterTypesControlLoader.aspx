<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="UpdateSelectedTesterTypesControlLoader.aspx.cs" Inherits="BDika.Web.Application.ControlLoaders.Tests.UpdateSelectedTesterTypesControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="UpdateSelectedTesterTypes" Src="~/Controls/Tests/UpdateSelectedTesterTypes.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:UpdateSelectedTesterTypes runat="server" id="Tests_UpdateSelectedTesterTypes"/>
</asp:Content>