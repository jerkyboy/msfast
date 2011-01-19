<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TesterTypesPaging.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.Browse.TesterTypesPaging" %>
<%@ Register TagPrefix="Tests" TagName="TesterTypesList" Src="~/Controls/Tests/Browse/TesterTypesList.ascx" %>
<eyf:BrowseForm ID="bfBrowseTesterTypes" runat="server" StyleClass="browseTesterTypes" ResultsView="'div.ttr'">
    <div class="ts br t">
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>

    <div class="ttr">
        <Tests:TesterTypesList runat="server" id="Tests_TesterTypesList" />
    </div>
        
    <div class="ts br b">    
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>
</eyf:BrowseForm>
