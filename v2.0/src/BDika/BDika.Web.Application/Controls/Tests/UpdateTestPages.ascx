<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateTestPages.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.UpdateTestPages" %>
<%@ Register TagPrefix="Tests" TagName="UpdateSelectedTesterTypes" Src="~/Controls/Tests/UpdateSelectedTesterTypes.ascx" %>
<%@ Register TagPrefix="Tests" TagName="UpdateOrCreateTest" Src="~/Controls/Tests/UpdateOrCreateTest.ascx" %>
<eyf:StaticFileInclude ID="JTabsStaticFileInclude" runat="server" FileId="JTabs" />
<eyf:ScriptLiteral ID="scUpdateTestPages" runat="server">
$(function(){
    $("div.testTbs").tabControl({tabs:$("div.testTbs a.testTbs"), callback : function(id){
            $("div.pg").hide();
            $("div.pg." + id).show();
        }
    });
 });
</eyf:ScriptLiteral>
<eyf:StyleLiteral ID="slUpdateTestPages" runat="server">
.testInfo{min-width:600px;min-height:500px;}
.usrPgTestBoxes{display:none;}
.testInfo .pg{clear:both}
</eyf:StyleLiteral>
<asp:PlaceHolder id="phTest" runat="server">
    <div class="testInfo">
        <div class="testTbs tabs">
            <a href="#" class="testTbs selected" bt="testPgInfo"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateTestPages, testinfo %>" /></a>
            <a href="#" class="testTbs" bt="usrPgTestBoxes"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateTestPages, testertypes %>" /></a>
        </div>
        <div class="pg testPgInfo">
            <Tests:UpdateOrCreateTest runat="server" id="Tests_UpdateOrCreateTest"/>
        </div>
        <div class="pg usrPgTestBoxes">
            <Tests:UpdateSelectedTesterTypes ID="Tests_UpdateSelectedTesterTypes" runat="server" />
        </div>
    </div>   
</asp:PlaceHolder>
<asp:PlaceHolder id="phInvalidTest" runat="server">
    <asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateTestPages, invalidtest %>" />
</asp:PlaceHolder>
