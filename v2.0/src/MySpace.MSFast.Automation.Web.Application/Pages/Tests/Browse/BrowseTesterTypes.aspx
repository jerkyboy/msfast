<%@ Page Language="C#" MasterPageFile="~/Masters/MainWithSideMenu.Master" AutoEventWireup="true" CodeBehind="BrowseTesterTypes.aspx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Pages.Tests.Browse.BrowseTesterTypes" %>
<%@ Register TagPrefix="Tests" TagName="TesterTypesPaging" Src="~/Controls/Tests/Browse/TesterTypesPaging.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">    
    <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
    <eyf:StyleLiteral ID="slBrowseTesterTypes" runat="server">
        a.addtestertype{font-weight:bold;z-index:10;display:block;padding:10px}
        a.addtestertype .ico{float:left;margin:-2px 6px 0 0;}
    </eyf:StyleLiteral>
    <eyf:ScriptLiteral ID="scBrowseTesterTypes" runat="server">
    $(function(){
        $(document).bind("testerTypeCreated",function(e,t){
            hidePopup();
            location.reload(true);
        });
    });
    function createTesterType(){
        $.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTesterTypeControlLoader.GetURL()%>","<asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTesterTypes, create %>" />");
    }
    </eyf:ScriptLiteral>
    <eyf:Box runat="server">
        <a href="#" onclick="createTesterType()" class="addtestertype"><span class="ico ico-add-test-box"></span><asp:Literal runat="server" Text="<%$MSFAResources: Pages.Tests.Browse.BrowseTesterTypes, create %>" /></a>
    </eyf:Box>
    <Tests:TesterTypesPaging ID="Tests_TesterTypesPaging" runat="server" />
</asp:Content>







