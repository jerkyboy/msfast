<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrCreateTest.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.UpdateOrCreateTest" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<eyf:AsyncForm ID="afUpdateOrCreateTest" runat="server" styleClass="afUpdateOrCreateTest">
    <table class="frmtbl">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateOrCreateTest, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.UpdateOrCreateTest, name %>" /></td>
                <td class="fld"><eyf:InputText ID="ltName" name="n" runat="server" /></td>
            </tr>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.UpdateOrCreateTest, url %>" /></td>
                <td class="fld"><eyf:InputText ID="ltURL" name="u" runat="server" /></td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$BDikaResources: Controls.Tests.UpdateOrCreateTest, save %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTestID" runat="server" name="t" />
</eyf:AsyncForm>
<Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
