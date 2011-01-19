<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TriggersDisplayAndFilter.ascx.cs" Inherits="BDika.Web.Application.Controls.Triggers.TriggersDisplayAndFilter" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersPaging" Src="~/Controls/Triggers/Browse/TriggersPaging.ascx" %>
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
});

</eyf:ScriptLiteral>
<eyf:AsyncForm ID="afTriggersDisplayAndFilter" runat="server" styleClass="afTriggersDisplayAndFilter">
    <table class="frmtbl resultsfilter">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Triggers.TriggersDisplayAndFilter, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.TriggersDisplayAndFilter, enabled %>" /></td>
                <td class="fld"><eyf:InputSelect ID="isEnabled" runat="server" name="e" /></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.TriggersDisplayAndFilter, type %>" /></td>
                <td class="fld"><eyf:InputSelect ID="isType" runat="server" name="t"/></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.TriggersDisplayAndFilter, name %>" /></td>
                <td class="fld"><eyf:InputText ID="itTriggerName" runat="server" name="n" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$BDikaResources: Controls.Triggers.TriggersDisplayAndFilter, filter %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
</eyf:AsyncForm>
<div class="tripaging">
    <Triggers:TriggersPaging ID="Triggers_TriggersPaging" runat="server" />
</div>