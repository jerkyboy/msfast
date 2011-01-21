<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsThumbnails.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.ResultsThumbnails" %>
<asp:Repeater ID="rptThumbnails" runat="server">
    <ItemTemplate>
        <asp:Literal runat="server" id="ltImage" />
    </ItemTemplate>
</asp:Repeater>
<asp:PlaceHolder ID="cphNoThumbnails" runat="server" >
    <div class="nores">No Thumbnails Found...</div>
</asp:PlaceHolder>