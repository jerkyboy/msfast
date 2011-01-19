<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TesterTypesList.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.Browse.TesterTypesList" %>
<%@ Register TagPrefix="Tests" TagName="ShortTesterTypeDescription" Src="~/Controls/Tests/ShortTesterTypeDescription.ascx" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <div class="nores">
        <asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.Browse.TesterTypesList, noresults %>" />
    </div>
</asp:PlaceHolder>
<asp:Repeater ID="rptTesterTypesList" runat="server">    
    <HeaderTemplate>
        <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
        <eyf:ScriptLiteral ID="scTesterTypesList" runat="server">
        function edittestertype(ttid){$.loadControlPopup("<%=BDika.Web.Application.ControlLoaders.Tests.UpdateOrCreateTesterTypeControlLoader.GetURL(new BDika.Entities.Tests.TesterTypeID(222))%>".replace(/222/g,ttid),"<asp:Literal runat="server" Text="<%$BDikaResources: Controls.Tests.Browse.TesterTypesList, edit %>" />");}
        </eyf:ScriptLiteral>
    </HeaderTemplate>
    <ItemTemplate>
        <Tests:ShortTesterTypeDescription runat="server" id="Tests_ShortTesterTypeDescription" />
    </ItemTemplate>
</asp:Repeater>
