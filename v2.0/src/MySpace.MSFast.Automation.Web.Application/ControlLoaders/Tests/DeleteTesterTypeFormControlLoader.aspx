<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master"  AutoEventWireup="True" CodeBehind="DeleteTesterTypeFormControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.DeleteTesterTypeFormControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="DeleteTesterTypeForm" Src="~/Controls/Tests/DeleteTesterTypeForm.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:DeleteTesterTypeForm runat="server" id="Tests_DeleteTesterTypeForm"/>
</asp:Content>