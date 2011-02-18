<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestsList.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.Browse.TestsList" %>
<eyf:StyleLiteral runat="server" ID="slTestsList">
.resultstable tbody td.testslst_selected{width:26px;padding:0 3px 0 6px;}
</eyf:StyleLiteral>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <tr>
        <td class="nores" colspan="6">
            <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TestsList, noresults %>" />
        </td>
    </tr>
</asp:PlaceHolder>
<asp:Repeater ID="rptTestsList" runat="server">
    <ItemTemplate>
        <tr class="r<%#(Container.ItemIndex % 2 == 0) ? "1" : "2" %>" onmouseover="$(this).parents('table:first').find('tr.over').removeClass('over');$(this).addClass('over');" onmouseout="$(this).removeClass('over');">                    
            <asp:PlaceHolder runat="server" id="phTestslstSelectedL"><td class="testslst_selected testslst_selected_link"><asp:Literal runat="server" ID="icTestSelected" /></td></asp:PlaceHolder> 
            <asp:PlaceHolder runat="server" id="phTestslstNameL"><td class="testslst_name testslst_name_link rdb"><eyf:Href runat="server" ID="hrefTestName" /></td></asp:PlaceHolder> 
            <asp:PlaceHolder runat="server" id="phTestslstURLL"><td class="testslst_url testslst_url_link"><eyf:Href runat="server" ID="hrefTestURL" /></td></asp:PlaceHolder> 
            <asp:PlaceHolder runat="server" id="phActionsL">
                <td class="testslst_actions testslst_actions_link rdac">
                    <eyf:Href runat="server" ID="hrefEditTest" />
                    <eyf:Href runat="server" ID="hrefRemoveTest" />
                </td>
            </asp:PlaceHolder>
        </tr>
    </ItemTemplate>
</asp:Repeater>
