<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateSelectedTesterTypes.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.UpdateSelectedTesterTypes" %>
<eyf:ScriptLiteral ID="scUpdateSelectedTesterTypes" runat="server">
$(function(){
    $("form.afUpdateSelectedTesterTypes input[type=checkbox]").click(function(){
        var f = $(this).parents("form");
        f.find("input[name=ttid]").val($(this).attr("value"));
        f.find("input[name=state]").val(($(this).attr("value") == $(this).fieldValue()) ? "true" : "false");
        f.submit();
    });
});
</eyf:ScriptLiteral>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.UpdateSelectedTesterTypes, noresults %>" />
    </div>
</asp:PlaceHolder>
<eyf:AsyncForm ID="afUpdateSelectedTesterTypes" runat="server" styleClass="afUpdateSelectedTesterTypes">
    <asp:Repeater ID="rptTesterTypesRpt" runat="server">
        <HeaderTemplate>
            <table>
                <thead>
                    <tr>
                        <th><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.UpdateSelectedTesterTypes, title %>" /></th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><eyf:InputCheckbox ID="icSelectedTesterType" name="strid" runat="server" /> <asp:Literal runat="server" id="ltTesterTypeName" /></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
                </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    <eyf:InputHidden ID="ihStatus" runat="server" name="state" />
    <eyf:InputHidden ID="ihTesterTypeID" runat="server" name="ttid" />
    <eyf:InputHidden ID="ihTriggerID" runat="server" name="trid" />
    <eyf:InputHidden ID="ihTestID" runat="server" name="tid" />
</eyf:AsyncForm>
