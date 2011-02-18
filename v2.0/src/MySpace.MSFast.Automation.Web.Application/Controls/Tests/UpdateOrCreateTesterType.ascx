<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTesterType.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.UpdateOrCreateTesterType" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<eyf:ScriptLiteral ID="scUpdateOrCreateTest" runat="server">
$(function(){
    $("form.afUpdateOrCreateTesterType").bind("formIndicator",function(e, r, k, v){
        var o = $.evalJSON(v);
        if(k == "tester_type_created" && o){
            $(document).trigger("testerTypeCreated",o);
        }else{
            $(document).trigger("testerTypeUpdated",o);
        }
    });
});
</eyf:ScriptLiteral>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afUpdateOrCreateTesterType" runat="server" styleClass="afUpdateOrCreateTesterType">
    <table class="frmtbl">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Tests.UpdateOrCreateTesterType, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.UpdateOrCreateTesterType, name %>" /></td>
                <td class="fld"><eyf:InputText ID="ltName" name="n" runat="server" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$MSFAResources: Controls.Tests.UpdateOrCreateTesterType, save %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTesterTypeID" runat="server" name="t" />
</eyf:AsyncForm>
</eyf:Box>
<eyf:Box runat="server" ID="bxUpdateCollectorsConfiguration">
<Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
</eyf:Box>