<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsThumbnails.ascx.cs" Inherits="MySpace.MSFast.Automation.Web.Application.Controls.Results.ResultsThumbnails" %>
<eyf:Box runat="server">
<eyf:SubTitle text="Captured States" runat="server" />
<asp:Repeater ID="rptThumbnails" runat="server">
    <HeaderTemplate>
    <eyf:StaticFileInclude ID="siJPopup" runat="server" FileId="JPopup" />
    <eyf:StyleLiteral ID="slResultsThumbnails" runat="server">
        .thumbnails{clear:both;}
        .thumbnail{padding:6px;position:relative;}
        .thumbnail a{position:relative;}
        .thumbnail img{width:120px;height:90px;border:none}
        .thumbnail span.ts{opacity:0.5;filter:alpha(opacity=50);display:block;padding:2px;position:absolute;left:0;bottom:0;color:#FFF;background:#000;font-size:10px;}
        .scrl{overflow:auto;padding:20px 10px}
    </eyf:StyleLiteral>
    <eyf:ScriptLiteral id="scResultsThumbnails" runat="server">
    function openthum(e){
        showPopup("<img src=\"" + e.find("img").attr("src") + "\" style=\"max-width:800px\"/>",e.siblings("span.ts").text());
    }
    </eyf:ScriptLiteral>
<div class="scrl">
<table class="thumbnails">
    <tr>
    </HeaderTemplate>
    <ItemTemplate>
        <td class="thumbnail">
            <a href="#" onclick="openthum($(this));return false;"><asp:Literal runat="server" id="ltImage" /><span class="ts"><asp:Literal runat="server" id="ltTimeStamp" /></span></a>
        </td>
    </ItemTemplate>
    <FooterTemplate>
    </tr>
</table>
</div>
    </FooterTemplate>
</asp:Repeater>
<asp:PlaceHolder ID="cphNoThumbnails" runat="server" >
    <div class="nores">No Thumbnails Found...</div>
</asp:PlaceHolder>
</eyf:Box>