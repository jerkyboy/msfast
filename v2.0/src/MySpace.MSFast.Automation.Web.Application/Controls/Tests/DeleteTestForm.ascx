<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DeleteTestForm.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.DeleteTestForm" %>
<asp:PlaceHolder ID="phUnexpectedError" runat="server">
    <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.DeleteTestForm, Unexpected %>" />
</asp:PlaceHolder>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afDeleteTestFrm" runat="server" styleClass="afDeleteTest">
    <eyf:ScriptLiteral ID="scDeleteTestFrm" runat="server">
    $(function(){
        $("form.afDeleteTest .cancelDelete").click(function(e){
            e.preventDefault();
            $(document).trigger("deleteCanceled");
        });
        $("form.afDeleteTest").bind("formIndicator", function(e, r, k, v){           
            if(k == "test_deleted") $(document).trigger("testDeleted",$.evalJSON(v));
        });        
    });
    </eyf:ScriptLiteral>
    <table class="frmtbl">
        <thead>
            <tr>
                <th><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.DeleteTestForm, t1 %>" /></th>
            </tr>
        </thead>
        <tbody>
            <tr><td style="padding:20px 20px 10px 20px"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.DeleteTestForm, p1 %>" /> <strong><asp:Literal ID="ltTestName" runat="server" /></strong>?</td></tr>
            <tr><td style="padding:0 20px 20px 20px"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.DeleteTestForm, p2 %>" /></td></tr>
            <tr>
                <td class="btnbar">
                    <eyf:InputSubmit ID="isSubmit" runat="server" value="<%$MSFAResources: Controls.Tests.DeleteTestForm, Delete %>"/>
                    <eyf:InputButton runat="server" styleclass="cancelDelete btn2" value="<%$MSFAResources: Controls.Tests.DeleteTestForm, Cancel %>" />
                </td>
            </tr>
        </tbody>
    </table>                      
    
    <eyf:InputHidden ID="ihTid" name="t" runat="server"/>
</eyf:AsyncForm>
</eyf:Box>













