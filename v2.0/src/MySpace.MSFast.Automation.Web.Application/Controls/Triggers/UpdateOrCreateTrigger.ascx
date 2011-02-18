<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTrigger.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.UpdateOrCreateTrigger" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<eyf:ScriptLiteral ID="scUpdateOrCreateTrigger" runat="server">
$(function(){
    $("form.afUpdateOrCreateTrigger").bind("formIndicator",function(e, r, k, v){
        var o = $.evalJSON(v);
        if(k == "trigger_created" && o){
            $(document).trigger("triggerCreated",o);
        }else{
            $(document).trigger("triggerUpdated",o);
        }
    }).find("select[name=t]").change(function(){
        if($(this).fieldValue()+"" == "1"){
            $(this).parents("form:first").find("input[name=m]").removeAttr("disabled").focus();
        }else{
            $(this).parents("form:first").find("input[name=m]").attr("disabled","false");
        }
    });
});
</eyf:ScriptLiteral>
<eyf:Box runat="server">
    <eyf:AsyncForm ID="afUpdateOrCreateTrigger" runat="server" styleClass="afUpdateOrCreateTrigger">
        <table class="frmtbl">
            <thead>
                <tr>
                    <th colspan="2"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Triggers.UpdateOrCreateTrigger, title %>" /></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.UpdateOrCreateTrigger, name %>" /></td>
                    <td class="fld"><eyf:InputText ID="itName" name="n" runat="server" /></td>
                </tr>
                <tr>
                    <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.UpdateOrCreateTrigger, triggertype %>" /></td>
                    <td class="fld"><eyf:InputSelect ID="isTriggerType" name="t" runat="server" /></td>
                </tr>
                <tr>
                    <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.UpdateOrCreateTrigger, timeout %>" /></td>
                    <td class="fld"><eyf:InputText ID="itTimeout" name="m" runat="server" /></td>
                </tr>
                <tr>
                    <td class="btnbar" colspan="2">
                        <eyf:InputSubmit value="<%$MSFAResources: Controls.Triggers.UpdateOrCreateTrigger, save %>" runat="server"/>
                    </td>
                </tr>
            </tbody>
        </table>
        <eyf:InputHidden ID="ihTriggerID" runat="server" name="trid" />
    </eyf:AsyncForm>
</eyf:Box>
<eyf:Box runat="server" ID="bxUpdateCollectorsConfiguration">
<Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
</eyf:Box>