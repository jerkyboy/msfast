<%@ Page Language="C#" MasterPageFile="~/Masters/MainWithSideMenu.Master" AutoEventWireup="true" CodeBehind="BrowseTests.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Pages.Tests.Browse.BrowseTests" %>
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
            $.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTestControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid),"<asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTests, edit %>" />");
        }).bind("removeTestClicked",function(e, t){
            $.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.DeleteTestFormControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid),"<asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTests, delete %>" />");        
        }).bind("testURLClicked",function(e, t){
            top.location = "<%=MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid);
        }).bind("testNameClicked",function(e, t){
            top.location = "<%=MySpace.MSFast.Automation.Web.Application.Pages.Results.Browse.BrowseResults.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TestID(222))%>".replace(/222/g, t.tid);
        }).bind("testCreated",function(e,t){
            hidePopup();
            location.reload(true);
        }).bind("testUpdated",function(e,t){
            hidePopup();
            location.reload(true);
        }).bind("deleteCanceled",function(e,t){
            hidePopup();
        }).bind("testDeleted",function(e,t){
            hidePopup();
            location.reload(true);
        });
    });
    function createTest(){
        $.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTestControlLoader.GetURL()%>","<asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTests, create %>" />");
    }
    
    </eyf:ScriptLiteral>
    <eyf:Box runat="server">
        <a href="#" onclick="createTest()" class="addtest"><span class="ico ico-add-test"></span><asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTests, create %>" /></a>
    </eyf:Box>
    <Tests:TestsPaging ID="Tests_TestsPaging" runat="server" />
</asp:Content>