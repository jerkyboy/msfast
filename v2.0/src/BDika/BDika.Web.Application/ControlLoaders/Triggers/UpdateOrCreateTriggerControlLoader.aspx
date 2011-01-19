<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master" AutoEventWireup="true" CodeBehind="UpdateOrCreateTriggerControlLoader.aspx.cs" Inherits="BDika.Web.Application.ControlLoaders.Triggers.UpdateOrCreateTriggerControlLoader" %>
<%@ Register TagPrefix="Triggers" TagName="UpdateOrCreateTrigger" Src="~/Controls/Triggers/UpdateOrCreateTrigger.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Triggers:UpdateOrCreateTrigger runat="server" id="Triggers_UpdateOrCreateTrigger"/>
</asp:Content>
