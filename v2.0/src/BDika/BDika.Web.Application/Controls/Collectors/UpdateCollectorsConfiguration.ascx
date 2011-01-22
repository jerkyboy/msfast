<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateCollectorsConfiguration.ascx.cs" Inherits="BDika.Web.Application.Controls.Collectors.UpdateCollectorsConfiguration" %>
<eyf:StyleLiteral ID="slUpdateCollectorsConfiguration" runat="server">
table.updateExecConf tr.template,
table.updateExecConf tr td.td1 span,
table.updateExecConf tr.newval td.td4 span{display:none;}
table.updateExecConf tr.override td.td4 span,
table.updateExecConf tr.newval td.td1 span{display:block;}
table.updateExecConf tr.override td.td2 input,
table.updateExecConf tr.inherit input{color:#ccc;} 
table.updateExecConf tr.override input{color:#333;}
.addvar{position:relative;padding:10px 40px;display:block}
</eyf:StyleLiteral>
<eyf:ScriptLiteral ID="scUpdateCollectorsConfiguration" runat="server">
function saveAttr(tr){
    var v = tr.find("input[name=v]").val();
    var k = tr.find("input[name=k]").val();
    if(k == "") 
        return;
    if(tr.attr("latestval") != v){
        var f = $(".afUpdateCollectorsConfiguration");
        f.find("input[name=k]").val(k);
        f.find("input[name=v]").val(v);
        f.submit();
    }
    tr.attr("latestval",v);
}
function removeAttr(tr){
    var k = tr.find("input[name=k]").val();
    var f = $(".afRemoveCollectorsConfiguration");
    f.find("input[name=k]").val(k);
    f.submit();
}
function addnewarg(){
    $(".updateExecConf tr:last").before(bindrows($(".updateExecConf tr.template").clone().removeClass("template").attr("lastval","").attr("originalval","")));
}
function bindrows(t){
    t.find("input[type=checkbox]").click(function(e){
        var tr = $(this).parents("tr:first");
        if(tr.hasClass("override")){
            tr.removeClass().addClass("inherit").find("input[name=v]").val(tr.attr("originalval"));
            removeAttr(tr);
        }else{
            tr.removeClass().addClass("override").find("input[name=v]").focus();
        }
    });
    t.find("input[name=v]").click(function(){
        var tr = $(this).parents("tr:first");
        if(tr.hasClass("inherit")){
            tr.removeClass().addClass("override");
            tr.find("input[type=checkbox]").attr("checked","checked");
            $(this).focus();
        }
    }).blur(function(){
        var tr = $(this).parents("tr:first");
        saveAttr(tr);
    });

    return t;
}
$(function(){
    bindrows($("table.updateExecConf"));
});
</eyf:ScriptLiteral>

<eyf:AsyncForm ID="afUpdateCollectorsConfiguration" styleclass="afUpdateCollectorsConfiguration" runat="server">
    <eyf:InputHidden ID="ihTestID_Update" name="tid" runat="server" />
    <eyf:InputHidden ID="ihTesterTypeID_Update" name="ttid" runat="server" />
    <eyf:InputHidden ID="ihTriggerID_Update" name="trid" runat="server" />
    <eyf:InputHidden ID="ihKey_Update" name="k" runat="server" />
    <eyf:InputHidden ID="ihVal_Update" name="v" runat="server" />
</eyf:AsyncForm>

<eyf:AsyncForm ID="afRemoveCollectorsConfiguration" styleclass="afRemoveCollectorsConfiguration" runat="server">
    <eyf:InputHidden ID="ihTestID_Remove" name="tid" runat="server" />
    <eyf:InputHidden ID="ihTesterTypeID_Remove" name="ttid" runat="server" />
    <eyf:InputHidden ID="ihTriggerID_Remove" name="trid" runat="server" />
    <eyf:InputHidden ID="ihKey_Remove" name="k" runat="server" />
</eyf:AsyncForm>

<asp:Repeater ID="rptConfiguration" runat="server">
    <HeaderTemplate>
        <table class="frmtbl updateExecConf">
        <thead>
            <tr>
                <th colspan="4"><span class="ico ico-variable"></span>Variables</th>
            </tr>
        </thead>
        <tbody>
    </HeaderTemplate>
    <ItemTemplate>
        <asp:Literal ID="ltTR" runat="server" />
            <td class="td1"><a href="#" onclick="var t=$(this).parents('tr:first');removeAttr(t);t.remove();"><span class="ico ico-delete-variable"></span></a></td>
            <td class="td2"><eyf:InputText ID="itKey" runat="server" name="k" /></td>
            <td class="td3"><eyf:InputText ID="itVal" runat="server" name="v" /></td>
            <td class="td4"><span><eyf:InputCheckbox ID="icInherit" runat="server"/> Override</span></td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        <tr class="template newval">
            <td class="td1"><span><a href="#" onclick="var t=$(this).parents('tr:first');removeAttr(t);t.remove();"><span class="ico ico-delete-variable"></span></a></span></td>
            <td class="td2"><eyf:InputText ID="itKey" runat="server" name="k" /></td>
            <td class="td3"><eyf:InputText ID="itVal" runat="server" name="v" /></td>
            <td class="td4"></td>
        </tr>
        </tbody>
    </table>
    <a href="#" onclick="addnewarg();" class="addvar"><span class="ico ico-add-variable"></span> Add Variable</a>
    </FooterTemplate>
</asp:Repeater>