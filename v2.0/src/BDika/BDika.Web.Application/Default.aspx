<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BDika.Web.Application._Default" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
<eyf:StyleLiteral ID="slMain" runat="server">
table{margin:21px;}
table .ico{float:left;margin:-2px 6px 0 0;}
table td{padding:10px;font-size:12px;font-weight:bold;}
table td a:hover{text-decoration:underline;}
</eyf:StyleLiteral>
<eyf:Box runat="server">
<table>
    <tr>
        <td><span class="ico ico-results"></span><eyf:Href ID="hrefLatestResults" runat="server" /></td>
        <td><span class="ico ico-tests"></span><eyf:Href ID="hrefManageTests" runat="server" /></td>
    </tr>
    <tr>
        <td><span class="ico ico-testbox"></span><eyf:Href ID="hrefManageTestBox" runat="server" /></td>
        <td><span class="ico ico-trigger"></span><eyf:Href ID="hrefManageManageTriggers" runat="server" /></td>
    </tr>
</table>
</eyf:Box>
<eyf:Box runat="server">
<img src="/Static/Skin/Skin001/Global/img/pardon.png" />
</eyf:Box>
</asp:Content>