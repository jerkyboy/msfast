<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsGraph.ascx.cs" Inherits="BDika.Web.Application.Controls.Results.ResultsGraph" %>
<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"  codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="100%" height="400" id="main" align="middle">
<param name="allowScriptAccess" value="sameDomain" />
<param name="allowFullScreen" value="false" />
<param name="FlashVars" value="u=<%=RESULTS_URL %>" />
<param name="movie" value="/Controls/Results/MySpace.MSFast.GUI.Engine.RenderGraph.bin" />
<param name="quality" value="high" />
<param name="bgcolor" value="#ffffff" />
<embed src="/Controls/Results/MySpace.MSFast.GUI.Engine.RenderGraph.bin" quality="high" FlashVars="u=<%=RESULTS_URL %>" bgcolor="#ffffff" width="100%" height="400" name="main" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
</object>
 