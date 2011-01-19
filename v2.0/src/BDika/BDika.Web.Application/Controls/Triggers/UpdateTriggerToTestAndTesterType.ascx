<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateTriggerToTestAndTesterType.ascx.cs" Inherits="BDika.Web.Application.Controls.Triggers.UpdateTriggerToTestAndTesterType" %>
<%@ Register TagPrefix="Tests" TagName="TestsPaging" Src="~/Controls/Tests/Browse/TestsPaging.ascx" %>
<%@ Register TagPrefix="Tests" TagName="UpdateSelectedTesterTypes" Src="~/Controls/Tests/UpdateSelectedTesterTypes.ascx" %>
<eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
<eyf:StyleLiteral ID="slUpdateTriggerToTestAndTesterType" runat="server">
table.updateTriggerToTestAndTesterType{width:100%;}
table.updateTriggerToTestAndTesterType td.selectedtestertypes,
table.updateTriggerToTestAndTesterType td.selectedtests{width:50%;vertical-align:top;}
</eyf:StyleLiteral>
<eyf:ScriptLiteral ID="scUpdateTriggerToTestAndTesterType" runat="server">
$(function(){
    function reloadTestBoxes(tid){
       $("td.selectedtestertypes").loadControl("<%=BDika.Web.Application.ControlLoaders.Tests.UpdateSelectedTesterTypesControlLoader.GetURL(this.TriggerID,new BDika.Entities.Tests.TestID(9999))%>".replace(/9999/gi,tid));
    }
    $(document).bind("testNameClicked",function(e,o){
        if(o.sender.parents("td.selectedtests").length > 0){
            o.sender.parents("table").find("tr.selectedrow").removeClass("selectedrow");
            o.sender.parents("tr").addClass("selectedrow");
            reloadTestBoxes(o.tid);
        }
    }).bind("addTestClicked",function(e,o){
        if(o.sender.parents("td.availabletests").length > 0){
            var tb = $(".selectedtests .testslst.resultstable tbody");
            tb.find("tr.selectedrow").removeClass("selectedrow");
            var t = o.sender.parents("tr:first").remove().removeClass().addClass("r" + (($(".selectedtests .testslst.resultstable tbody tr:first").attr("class").indexOf("r1") == -1) ? "1" : "2" )).addClass("selectedrow");
            tb.prepend(t);         
            reloadTestBoxes(o.tid);
        }
    });
});
</eyf:ScriptLiteral>
<table class="updateTriggerToTestAndTesterType">
    <tr>
        <td class="selectedtests"><Tests:TestsPaging ID="Tests_TestsPaging_SelectedTests" runat="server" /></td>
        <td class="selectedtestertypes"><Tests:UpdateSelectedTesterTypes ID="Tests_UpdateSelectedTesterTypes" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="2" class="availabletests"><Tests:TestsPaging ID="Tests_TestsPaging_AllTests" runat="server" /></td>
    </tr>
</table>
