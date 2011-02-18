<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsPaging.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Results.Browse.ResultsPaging" %>
<%@ Register TagPrefix="Results" TagName="ResultsList" Src="~/Controls/Results/Browse/ResultsList.ascx" %>
<eyf:BrowseForm ID="bfBrowseResults" runat="server" StyleClass="browseResults" ResultsView="'tbody.trr'">
    <eyf:StaticFileInclude ID="siJPopup" runat="server" FileId="JPopup" />
    <eyf:StaticFileInclude ID="siJCookie" runat="server" FileId="JCookie" />
    <eyf:StyleLiteral ID ="slResultsList" runat="server">
    table.custommizetableview{width:400px;border-spacing:0px;background:#FFF;border:solid #EEE;border-width:0 1px 1px 0;-moz-box-shadow:0 2px 0 #e3e3e3;-webkit-box-shadow:0 2px 0 #e3e3e3;box-shadow:0 2px 0 #e3e3e3;}
    table.custommizetableview td{padding:8px 10px;border:1px #EEE solid;border-width:1px 0 0 1px}
    table.custommizetableview td input{margin:0 9px 0 0;}
    table.resultslst.resultstable{table-layout:auto;}
    table.resultslst.resultstable th,table.resultslst.resultstable td{display:none;}
    table.resultslst.resultstable .ts.br td,table.resultslst.resultstable .ts.br th{display:table-cell}
    table.resultslst.resultstable .srt .sr{display:none;margin:0 5px}
    table.resultslst.resultstable .srt.dsc.slc .sr.d,
    table.resultslst.resultstable .srt.asc.slc .sr.a{display:inline;}
    table.resultslst.resultstable .rstate{float:right;margin:0;}
    table.resultslst.resultstable .nores{display:table-cell!important;}
    </eyf:StyleLiteral>
    <eyf:ScriptLiteral ID="scResultsList" runat="server">
        $(function() {
            var getCols = function(name){
                return $.cookie("ct" + name) || "";
            };                
            $.fn.columnizedTable = function(options) {
                options = $.extend({name:"default", cols : ""}, options);
                $(this).attr("ctName", options.name);
                var cols = (options.cols != "") ? options.cols : getCols(options.name);
                if(cols == "" && options.defcols) cols = options.defcols;
                var vis = 0;
                for(var i = 0; i < cols.length; i++){
                    if(cols[i] == "1"){
                        $(this).find("tr .td" + (i+1)).show();
                        vis++;
                    }else{
                        $(this).find("tr .td" + (i+1)).hide();
                    }
                }
                $(".fxclspn").attr("colspan",(vis%2==0)?vis/2:(vis-1)/2);
                
                $.cookie("ct" + options.name, cols, { path: "/", expires: 25});
                return this;
            };          
            $.fn.customizeTableView = function() {
                var cols = getCols($(this).attr("ctName"));
                var inst = $(this);
                var col = 0;
                var htm = "<table class='custommizetableview'><tr>"; 
                $(this).find("thead tr:first").find("th").each(function(){
                    var cc = parseInt(($(this).attr("class").match(/td([0-9])*/gi) + "").replace(/td/gi,""))-1;
                    if(col++ % 2 == 0) htm += "</tr><tr>";
                    htm += "<td><input type='checkbox' " + ((cols[cc] == "1") ? "checked='checked'" : "") + " colid='" + cc + "' name='tc' value='1'/>" + $(this).find(".tx").text() + "</td>";
                });
                htm += "</tr></table>";
                htm = $(htm);
                htm.find("input").click(function(){
                   var ls = getCols(inst.attr("ctName"));
                   var nco = "";
                   var id = parseInt($(this).attr("colid"));
                   for(var i = 0 ; i < ls.length; i++){
                    if(i == id){
                        nco += ($(this).fieldValue() == "1") ? "1" : "0";
                    }else{
                        nco += ls[i];
                    }
                   }
                   inst.columnizedTable({name : $(inst).attr("ctName"), cols: nco});
                });
                showPopupObject(htm,"Customize Table");
            }
            $("form.browseResults").bind("browseDataLoaded", function(e,f) {
                $("table.resultstable").columnizedTable({name:"resultstable",defcols:"1010111101100011111"});
            });            
            $("table.resultstable").columnizedTable({name:"resultstable",defcols:"1010111101100011111"});
        });
    </eyf:ScriptLiteral>
    <table class="resultslst resultstable">
        <thead>
            <tr>
                <th class="rdl td1 frst"  width="55%"><a href="#" class="srt" sort="testname"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, testname %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td2"><a href="#" class="srt" sort="testbox"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, testbox %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td3"><a href="#" class="srt" sort="createdon"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, createdon %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td4"><a href="#" class="srt" sort="totaltime"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totaltime %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td5"><a href="#" class="srt" sort="rendertime"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, rendertime %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td6"><a href="#" class="srt" sort="servertime"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, servertime %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>             
                <th class="rdc td7"><a href="#" class="srt" sort="totaldownloadscount"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totaldownloadscount %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td8"><a href="#" class="srt" sort="totaljsdownloadscount"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totaljsdownloadscount %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td9"><a href="#" class="srt" sort="totalcssdownloadscount"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totalcssdownloadscount %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td10"><a href="#" class="srt" sort="totalimagesdownloadscount"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totalimagesdownloadscount %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td11"><a href="#" class="srt" sort="totaldownloadsize"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totaldownloadsize %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td12"><a href="#" class="srt" sort="totaljsdownloadsize"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totaljsdownloadsize %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td13"><a href="#" class="srt" sort="totalcssdownloadsize"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totalcssdownloadsize %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td14"><a href="#" class="srt" sort="totalimagesdownloadsize"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, totalimagesdownloadsize %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td15"><a href="#" class="srt" sort="processortimeavg"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, processortimeavg %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td16"><a href="#" class="srt" sort="usertimeavg"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, usertimeavg %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td17"><a href="#" class="srt" sort="privateworkingsetdelta"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, privateworkingsetdelta %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
                <th class="rdc td18"><a href="#" class="srt" sort="workingsetdelta"><span class="tx"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsPaging, workingsetdelta %>" /></span><span class="sr d">▲</span><span class="sr a">▼</span></a></th>
            </tr>
            <tr class="ts br t">
                <th colspan="19" class="frst rdr">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </th>
            </tr>
        </thead>
        <tbody class="trr">
            <Results:ResultsList runat="server" id="Results_ResultsList" />
        </tbody>
        <tfoot>
            <tr class="ts br b">
                <td colspan="9" class="frst fxclspn">
                    <a href="#" class="mre customizeview" onclick="$('table.resultstable').customizeTableView();"><span class="ico ico-tests"></span>Customize View</a>
                </td>
                <td colspan="9" class="rdr fxclspn">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </td>
            </tr>
        </tfoot>
    </table>        
</eyf:BrowseForm>
