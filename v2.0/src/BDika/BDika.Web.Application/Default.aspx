<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BDika.Web.Application._Default" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
<table>
    <tr>
        <td><eyf:Href ID="hrefLatestResults" runat="server" /></td>
        <td><eyf:Href ID="hrefManageTests" runat="server" /></td>
    </tr>
    <tr>
        <td><eyf:Href ID="hrefManageTestBox" runat="server" /></td>
        <td><eyf:Href ID="hrefManageManageTriggers" runat="server" /></td>
    </tr>
</table>
</asp:Content>