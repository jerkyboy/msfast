﻿<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="BrowseTriggers.aspx.cs" Inherits="BDika.Web.Application.Pages.Triggers.Browse.BrowseTriggers" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersDisplayAndFilter" Src="~/Controls/Triggers/TriggersDisplayAndFilter.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Triggers:TriggersDisplayAndFilter ID="Triggers_TriggersDisplayAndFilter" runat="server" />
</asp:Content>
