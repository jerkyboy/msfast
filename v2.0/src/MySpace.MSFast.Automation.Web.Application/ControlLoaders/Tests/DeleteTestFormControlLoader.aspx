<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="DeleteTestFormControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.DeleteTestFormControlLoader" %>
<%@ Register TagPrefix="Tests" TagName="DeleteTestForm" Src="~/Controls/Tests/DeleteTestForm.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:DeleteTestForm runat="server" id="Tests_DeleteTestForm"/>
</asp:Content>