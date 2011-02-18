<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeleteTriggerForm.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.DeleteTriggerForm" %>
<asp:PlaceHolder ID="phUnexpectedError" runat="server">
    <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, Unexpected %>" />
</asp:PlaceHolder>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afDeleteTriggerFrm" runat="server" styleClass="afDeleteTrigger">
    <eyf:ScriptLiteral ID="scDeleteTriggerFrm" runat="server">
    $(function(){
        $("form.afDeleteTrigger .cancelDelete").click(function(e){
            e.preventDefault();
            $(document).trigger("deleteCanceled");
        });
        $("form.afDeleteTrigger").bind("formIndicator", function(e, r, k, v){           
            if(k == "trigger_deleted") $(document).trigger("triggerDeleted",$.evalJSON(v));
        });        
    });
    </eyf:ScriptLiteral>
    <table class="frmtbl">
        <thead>
            <tr>
                <th><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, t1 %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr><td style="padding:20px 20px 10px 20px"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, p1 %>" /> <strong><asp:Literal ID="ltTriggerName" runat="server" /></strong>?</td></tr>
            <tr><td style="padding:0 20px 20px 20px"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, p2 %>" /></td></tr>
            <tr>
                <td class="btnbar">
                    <eyf:InputSubmit ID="isSubmit" runat="server" value="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, Delete %>"/>
                    <eyf:InputButton runat="server" styleclass="cancelDelete btn2" value="<%$MSFAResources: Controls.Triggers.DeleteTriggerForm, Cancel %>" />
                </td>
            </tr>
        </tbody>
    </table>                      
    
    <eyf:InputHidden ID="ihTrid" name="t" runat="server"/>
</eyf:AsyncForm>
</eyf:Box>






