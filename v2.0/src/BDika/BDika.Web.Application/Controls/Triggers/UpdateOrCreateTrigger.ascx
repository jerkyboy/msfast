<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTrigger.ascx.cs" Inherits="BDika.Web.Application.Controls.Triggers.UpdateOrCreateTrigger" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<eyf:AsyncForm ID="afUpdateOrCreateTrigger" runat="server" styleClass="afUpdateOrCreateTrigger">
    <table class="frmtbl">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Triggers.UpdateOrCreateTrigger, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.UpdateOrCreateTrigger, name %>" /></td>
                <td class="fld"><eyf:InputText ID="ltName" name="n" runat="server" /></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.UpdateOrCreateTrigger, triggertype %>" /></td>
                <td class="fld"><eyf:InputSelect ID="isTriggerType" name="t" runat="server" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$BDikaResources: Controls.Triggers.UpdateOrCreateTrigger, save %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTriggerID" runat="server" name="trid" />
</eyf:AsyncForm>
<Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
