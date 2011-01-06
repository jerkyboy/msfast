package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.geom.Point;
	import flash.utils.*;
	import MSFast.Engine.Data.PerformanceState;
	

	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	public class PerformanceSummery extends MovieClip
	{
		private var tooltip_text:TextField = null;
		private static var TOOLTIP_TEXT_FORMAT:TextFormat;

		public function PerformanceSummery()
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
		
		public function setDetails(before:PerformanceState, after:PerformanceState, r:Number):void
		{
			var u = "";

			u += "User Time: <b>" + getDecimal(before.userTime + ((after.userTime-before.userTime)*r)) + "</b>\n";
			u += "Processor Time: <b>" + getDecimal(before.processorTime + ((after.processorTime-before.processorTime)*r)) + "</b>\n";
			u += "Working Set: <b>" + getDecimal(before.workingSet + ((after.workingSet-before.workingSet)*r)) + "</b>\n";
			u += "Private Working Set: <b>" + getDecimal(before.privateWorkingSet + ((after.privateWorkingSet-before.privateWorkingSet)*r)) + "</b>\n";

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

