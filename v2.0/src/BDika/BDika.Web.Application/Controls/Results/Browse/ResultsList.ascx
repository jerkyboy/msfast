<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsList.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.Browse.ResultsList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, noresults %>" />
    </div>
</asp:PlaceHolder>
<asp:Repeater ID="rptResultsList" runat="server">
    <HeaderTemplate>
        <eyf:StaticFileInclude ID="siJPopup" runat="server" FileId="JPopup" />
        <eyf:StaticFileInclude ID="siJCookie" runat="server" FileId="JCookie" />
        <eyf:StyleLiteral ID ="slResultsList" runat="server">
        table.custommizetableview{width:400px;}
        table.custommizetableview td{padding:3px 10px;}
        table.custommizetableview td input{margin:0 9px 0 0;}
        a.customizeview{font-weight:bold;z-index:10;display:block;padding:10px}
        a.customizeview .ico{float:left;margin:-2px 6px 0 0;}
        </eyf:StyleLiteral>
        <eyf:ScriptLiteral ID="scResultsList" runat="server">
            $(function() {            
                var getCols = function(name){
                    return $.cookie("ct" + name) || "";
                };                
                $.fn.columnizedTable = function(options) {
                    options = $.extend({name:"default", cols : ""}, options);
                    $(this).attr("ctName", options.name);
                    var cols = (options.cols != "") ? options.cols : getCols(options);
                    if(cols == "" && options.defcols) cols = options.defcols;
                    
                    for(var i = 0; i < cols.length; i++){
                        if(cols[i] == "1"){
                            $(this).find("tr .td" + (i+1)).show();
                        }else{
                            $(this).find("tr .td" + (i+1)).hide();
                        }
                    }
                    $.cookie("ct" + options.name, cols);
                    return this;
                };          
                $.fn.customizeTableView = function() {
                    var cols = getCols($(this).attr("ctName"));
                    var inst = $(this);
                    var col = 0;
                    var htm = "<table class='custommizetableview'><tr>"; 
                    $(this).find("th").each(function(){
                        var cc = parseInt(($(this).attr("class").match(/td([0-9])*/gi) + "").replace(/td/gi,""))-1;
                        if(col++ % 2 == 0) htm += "</tr><tr>";
                        htm += "<td><input type='checkbox' " + ((cols[cc] == "1") ? "checked='checked'" : "") + " colid='" + cc + "' name='tc' value='1'/>" + $(this).text() + "</td>";
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
            });
            $(function(){$("table.resultstable").columnizedTable({name:"resultstable",defcols:"101011110110001111"});});
        </eyf:ScriptLiteral>
        <div class="rs">
        <a href="#" class="customizeview" onclick="$('table.resultstable').customizeTableView();"><span class="ico ico-tests"></span>Customize View</a>
        <table class="resultslst resultstable">
            <thead>
                <tr>
                    <th class="rdl td1"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, testname %>" /></th>
                    <th class="rdc td2"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, testbox %>" /></th>
                    <th class="rdc td3"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, createdon %>" /></th>
                    <th class="rdc td4"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaltime %>" /></th>
                    <th class="rdc td5"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, rendertime %>" /></th>
                    <th class="rdc td6"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, servertime %>" /></th>                    
                    <th class="rdc td7"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaldownloadscount %>" /></th>
                    <th class="rdc td8"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaljsdownloadscount %>" /></th>
                    <th class="rdc td9"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalcssdownloadscount %>" /></th>
                    <th class="rdc td10"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalimagesdownloadscount %>" /></th>
                    <th class="rdc td11"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaldownloadsize %>" /></th>
                    <th class="rdc td12"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaljsdownloadsize %>" /></th>
                    <th class="rdc td13"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalcssdownloadsize %>" /></th>
                    <th class="rdc td14"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalimagesdownloadsize %>" /></th>
                    <th class="rdc td15"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, processortimeavg %>" /></th>
                    <th class="rdc td16"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, usertimeavg %>" /></th>
                    <th class="rdc td17"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, privateworkingsetdelta %>" /></th>
                    <th class="rdc td18"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, workingsetdelta %>" /></th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>  
    <ItemTemplate>
                <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>" onmouseover="$(this).parents('table:first').find('tr.over').removeClass('over');$(this).addClass('over');" onmouseout="$(this).removeClass('over');">
                    <td class="rdl rdb td1"><eyf:Href runat="server" ID="hrefTestName" /></td>
                    <td class="rdl td2"><eyf:Href runat="server" ID="hrefTesterTypeName" /></td>
                    <td class="rdc td3"><eyf:Href runat="server" ID="hrefResultsDate" /></td>                    
                    <td class="rdr td4"><eyf:Href runat="server" ID="hrefTotalTime" /></td>                    
                    <td class="rdr td5"><eyf:Href runat="server" ID="hrefRenderTime" /></td>
                    <td class="rdr td6"><eyf:Href runat="server" ID="hrefServerTime" /></td>                    
                    <td class="rdc td7"><eyf:Href runat="server" ID="hrefTotalDownloadsCount" /></td>
                    <td class="rdc td8"><eyf:Href runat="server" ID="hrefTotalJSDownloadsCount" /></td>
                    <td class="rdc td9"><eyf:Href runat="server" ID="hrefTotalCSSDownloadsCount" /></td>
                    <td class="rdc td10"><eyf:Href runat="server" ID="hrefTotalImagesDownloadsCount" /></td>
                    <td class="rdr td11"><eyf:Href runat="server" ID="hrefTotalDownloadSize" /></td>
                    <td class="rdr td12"><eyf:Href runat="server" ID="hrefTotalJSDownloadSize" /></td>
                    <td class="rdr td13"><eyf:Href runat="server" ID="hrefTotalCSSDownloadSize" /></td>
                    <td class="rdr td14"><eyf:Href runat="server" ID="hrefTotalImagesDownloadSize" /></td>
                    <td class="rdc td15"><eyf:Href runat="server" ID="hrefProcessorTimeAvg" /></td>
                    <td class="rdr td16"><eyf:Href runat="server" ID="hrefUserTimeAvg" /></td>
                    <td class="rdr td17"><eyf:Href runat="server" ID="hrefPrivateWorkingSetDelta" /></td>
                    <td class="rdr td18"><eyf:Href runat="server" ID="hrefWorkingSetDelta" /></td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
        </div>
    </FooterTemplate>
</asp:Repeater>



