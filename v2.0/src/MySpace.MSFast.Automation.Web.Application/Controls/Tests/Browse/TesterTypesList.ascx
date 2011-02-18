<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TesterTypesList.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Tests.Browse.TesterTypesList" %>
<%@ Register TagPrefix="Tests" TagName="ShortTesterTypeDescription" Src="~/Controls/Tests/ShortTesterTypeDescription.ascx" %>
<asp:PlaceHolder ID="cphNoResults" runat="server" >
    <eyf:Box runat="server">
        <div class="nores">
            <asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TesterTypesList, noresults %>" />
        </div>
    </eyf:Box>
</asp:PlaceHolder>
<asp:Repeater ID="rptTesterTypesList" runat="server">    
    <HeaderTemplate>
        <eyf:StaticFileInclude ID="siJLoadControl" runat="server" FileId="JLoadControl" />
        <eyf:ScriptLiteral ID="scTesterTypesList" runat="server">
        $(function(){
            $(document).bind("deleteCanceled",function(e,t){
                hidePopup();
            }).bind("testerTypeDeleted",function(e,t){
                hidePopup();
                location.reload(true);
            }).bind("testerTypeUpdated",function(e,t){
                hidePopup();
                location.reload(true);
            });
        });
        function edittestertype(ttid){$.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.UpdateOrCreateTesterTypeControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TesterTypeID(222))%>".replace(/222/g,ttid),"<asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TesterTypesList, edit %>" />");}
        function deletetestertype(ttid){$.loadControlPopup("<%=MySpace.MSFast.Automation.Web.Application.ControlLoaders.Tests.DeleteTesterTypeFormControlLoader.GetURL(new MySpace.MSFast.Automation.Entities.Tests.TesterTypeID(222))%>".replace(/222/g,ttid),"<asp:Literal runat="server" Text="<%$MSFAResources: Controls.Tests.Browse.TesterTypesList, delete %>" />");}        
        </eyf:ScriptLiteral>
    </HeaderTemplate>
    <ItemTemplate>
        <Tests:ShortTesterTypeDescription runat="server" id="Tests_ShortTesterTypeDescription" />
    </ItemTemplate>
</asp:Repeater>
