<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="TriggersPaging.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.Browse.TriggersPaging" %>
<%@ Register TagPrefix="Triggers" TagName="TriggersList" Src="~/Controls/Triggers/Browse/TriggersList.ascx" %>
<eyf:BrowseForm ID="bfBrowssTriggers" runat="server" StyleClass="browseTriggers" ResultsView="'tbody.tri'">
    <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
    <eyf:ScriptLiteral ID="scTriggersList" runat="server">
    function edittrigger(tid){$.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Triggers.UpdateTriggerPagesControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Triggers.TriggerID(222))%>".replace(/222/g,tid),"<asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, edit %>" />");}
    function removetrigger(tid){$.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Triggers.DeleteTriggerFormControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Triggers.TriggerID(222))%>".replace(/222/g,tid),"<asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, remove %>" />");}
    $(function(){
        $(document).bind("deleteCanceled",function(e,t){
            hidePopup();
        }).bind("triggerDeleted",function(e,t){
            hidePopup();
            location.reload(true);
        });
    });
    </eyf:ScriptLiteral>    
    <table class="triggerslst resultstable">
        <thead>
            <tr>
                <th width="45%" class="frst"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, triggername %>" /></th>
                <th width="10%"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, triggertype %>" /></th>
                <th width="15%"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.Browse.TriggersList, lasttriggered %>" /></th>
                <th width="30%">&nbsp;</th>
            </tr>
            <tr class="ts br t">
                <th colspan="5" class="frst rdr">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </th>
            </tr>
        </thead>
        <tbody class="tri">
            <Triggers:TriggersList runat="server" id="Triggers_TriggersList" />
        </tbody>
        <tfoot>
            <tr class="ts br b">
                <td colspan="5" class="rdr frst">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </td>
            </tr>
        </tfoot>
    </table>        
</eyf:BrowseForm>
