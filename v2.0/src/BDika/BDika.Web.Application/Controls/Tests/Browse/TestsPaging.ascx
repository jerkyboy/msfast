<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestsPaging.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.Browse.TestsPaging" %>
<%@ Register TagPrefix="Tests" TagName="TestsList" Src="~/Controls/Tests/Browse/TestsList.ascx" %>
<eyf:StyleLiteral runat="server">
<%if (!this.ShowHeaders){%>form#<%=FORMID%> thead{display:none;}<%} %>
<%if (!this.ShowEditLink){%>form#<%=FORMID%> .testslst_edit{display:none;}<%} %>
<%if (!this.ShowTestURL){%>form#<%=FORMID%> .testslst_url{display:none;}<%} %>
<%if (!this.ShowRemoveLink){%>form#<%=FORMID%> .testslst_remove{display:none;}<%} %>
<%if (!this.ShowAddLink){%>form#<%=FORMID%> .testslst_add{display:none;}<%} %>
</eyf:StyleLiteral>
<eyf:BrowseForm ID="bfBrowseTests" runat="server" StyleClass="browseTests" ResultsView="'div.tsr'">
    <div class="ts br t">
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>
    <div class="tsr">
        <Tests:TestsList runat="server" id="Tests_TestsList" />
    </div>
    <div class="ts br b">    
        <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
        <eyf:BrowsePagesHref runat="server" />
        <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
    </div>
</eyf:BrowseForm>
