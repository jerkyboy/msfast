package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.MarkerPeice;
	import MSFast.Engine.GUI.ThumbnailsPair;
	import flash.geom.*;
	import flash.events.Event;
	import MSFast.Engine.GUI.ThumbnailPreview;
	import MSFast.Engine.GUI.Popup;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	import MSFast.Engine.Events.UIEvent;
	
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	public class MarkersGraph extends AbsDataGraphDisplay
	{
		private var graph_mc:MovieClip = null;
		private var mask_mc:MovieClip = null;
		
		private var markerText:Array = new Array();
		private var styles:Array = new Array();
		
		public function MarkersGraph()
		{
			super();

			var textFormat = new TextFormat(); 
			
			with(textFormat){ size = 10; color = 0x333333; font = "Tahoma"; align = "center"; }
			styles.push({match : "Segment([0-9]*)", textFormat : textFormat, line : 0xFBEC99, background : 0xFAA403});
			
			textFormat = new TextFormat(); 
			with(textFormat){ size = 10; color = 0x999999; font = "Tahoma"; align = "center"; }
			styles.push({match : ".*", textFormat : textFormat, line : 0xDDDDDD, background : 0xF0F0F0});

			this.mask_mc = new MovieClip();
			this.graph_mc = new MovieClip();
			
			this.bounds_mc.visible = false;
			
			addChild(this.mask_mc);
			addChild(this.graph_mc);
			
			drawRect(this.mask_mc);
			drawRect(this.graph_mc);
			
			this.graph_mc.mask = (this.mask_mc);
			
			this.addEventListener(Event.RESIZE,_paneResized);
			this.addEventListener("GraphSizeChanged",_graphResized);
			this.addEventListener("GraphMoved",_graphMoved);
		}
		
		private function createTextField(st:String,textFormat:TextFormat,backColor:Number,borderColor:Number):TextField
		{
			var t = new TextField();
			t.text = st;
			t.selectable = false;
			t.setTextFormat(textFormat);
			t.background = true;
			t.height = 16;
			t.backgroundColor = backColor;
			t.border = true;
			t.borderColor = borderColor;
			return t;
		}
		
		public override function init():void
		{
		}
		
		//Resize
		private function _paneResized(e:Event):void
		{
			this.mask_mc.width = this.containerBounds.width;
			this.mask_mc.height = this.containerBounds.height;
			onNewPerfData(this.perfData);
		}		
		
		private function _graphResized(e:Event):void{
			onNewPerfData(this.perfData);
		}
		
		private function _graphMoved(e:Event):void{
			this.graph_mc.x = this.graphBounds.x;
		}	
		
		public override function onNewPerfData(perfData:PerfData):void
		{
			clearDisplay(this.perfData != perfData);
			
			if(perfData != null && perfData.markersData != null && perfData.markersData.length > 0)
			{
				this.mask_mc.visible = true;
				this.graph_mc.visible = true;
				drawDisplay(perfData);
			}
			else
			{
				this.mask_mc.visible = false;
				this.graph_mc.visible = false;
			}
		}
		
		private function drawDisplay(perfData:PerfData):void
		{	
		
			if(perfData == null || perfData.markersData == null || perfData.markersData.length == 0)
			{
				return;
			}
			
			var rp:MarkerPeice = null;
			var markersData = perfData.markersData;
			
			var x = 0;
			var h = this.containerBounds.height - ThumbnailsPair.DEFAULT_HEIGHT - 5;
			var d = 0;
			var style = null;
			
			if(markerText.length == 0){
				for(d = 0 ; d < markersData.length; d++){
					rp = ((MarkerPeice)(markersData[d]));
					style = getStyle(rp.name);
					if(rp.timeStamp > 0 && rp.name.toLocaleLowerCase().indexOf("segment(") == -1){
						var tf:TextField = createTextField(rp.name,style.textFormat,style.background,style.line);
						markerText.push({textfield : tf, timeStamp : rp.timeStamp});
						this.graph_mc.addChild(tf);
					}
				}
			}
			
			for(d = 0 ; d < markerText.length; d++){	
				if(markerText[d].timeStamp > 0)
				{
					x =  ((markerText[d].timeStamp - perfData.startTime) * ratio);
					markerText[d].textfield.x = x;
					markerText[d].textfield.y = markerText[d].textfield.height * d;
				}					
			}
			
			for(d = 0 ; d < markersData.length; d++)
			{
				rp = ((MarkerPeice)(markersData[d]));
				
				if(rp.timeStamp > 0)
				{
					x =  ((rp.timeStamp - perfData.startTime) * ratio);
					style = getStyle(rp.name);
					this.graph_mc.graphics.lineStyle(0,style.line,0.25);
					this.graph_mc.graphics.moveTo(x,0);
					this.graph_mc.graphics.lineTo(x,h);
					this.graph_mc.graphics.moveTo(x,h);
					this.graph_mc.graphics.beginFill(style.background);
					this.graph_mc.graphics.drawCircle(x, h, 2);
					this.graph_mc.graphics.endFill();
				}
			}
			
/*			for(var i = 0 ; i < eventsDisplay.length ; i++)
			{
				trace(perfData[eventsDisplay[i].fi]);
				if(perfData[eventsDisplay[i].fi] > 0)
				{
					eventsDisplay[i].mc.visible = true;
					eventsDisplay[i].mc.x = (perfData[eventsDisplay[i].fi] - perfData.startTime) * ratio;
					drawEvent(this.containerBounds.height, eventsDisplay[i].mc.x);
				}else{
					eventsDisplay[i].mc.visible = false;
				}
			}*/
		}
		private function getStyle(n:String):Object{
			for(var i = 0 ; i < styles.length; i++){
				if(n.match(styles[i].match)){
					return styles[i];
				}
			}
			return styles[0];
		}
		private function drawEvent(h:Number,xx:Number):void
		{
				this.graph_mc.graphics.lineStyle(0, 0x2594af , 0.25);
				this.graph_mc.graphics.moveTo(xx,0);
				this.graph_mc.graphics.lineTo(xx,h);
		}
			
		public override function get ratio():Number{
			if(perfData == null) return 0;
			return (this.graphBounds.width / (perfData.endTime - perfData.startTime));
		}		
		
		private function clearDisplay(clearAll:Boolean):void
		{
			if(clearAll && markerText != null && markerText.length > 0){
				while(markerText.length > 0){
					var _mc:MovieClip = MovieClip(markerText.pop().textfield);
					_mc.parent.removeChild(_mc);
				}
			}
			this.graph_mc.graphics.clear();
		}
	}	
}