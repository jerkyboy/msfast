<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateTriggerToTestAndTesterType.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Triggers.UpdateTriggerToTestAndTesterType" %>
<%@ Register TagPrefix="Tests" TagName="TestsList" Src="~/Controls/Tests/Browse/TestsList.ascx" %>
<eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
<eyf:StyleLiteral ID="slUpdateTriggerToTestAndTesterType" runat="server">
.updateTriggerToTestAndTesterType{width:700px;border-spacing:0px;border:0}
td.configuration{display:none;}
div.selectedtestertypes, div.selectedtests{height:350px;overflow:auto;}
.selectedtestertypes div.nores{padding:120px 0;height:109px;border-top:1px solid #CCC}
td.selectedtests,td.selectedtestertypes{width:50%;vertical-align:top;padding:0}
td.selectedtestertypes{padding-right:10px;}
td.selectedtestertypes ul{border-top:1px solid #CCC;list-style:none;margin:0;padding:0;}
td.selectedtestertypes li{cursor:pointer;margin:0;padding:8px;border-bottom:1px solid #CCC}
td.selectedtestertypes li.r1{background:#F6F7FA;}
td.selectedtestertypes li.selected{background:#7583ac;color:#FFF}
.updateTriggerToTestAndTesterType .resultstable th.testslst_header{width:100%;}
.updateTriggerToTestAndTesterType .resultstable th.testslst_selected_header{padding:3px;width:14px}
.updateTriggerToTestAndTesterType .resultstable tbody td{border-width:1px 0 0 0}
</eyf:StyleLiteral>
<eyf:ScriptLiteral ID="scUpdateTriggerToTestAndTesterType" runat="server">
function selecttestertype(who){
    $("input[name=icTestSelected]").removeAttr("checked");
    $(".asUpdateSelectedTests input[name=ttid]").val(who.attr("ttid"));
    who.addClass("selected").siblings("li.selected").removeClass("selected");$("div.selectedtests tbody").loadControl("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.Browse.BrowseTestsForTriggerAndTesterTypeTestsListControlLoader.GetURL(this.TriggerID, new MySpace.MSFast.Automation.Entities.Tests.TesterTypeID(9999))%>".replace(/9999/gi,who.attr("ttid")));
}
function addRemoveTests(ad,rm){    
    var frm = $(".asUpdateSelectedTests");
    frm.find("input[name=rm]").val(rm && rm.length > 0 ? rm.join("|") : "");
    frm.find("input[name=ad]").val(ad && ad.length > 0 ? ad.join("|") : "");
    frm.submit();    
}
function selectDeselectAll(w){
    var arr = [];
    w.parents("td:first").find("div.selectedtests input[type=checkbox]").each(function(){        
        checked = $(this).attr("checked");
        checked = (checked == true || checked == "checked" || checked == "true");        
        if(w.fieldValue() && w.fieldValue() != "" && checked == false){
            arr.push($(this).attr("value"));
            $(this).attr("checked","checked");
        }else if((!w.fieldValue() || w.fieldValue() == "") && checked){
            arr.push($(this).attr("value"));
            $(this).removeAttr("checked");
        }                
    });
    addRemoveTests((w.fieldValue() && w.fieldValue() != "") ? arr : [], (!w.fieldValue() || w.fieldValue() == "") ? arr : []);
}
$(function(){
    $(document).bind("testNameClicked",function(e,o){
        o.sender.parents("table:first").find("tr.selectedrow").removeClass("selectedrow");
        var ch = o.sender.parents("tr:first").addClass("selectedrow").find("input[type=checkbox]");
        if(!ch.fieldValue() || ch.fieldValue() == ""){
            addRemoveTests([o.tid],[]);
            ch.attr("checked","checked");
        }
        $("div.configuration").loadControl("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Collectors.UpdateCollectorsConfigurationControlLoader.GetURL(this.TriggerID, new MySpace.MSFast.Automation.Entities.Tests.TesterTypeID(9999),new MySpace.MSFast.Automation.Entities.Tests.TestID(6666))%>".replace(/6666/gi,o.tid).replace(/9999/gi,$(".asUpdateSelectedTests input[name=ttid]").val()),function(){$("td.configuration").show()});
    }).bind("selectTestClicked",function(e,o){
        if(o.sender.fieldValue() && o.sender.fieldValue() != ""){
            addRemoveTests([o.tid],[]);
        }else{
            addRemoveTests([],[o.tid]);
        }
    });
});
</eyf:ScriptLiteral>
<eyf:AsyncForm id="asUpdateSelectedTests" styleclass="asUpdateSelectedTests" runat="server">
    <eyf:InputHidden id="ihTesterTypeID" name="ttid" runat="server" />
    <eyf:InputHidden id="ihTriggerID" name="trid" runat="server" />
    <eyf:InputHidden id="ihRemove" name="rm" runat="server" />
    <eyf:InputHidden id="ihAdd" name="ad" runat="server" />
</eyf:AsyncForm>
<table class="updateTriggerToTestAndTesterType">
    <tr>
        <td class="selectedtestertypes">
            <eyf:Box runat="server">
                <eyf:Title text="Test Boxes" runat="server" />
            <asp:PlaceHolder id="cphNoResults" runat="server">
                <div class="nores">No Test Boxes Found...</div>
            </asp:PlaceHolder>            
            <asp:Repeater ID="rptTesterTypesRpt" runat="server">
                <HeaderTemplate>                
                <div class="selectedtestertypes">
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                        <asp:Literal runat="server" id="ltLI" /><asp:Literal runat="server" id="ltTesterTypeName" /></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </div>
                </FooterTemplate>
            </asp:Repeater>
            </eyf:Box>
        </td>        
        <td class="selectedtests">
            <eyf:Box runat="server">
                <eyf:Title runat="server">
                <input type="checkbox" name="icTestSelected" style="margin:0 5px 0 0" value="ON" onclick="selectDeselectAll($(this));" />
                Selected Tests
                </eyf:Title>
                <div class="selectedtests">
                    <table class="testslst resultstable">
                        <tbody></tbody>
                    </table>
                </div>
            </eyf:Box>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="configuration">
        <eyf:Box runat="server"><div class="configuration"></div></eyf:Box></td>
    </tr>
</table>














