package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.geom.Point;
	import flash.utils.*;
	
	import MSFast.Engine.Data.DownloadState;

	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	public class DownloadSummery extends MovieClip
	{
		private var _downloadState:DownloadState = null;
		private var tooltip_text:TextField = null;
		private static var TOOLTIP_TEXT_FORMAT:TextFormat;

		public function DownloadSummery()
		{
			TOOLTIP_TEXT_FORMAT = new TextFormat(); 
			TOOLTIP_TEXT_FORMAT.size = 11;
			TOOLTIP_TEXT_FORMAT.color = 0x000000;
			TOOLTIP_TEXT_FORMAT.font = "Tahoma";
			
			this.tooltip_text = new TextField();
			this.tooltip_text.selectable = false;
			this.tooltip_text.autoSize = "left";
			this.tooltip_text.background = true;
			this.tooltip_text.backgroundColor = 0xFFFFCC;
			this.tooltip_text.border = true;
			this.tooltip_text.borderColor = 0x555555;
			this.tooltip_text.x = 0;
			this.addChild(this.tooltip_text);
		}
		
		public function set downloadState(downloadState:DownloadState):void
		{	
			if(this._downloadState != downloadState){
				this._downloadState = downloadState;
				resetView();
			}
		}
		public function get downloadState():DownloadState{return this._downloadState;}

		private function resetView():void
		{
			if(this._downloadState == null)
				return;
			var u = _downloadState.url;
			if (u.length > 100)
				u = u.substr(0,100) + "...";
			
			u += "\n\n";
			u += "Total Bytes Sent: <b>" + getDecimal(_downloadState.totalSent) + "</b>\n";
			u += "Total Bytes Received: <b>" + getDecimal(_downloadState.totalReceived) + "</b>\n\n";

			var dt = (_downloadState.receivingResponseEndTime-_downloadState.sendingRequestStartTime);
			u += "Estimated Download Times: \n"
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt) + "</b> (ms) (9 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.6428) + "</b> (ms) (14 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.3214) + "</b> (ms) (28 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.1607) + "</b> (ms) (56 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.0703) + "</b> (ms) (128 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.0351) + "</b> (ms) (256 Kbps)\n";
			u += "&nbsp;&nbsp;<b>" + getDecimal(dt*0.0090) + "</b> (ms) (1.5 Mbps)\n";


			this.tooltip_text.htmlText = u;
			this.tooltip_text.setTextFormat(TOOLTIP_TEXT_FORMAT);
		}
	
	private function getDecimal(n:Number):String
	{
		var s:String = ""+Math.round(n);
		var x = new Array();
		x.push("");
		
		var w = 0;
		for(var i = s.length-1; i >= 0 ; i--)
		{
			if((w%3) == 0)
				x.push("");
				
			x[x.length-1] = s.charAt(i) + x[x.length-1];
			
			w++;
		}
		var ss = "";
		for(i = x.length-1; i >= 0 ; i--)
		{
			if(i < x.length-1 && i > 0)
			  ss += ",";
			ss += x[i];
		}
		return ""+ss;
	 }
	}
}

