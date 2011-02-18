<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="DeleteTriggerFormControlLoader.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.ControlLoaders.Triggers.DeleteTriggerFormControlLoader" %>
<%@ Register TagPrefix="Triggers" TagName="DeleteTriggerForm" Src="~/Controls/Triggers/DeleteTriggerForm.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Triggers:DeleteTriggerForm runat="server" id="Triggers_DeleteTriggerForm"/>
</asp:Content>