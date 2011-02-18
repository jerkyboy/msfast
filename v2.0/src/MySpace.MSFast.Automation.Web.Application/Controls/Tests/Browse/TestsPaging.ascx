<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestsPaging.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.Browse.TestsPaging" %>
<%@ Register TagPrefix="Tests" TagName="TestsList" Src="~/Controls/Tests/Browse/TestsList.ascx" %>
<eyf:StyleLiteral runat="server">
<%if (!this.ShowEditLink && !this.ShowRemoveLink){%>form#<%=FORMID%> .testslst_actions{display:none;}<%} %>
<%if (!this.ShowTestURL){%>form#<%=FORMID%> .testslst_url{display:none;}<%} %>
<%if (!this.ShowSelected){%>form#<%=FORMID%> .testslst_selected{display:none;}<%} %>
</eyf:StyleLiteral>
<eyf:BrowseForm ID="bfBrowseTests" runat="server" StyleClass="browseTests" ResultsView="'tbody.tsr'">
    <table class="testslst resultstable">
        <thead>
            <tr>
                <th width="20px" class="frst testslst_selected testslst_selected_header"><eyf:InputCheckbox runat="server" ID="icTestSelected" /></th>
                <th width="25%" class="testslst_name testslst_name_header"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TestsList, testname %>" /></th>
                <th width="60%" class="testslst_url testslst_url_header"><asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TestsList, testurl %>" /></th>
                <th width="15%" class="testslst_actions testslst_actions_header">&nbsp;</th>
            </tr>
            <tr class="ts br t">
                <th colspan="6" class="frst rdr">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </th>
            </tr>
        </thead>
        <tbody class="tsr">
            <Tests:TestsList runat="server" id="Tests_TestsList" />
        </tbody>
        <tfoot>
            <tr class="ts br b">
                <td colspan="6" class="rdr frst">
                    <eyf:BrowsePreviousPageHref runat="server"><span>◄</span></eyf:BrowsePreviousPageHref>
                    <eyf:BrowsePagesHref runat="server" />
                    <eyf:BrowseNextPageHref runat="server"><span>►</span></eyf:BrowseNextPageHref>
                </td>
            </tr>
        </tfoot>
    </table>        
</eyf:BrowseForm>
