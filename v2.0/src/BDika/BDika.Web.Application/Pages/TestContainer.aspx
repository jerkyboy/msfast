<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="TestContainer.aspx.cs" Inherits="BDika.Web.Application.Pages.TestContainer" %>
<%@ Register TagPrefix="Collectors" TagName="UpdateCollectorsConfiguration" Src="~/Controls/Collectors/UpdateCollectorsConfiguration.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Collectors:UpdateCollectorsConfiguration ID="Collectors_UpdateCollectorsConfiguration" runat="server" />
</asp:Content>
