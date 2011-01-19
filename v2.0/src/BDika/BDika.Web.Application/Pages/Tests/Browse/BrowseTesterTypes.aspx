<%@ Page Language="C#" MasterPageFile="~/Masters/Main.Master" AutoEventWireup="true" CodeBehind="BrowseTesterTypes.aspx.cs" Inherits="BDika.Web.Application.Pages.Tests.Browse.BrowseTesterTypes" %>
<%@ Register TagPrefix="Tests" TagName="TesterTypesPaging" Src="~/Controls/Tests/Browse/TesterTypesPaging.ascx" %>
<asp:Content id="cntBody" ContentPlaceHolderID="body" runat="Server">
    <Tests:TesterTypesPaging ID="Tests_TesterTypesPaging" runat="server" />
</asp:Content>







