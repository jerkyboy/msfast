<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TriggersList.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.Browse.TriggersList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <tr>
        <td class="nores" colspan="5">
            <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, noresults %>" />
        </td>
    </tr>
</asp:PlaceHolder>
<asp:Repeater ID="rptTriggersList" runat="server">
    <ItemTemplate>
        <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>" onmouseover="$(this).parents('table:first').find('tr.over').removeClass('over');$(this).addClass('over');" onmouseout="$(this).removeClass('over');">
            <td><eyf:Href runat="server" ID="hrefTriggerName" /></td>
            <td><eyf:Href runat="server" ID="hrefTriggerType" /></td>
            <td><eyf:Href runat="server" ID="hrefLastTriggered" /></td>
            <td class="rdac">
                <a href="#"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, trigger %>" /></a>
                <eyf:Href runat="server" ID="hrefEditTrigger" /></a>
                <eyf:Href runat="server" ID="hrefRemoveTrigger" /></a>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
