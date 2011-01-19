<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTesterType.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.UpdateOrCreateTesterType" %>
<eyf:AsyncForm ID="afUpdateOrCreateTesterType" runat="server" styleClass="afUpdateOrCreateTesterType">
    <table class="frmtbl">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateOrCreateTesterType, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.UpdateOrCreateTesterType, name %>" /></td>
                <td class="fld"><eyf:InputText ID="ltName" name="n" runat="server" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$BDikaResources: Controls.Tests.UpdateOrCreateTesterType, save %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTesterTypeID" runat="server" name="t" />
</eyf:AsyncForm>
