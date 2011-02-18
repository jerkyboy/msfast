<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/BrowseResults.Master" CodeBehind="BrowseTriggers.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Handlers.Triggers.Browse.BrowseTriggers" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersList" Src="~/Controls/Triggers/Browse/TriggersList.ascx" %>
<asp:Content id="cntResults" ContentPlaceHolderID="results" runat="Server">
    <Triggers:TriggersList runat="server" id="Triggers_TriggersList" />
</asp:Content>