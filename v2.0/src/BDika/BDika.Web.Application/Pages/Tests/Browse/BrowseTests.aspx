<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="BrowseTests.aspx.cs" Inherits="BDika.Web.Application.Pages.Tests.Browse.BrowseTests" %>
<%@ Register TagPrefix="Tests" TagName="TestsPaging" Src="~/Controls/Tests/Browse/TestsPaging.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
    <eyf:StyleLiteral ID="slBrowseTests" runat="server">
        a.addtest{font-weight:bold;z-index:10;display:block;padding:10px}
        a.addtest .ico{float:left;margin:-2px 6px 0 0;}
    </eyf:StyleLiteral>
    <eyf:ScriptLiteral ID="scBrowseTests" runat="server">
    $(function(){
        $(document).bind("editTestClicked",function(e, t){
            $.loadControlPopup("<%=BDika.Web.Application.ControlLoaders.Tests.UpdateTestPagesControlLoader.GetURL(new BDika.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid),"<asp:Literal runat="server" Text="<%$BDikaResources: Pages.Tests.Browse.BrowseTests, edit %>" />");
        }).bind("testURLClicked",function(e, t){
            top.location = "<%=BDika.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(new BDika.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid);
        }).bind("testNameClicked",function(e, t){
            top.location = "<%=BDika.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(new BDika.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid);
        });
    });
    function createTest(){
        $.loadControlPopup("<%=BDika.Web.Application.ControlLoaders.Tests.UpdateOrCreateTestControlLoader.GetURL()%>","<asp:Literal runat="server" Text="<%$BDikaResources: Pages.Tests.Browse.BrowseTests, create %>" />");
    }
    </eyf:ScriptLiteral>
    <eyf:Box runat="server">
    <a href="#" onclick="createTest()" class="addtest"><span class="ico ico-add-test"></span><asp:Literal runat="server" Text="<%$BDikaResources: Pages.Tests.Browse.BrowseTests, create %>" /></a>
    <Tests:TestsPaging ID="Tests_TestsPaging" runat="server" />
    </eyf:Box>
</asp:Content>