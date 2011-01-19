<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TriggersList.ascx.cs" Inherits="BDika.Web.Application.Controls.Triggers.Browse.TriggersList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, noresults %>" />
    </div>
</asp:PlaceHolder>
<asp:Repeater ID="rptTriggersList" runat="server">
    <HeaderTemplate>
        <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
        <eyf:ScriptLiteral ID="scTriggersList" runat="server">
        function edittrigger(tid){$.loadControlPopup("<%=BDika.Web.Application.ControlLoaders.Triggers.UpdateTriggerPagesControlLoader.GetURL(new BDika.Entities.Triggers.TriggerID(222))%>".replace(/222/g,tid),"<asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, edit %>" />");}
        </eyf:ScriptLiteral>    
        <table class="triggerslst resultstable">
            <thead>
                <tr>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, triggername %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, triggertype %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, lasttriggered %>" /></th>
                    <th>&nbsp;</th>
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
                <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>">
                    <td><eyf:Href runat="server" ID="hrefTriggerName" /></td>
                    <td><eyf:Href runat="server" ID="hrefTriggerType" /></td>
                    <td><eyf:Href runat="server" ID="hrefLastTriggered" /></td>
                    <td><a href="#"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Triggers.Browse.TriggersList, trigger %>" /></a></td>
                    <td><eyf:Href runat="server" ID="hrefEditTrigger" /></a></td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>
