<%@ Page Language="C#" MasterPageFile="~/Masters/ControlLoaderMaster.Master"  AutoEventWireup="true" CodeBehind="UpdateTriggerPagesControlLoader.aspx.cs" Inherits="BDika.Web.Application.ControlLoaders.Triggers.UpdateTriggerPagesControlLoader" %>
<%@ Register TagPrefix="Triggers" TagName="UpdateTriggerPages" Src="~/Controls/Triggers/UpdateTriggerPages.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Triggers:UpdateTriggerPages runat="server" id="Triggers_UpdateTriggerPages"/>
</asp:Content>
