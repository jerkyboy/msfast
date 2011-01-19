<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestsList.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.Browse.TestsList" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.Browse.TestsList, noresults %>" />
    </div>
</asp:PlaceHolder>
<asp:Repeater ID="rptTestsList" runat="server">
    <HeaderTemplate>
        <table class="testslst resultstable">
            <thead>
                <tr>
                    <th class="testslst_name testslst_name_header"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.Browse.TestsList, testname %>" /></th>
                    <th class="testslst_url testslst_url_header"><asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.Browse.TestsList, testurl %>" /></th></asp:PlaceHolder>
                    <th class="testslst_edit testslst_edit_header">&nbsp;</th>
                    <th class="testslst_remove testslst_remove_header">&nbsp;</th>
                    <th class="testslst_add testslst_add_header">&nbsp;</th>
                </tr>
            </thead>
            <tbody>
    </HeaderTemplate>
    <ItemTemplate>
                <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>">                    
                    <td class="testslst_name testslst_name_link"><eyf:Href runat="server" ID="hrefTestName" /></td>                    
                    <td class="testslst_url testslst_url_link"><eyf:Href runat="server" ID="hrefTestURL" /></td>
                    <td class="testslst_edit testslst_edit_link"><eyf:Href runat="server" ID="hrefEditTest" /></td>
                    <td class="testslst_remove testslst_remove_link"><eyf:Href runat="server" ID="hrefRemoveTest" /></td>
                    <td class="testslst_add testslst_add_link"><eyf:Href runat="server" ID="hrefAddTest" /></td>
                </tr>
    </ItemTemplate>
    <FooterTemplate>
        </tbody>
    </table>
    </FooterTemplate>
</asp:Repeater>
