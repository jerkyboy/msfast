<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsPaging.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.Browse.ResultsPaging" %>
<%@ Register TagPrefix="Results" TagName="ResultsList" Src="~/Controls/Results/Browse/ResultsList.ascx" %>
<eyf:BrowseForm ID="bfBrowseResults" runat="server" StyleClass="browseResults" ResultsView="'div.trr'">
    <div class="ts br t">
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>

    <div class="trr">
        <Results:ResultsList runat="server" id="Results_ResultsList" />
    </div>
        
    <div class="ts br b">    
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>
</eyf:BrowseForm>
