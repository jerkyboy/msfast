<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TriggersDisplayAndFilter.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.TriggersDisplayAndFilter" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersPaging" Src="~/Controls/Triggers/Browse/TriggersPaging.ascx" %>
<eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
<eyf:ScriptLiteral ID="scTriggersDisplayAndFilter" runat="server">
$(function(){
    $("form.afTriggersDisplayAndFilter").bind("beforeSerialize", function(e,r){
        r.kill = true;
        var t = $("div.tripaging form.brfrm.browseTriggers");
        t.setBrowseArg("e",$(this).find("select[name=e]").fieldValue());
        t.setBrowseArg("n",$(this).find("input[name=n]").val());
        t.setBrowseArg("t",$(this).find("select[name=t]").fieldValue());
        t.resetBrowseParameters({index:0, popTotal:true});
        t.reloadBrowseResults();    
    });
    $(document).bind("triggerCreated",function(e,t){
        hidePopup();
        location.reload(true);
    }).bind("triggerUpdated",function(e,t){
        hidePopup();
        location.reload(true);
    });
});
function createTrigger(){
    $.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Triggers.UpdateOrCreateTriggerControlLoader.GetURL()%>","<asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, create %>" />");
}
</eyf:ScriptLiteral>
<eyf:StyleLiteral id="slTriggersDisplayAndFilter" runat="server" >
    a.addtrigger{font-weight:bold;z-index:10;display:block;padding:10px}
    a.addtrigger .ico{float:left;margin:-2px 6px 0 0;}
</eyf:StyleLiteral>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afTriggersDisplayAndFilter" runat="server" styleClass="afTriggersDisplayAndFilter">
    <table class="frmtbl resultsfilter">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, enabled %>" /></td>
                <td class="fld"><eyf:InputSelect ID="isEnabled" runat="server" name="e" /></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, type %>" /></td>
                <td class="fld"><eyf:InputSelect ID="isType" runat="server" name="t"/></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, name %>" /></td>
                <td class="fld"><eyf:InputText ID="itTriggerName" runat="server" name="n" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, filter %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
</eyf:AsyncForm>
</eyf:Box>

<eyf:Box runat="server">
    <a href="#" onclick="createTrigger()" class="addtrigger"><span class="ico ico-add-trigger"></span><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.TriggersDisplayAndFilter, create %>" /></a>
</eyf:Box>

<div class="tripaging">
    <Triggers:TriggersPaging ID="Triggers_TriggersPaging" runat="server" />
</div>
