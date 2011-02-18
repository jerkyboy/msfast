<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="UpdateTriggerPages.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.UpdateTriggerPages" %>
<%@ Register TagPrefix="Triggers" TagName="UpdateOrCreateTrigger" Src="~/Controls/Triggers/UpdateOrCreateTrigger.ascx" %>
<%@ Register TagPrefix="Triggers" TagName="UpdateTriggerToTestAndTesterType" Src="~/Controls/Triggers/UpdateTriggerToTestAndTesterType.ascx" %>
<eyf:StaticFileInclude ID="JTabsStaticFileInclude" runat="server" FileId="JTabs" />
<eyf:ScriptLiteral ID="scUpdateTriggerPages" runat="server">
$(function(){
    $("div.triggerTbs").tabControl({tabs:$("div.triggerTbs a.triggerTbs"), callback : function(id){
            $("div.pg").hide();
            $("div.pg." + id).show();
        }
    });
 });
</eyf:ScriptLiteral>
<eyf:StyleLiteral ID="slUpdateTriggerPages" runat="server">
.triggerInfo{min-width:720px;min-height:300px;}
.triggerPgTestBoxes{display:none;}
.triggerInfo .pg{clear:both;padding:10px}
</eyf:StyleLiteral>
<asp:PlaceHolder id="phTrigger" runat="server">
    
    <div class="triggerInfo">
        <div class="triggerTbs tabs">
            <a href="#" class="triggerTbs selected" bt="triggerPgInfo"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Triggers.UpdateTriggerPages, triggerinfo %>" /></a>
            <a href="#" class="triggerTbs" bt="triggerPgTestBoxes"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Triggers.UpdateTriggerPages, selecttests %>" /></a>
        </div>
        <div class="pg triggerPgInfo">
            <Triggers:UpdateOrCreateTrigger runat="server" id="Triggers_UpdateOrCreateTrigger"/>
        </div>
        <div class="pg triggerPgTestBoxes">
            <Triggers:UpdateTriggerToTestAndTesterType ID="Triggers_UpdateTriggerToTestAndTesterType" runat="server" />
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder id="phInvalidTrigger" runat="server">
    <asp:Literal runat="server" text="<%$MSFAResources: Controls.Triggers.UpdateTriggerPages, invalidtrigger %>" />
</asp:PlaceHolder>
