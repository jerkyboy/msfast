<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="TriggersPaging.ascx.cs" Inherits="BDika.Web.Application.Controls.Triggers.Browse.TriggersPaging" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersList" Src="~/Controls/Triggers/Browse/TriggersList.ascx" %>
<eyf:BrowseForm ID="bfBrowssTriggers" runat="server" StyleClass="browseTriggers" ResultsView="'div.tri'">
    <div class="ts br t">
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>

    <div class="tri">
        <Triggers:TriggersList runat="server" id="Triggers_TriggersList" />
    </div>
        
    <div class="ts br b">    
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>
</eyf:BrowseForm>
