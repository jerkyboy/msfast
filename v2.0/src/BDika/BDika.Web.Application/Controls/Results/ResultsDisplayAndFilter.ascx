<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsDisplayAndFilter.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.ResultsDisplayAndFilter" %>
<%@ Register TagPrefix="Results" TagName="ResultsPaging" Src="~/Controls/Results/Browse/ResultsPaging.ascx" %>
<eyf:ScriptLiteral ID="scResultsDisplayAndFilter" runat="server">
$(function(){
    $("form.afResultsDisplayAndFilter").bind("beforeSerialize", function(e,r){
        r.kill = true;
        var t = $("div.respaging form.brfrm.browseResults");
        t.setBrowseArg("tid",$(this).find("input[name=tid]").val());
        t.setBrowseArg("ttid",$(this).find("select[name=ttid]").fieldValue());
        t.setBrowseArg("r",$(this).find("select[name=r]").fieldValue());
        t.resetBrowseParameters({index:0, popTotal:true});
        t.reloadBrowseResults();    
    });
});
</eyf:ScriptLiteral>
<eyf:Box runat="server">
<eyf:AsyncForm ID="afResultsDisplayAndFilter" runat="server" styleClass="afResultsDisplayAndFilter">
    <table class="frmtbl resultsfilter">
        <thead>
            <tr>
                <th colspan="2"><asp:Literal runat="server" text="<%$BDikaResources: Controls.Results.ResultsDisplayAndFilter, title %>" /></th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder id="phTestTypes" runat="server" Visible="false">
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.ResultsDisplayAndFilter, testertype %>" /></td>
                <td class="fld">
                    <eyf:InputSelect ID="isTesterTypeIDs" runat="server" name="ttid" />
                </td>
            </tr>
            </asp:PlaceHolder>
            <tr>
                <td class="fldnam"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.ResultsDisplayAndFilter, resultsstate %>" /></td>
                <td class="fld">
                    <eyf:InputSelect ID="isResultsState" runat="server" name="r"/>
                </td>
            </tr>
            <tr>
                <td class="btnbar" colspan="2">
                    <eyf:InputSubmit value="<%$BDikaResources: Controls.Results.ResultsDisplayAndFilter, filter %>" runat="server"/>
                </td>
            </tr>
        </tbody>
    </table>
    <eyf:InputHidden ID="ihTestID" runat="server" name="tid" />
</eyf:AsyncForm>
</eyf:Box>
<eyf:Box runat="server">
<div class="respaging">
    <Results:ResultsPaging ID="Results_ResultsPaging" runat="server" />
</div>
</eyf:Box>