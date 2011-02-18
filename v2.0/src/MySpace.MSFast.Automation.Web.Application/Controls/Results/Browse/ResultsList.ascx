<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsList.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Results.Browse.ResultsList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <tr>
        <td class="nores" colspan="18">
            <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Results.Browse.ResultsList, noresults %>" />
        </td>
    </tr>
</asp:PlaceHolder>
<asp:Repeater ID="rptResultsList" runat="server">
    <ItemTemplate>
        <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>" onmouseover="$(this).parents('table:first').find('tr.over').removeClass('over');$(this).addClass('over');" onmouseout="$(this).removeClass('over');">
            <td class="rdl rdb td1"><eyf:Href runat="server" ID="hrefResultsState" styleclass="rstate"/><eyf:Href runat="server" ID="hrefTestName" /></td>
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
</asp:Repeater>



