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
		private static var MARKER_TEXTFORMAT:TextFormat;
		
		private var graph_mc:MovieClip = null;
		private var mask_mc:MovieClip = null;
		
		private var segments:Array;
		
		public function MarkersGraph()
		{
			super();

			MARKER_TEXTFORMAT = new TextFormat(); 
			MARKER_TEXTFORMAT.size = 10;
			MARKER_TEXTFORMAT.color = 0x0d8aa8;
			MARKER_TEXTFORMAT.font = "Tahoma";
			MARKER_TEXTFORMAT.align = "center";

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
		
		private function createTextField(st:String,textFormat:TextFormat,backColor:Number,borderColor:Number):TextField{
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
			
			if(perfData != null && perfData.renderData != null && perfData.renderData.length > 0)
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
			
			var min = getMinimumMarkerTime(renderData, this.thumbnails.length);
			
			for(var d = 0 ; d < markersData.length; d++)
			{
				rp = ((MarkerPeice)(markersData[d]));
				
				if(rp.timeStamp > 0)
				{
					x =  rp.timeStamp * ratio;
					
					this.graph_mc.graphics.lineStyle(0,0xffCC00,0.25);
					this.graph_mc.graphics.moveTo(x,0);
					this.graph_mc.graphics.lineTo(x,h);
				
					this.graph_mc.graphics.moveTo(x,h);
					this.graph_mc.graphics.beginFill(0xFF7700);
					this.graph_mc.graphics.drawCircle(x1, h, 2);
					this.graph_mc.graphics.endFill();

					if((rp.endTime-rp.startTime) > min && tcount < this.thumbnails.length)
					{
						this.graph_mc.graphics.lineStyle(0,0xffCC00);
						this.graph_mc.graphics.moveTo(x1,h+2);
						this.graph_mc.graphics.lineTo(x2,h+2);
						this.graph_mc.graphics.moveTo(x1 + ((x2-x1)/2),h+2);
						this.graph_mc.graphics.lineTo(x1 + ((x2-x1)/2),h+8);
						
						this.thumbnails[tcount].x = x1 + ((x2-x1)/2);
						this.thumbnails[tcount].y = h+3;
						this.thumbnails[tcount].setThumbnails(url, perfData.thumbnail_path + "TC_" + perfData.testId + "_" + id + ".jpg");
						this.thumbnails[tcount].segment = rp;
						this.thumbnails[tcount].setText( (Math.round((((rp.endTime-rp.startTime)/(perfData.endTime - perfData.startTime) )*10000))/100) + "%");
						tcount ++;
					}
					
					url = perfData.thumbnail_path + "TC_" + perfData.testId + "_" + id + ".jpg";
				}
			}
			
			this.graph_mc.graphics.moveTo(x2,0);
			this.graph_mc.graphics.lineTo(x2,h);
			

			for(var i = 0 ; i < eventsDisplay.length ; i++)
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
			}
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
		
		private function clearDisplay(clearThumbnails:Boolean):void
		{
			this.graph_mc.graphics.clear();
		}
		
		private function getMinimumMarkerTime(markersData:Array, top:Number) : Number
		{
			var avgc:Number = 0;
			var res = new Array();

			for(var d:Number = 0 ; d < renderData.length; d++)
			{
				if (renderData[d].startTime != 0 && renderData[d].endTime != 0)
				{
					res.push(renderData[d].endTime-renderData[d].startTime);
				}
			}

			res = res.sort(Array.DESCENDING | Array.NUMERIC);
			
			if (top < res.length)
			{
				return res[top];
			}
			
			return res[0];
		}
	}	
}