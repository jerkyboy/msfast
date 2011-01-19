<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsList.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.Browse.ResultsList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, noresults %>" />
    </div>
</asp:PlaceHolder>
<asp:Repeater ID="rptResultsList" runat="server">
    <HeaderTemplate>
        <table class="resultslst resultstable">
            <thead>
                <tr>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, testname %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, testbox %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, createdon %>" /></th>
                    
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaltime %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaldownloadscount %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaljsdownloadscount %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalcssdownloadscount %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalimagesdownloadscount %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaldownloadsize %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totaljsdownloadsize %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalcssdownloadsize %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, totalimagesdownloadsize %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, processortimeavg %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, usertimeavg %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, privateworkingsetdelta %>" /></th>
                    <th><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, workingsetdelta %>" /></th>
                    
                    <th>&nbsp;</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>  
    <ItemTemplate>
                <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>">
                    <td><eyf:Href runat="server" ID="hrefTestName" /></td>
                    <td><eyf:Href runat="server" ID="hrefTesterTypeName" /></td>
                    <td><eyf:Href runat="server" ID="hrefResultsDate" /></td>
                    
                    <td><eyf:Href runat="server" ID="hrefTotalTime" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalDownloadsCount" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalJSDownloadsCount" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalCSSDownloadsCount" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalImagesDownloadsCount" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalDownloadSize" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalJSDownloadSize" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalCSSDownloadSize" /></td>
                    <td><eyf:Href runat="server" ID="hrefTotalImagesDownloadSize" /></td>
                    <td><eyf:Href runat="server" ID="hrefProcessorTimeAvg" /></td>
                    <td><eyf:Href runat="server" ID="hrefUserTimeAvg" /></td>
                    <td><eyf:Href runat="server" ID="hrefPrivateWorkingSetDelta" /></td>
                    <td><eyf:Href runat="server" ID="hrefWorkingSetDelta" /></td>
                    
                    <td><a href="#"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Results.Browse.ResultsList, view %>" /></a></td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
            </tbody>
        </table>
    </FooterTemplate>
</asp:Repeater>



