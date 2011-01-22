<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShortTesterTypeDescription.ascx.cs" Inherits="BDika.Web.Application.Controls.Tests.ShortTesterTypeDescription" %>
<eyf:StyleLiteral ID="slShortTesterTypeDescription" runat="server">
.testbox {position:relative;}
.testbox .editbox{position:absolute;top:10px;right:10px;font-weight:bold;}
.testbox table{width:50%;background:#FAFAFA;margin:10px}
.testbox table td{padding:5px 10px;background:#FFF;}
</eyf:StyleLiteral>
<eyf:Box runat="server">
    <div class="testbox">
    <eyf:SubTitle ID="ttTesterTypeName" runat="server" />
    <table>
        <tr><td><strong>Client ID</strong></td><td><asp:Literal ID="ltClientID" runat="server" /></td></tr>
        <tr><td><strong>Client Key</strong></td><td><asp:Literal ID="ltClientKey" runat="server" /></td></tr>
        <tr><td><strong>Last Ping</strong></td><td><asp:Literal ID="ltLastPing" runat="server" /></td></tr>
        <tr><td><strong>Enabled</strong></td><td><asp:Literal ID="ltEnabled" runat="server" /></td></tr>
    </table>
    <a href="#" class="editbox" onclick="edittestertype(<%=(this.TesterType != null)? this.TesterType.TesterTypeID +"": ""%>)"><span class="ico ico-edit-test-box"></span><asp:Literal runat="server" text="<%$BDikaResources: Controls.Tests.ShortTesterTypeDescription, edit %>" /></a>
    <eyf:Href runat="server" ID="hrefEditTesterType" />
    </div>
</eyf:Box>