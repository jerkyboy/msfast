<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTest.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.UpdateOrCreateTest" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<eyf:ScriptLiteral ID="scUpdateOrCreateTest" runat="server">
$(function(){
    $("form.afUpdateOrCreateTest").bind("formIndicator",function(e, r, k, v){
        var o = $.evalJSON(v);
        if(k == "test_created" && o){
            $(document).trigger("testCreated",o);
        }else{
            $(document).trigger("testUpdated",o);
        }
    });
});
</eyf:ScriptLiteral>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afUpdateOrCreateTest" runat="server" styleClass="afUpdateOrCreateTest">
    <table class="frmtbl">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$MSFAResources: Controls.Tests.UpdateOrCreateTest, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.UpdateOrCreateTest, name %>" /></td>
                <td class="fld"><eyf:InputText ID="ltName" name="n" runat="server" /></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.UpdateOrCreateTest, url %>" /></td>
                <td class="fld"><eyf:InputText ID="ltURL" name="u" runat="server" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$MSFAResources: Controls.Tests.UpdateOrCreateTest, save %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTestID" runat="server" name="t" />
</eyf:AsyncForm>
</eyf:Box>
<eyf:Box runat="server" ID="bxUpdateCollectorsConfiguration">
    <Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
</eyf:Box>