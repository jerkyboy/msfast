package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.DownloadState;
	import MSFast.Engine.Events.UIEvent;
	import MSFast.Engine.GUI.Scrollbar;
	import MSFast.Engine.GUI.Popup;
	import MSFast.Engine.GUI.Scrollable;
	import MSFast.Engine.GUI.DownloadSummery;
	
	import flash.geom.Matrix;
	import flash.geom.*;
	
	import flash.display.SpreadMethod;
	import flash.display.InterpolationMethod;
	import flash.display.GradientType;

	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	public class DownloadGraph extends AbsDataGraphDisplay implements Scrollable
	{
		private var displayObjects:Array = null;
		private var matrix:Matrix = new Matrix();
		
		private var graph_mc:MovieClip = null;
		private var urls_mc:MovieClip = null;
		private var mask_mc:MovieClip = null;
		private var urlsmask_mc:MovieClip = null;
		private var rowOverlay_mc:MovieClip;
	
		private var scrollbar:Scrollbar = null;
		
		private var LINE_HEIGHT:Number = 13;
		
		private var hoverPoints:Array;
		
		private var tooltip:Popup = null;
		private var downloadSummery:DownloadSummery = null;
		
		private static var URL_TEXT_FORMAT:TextFormat;
		
		
		public function DownloadGraph()
		{
			super();
			
			URL_TEXT_FORMAT = new TextFormat(); 
			URL_TEXT_FORMAT.size = 10;
			URL_TEXT_FORMAT.color = 0x999999;
			URL_TEXT_FORMAT.font = "Tahoma";
			
			hoverPoints = new Array();
			matrix.createGradientBox( LINE_HEIGHT, LINE_HEIGHT, 1.57 );
			
			this.mask_mc = new MovieClip();
			this.urlsmask_mc = new MovieClip();
			this.graph_mc = new MovieClip();
			this.urls_mc = new MovieClip();
			this.rowOverlay_mc = new MovieClip();
			
			this.bounds_mc.visible = false;
			addChild(this.mask_mc);
			addChild(this.urlsmask_mc);
			addChild(this.graph_mc);
			addChild(this.urls_mc);
			addChild(this.rowOverlay_mc);
			
			drawRect(this.graph_mc);
			drawRect(this.urlsmask_mc);
			drawRect(this.mask_mc);
			
			this.graph_mc.graphics.moveTo(0,0);
			this.graph_mc.graphics.lineStyle(0,0xFF0000,1);
			this.graph_mc.graphics.lineTo(this.graphBounds.width,120);
			
			this.graph_mc.mask = (this.mask_mc);
			this.urls_mc.mask = (this.urlsmask_mc);
			
			this.addEventListener(Event.RESIZE,_paneResized);
			this.addEventListener("GraphSizeChanged",_graphResized);
			this.addEventListener("GraphMoved",_graphMoved);
			
		}
		
		
		public override function init():void
		{
			stage.addEventListener(MouseEvent.MOUSE_MOVE,_mouseMove);
			
			this.tooltip = new Popup();
			this.downloadSummery = new DownloadSummery();
			this.tooltip.addChild(this.downloadSummery);
			this.tooltip.visible = false;
			stage.addChild(this.tooltip);
			
			this.scrollbar = new Scrollbar();
			
			addChild(this.scrollbar);	
			
			this.scrollbar.setScrollable(this);
			
			this.displayObjects = new Array();
			this.displayObjects.push({s:"connectionStartTime",e:"connectionEndTime", m:new MovieClip(),g:[ 0xfcdb46, 0xdbb11e ]});
			this.displayObjects.push({s:"sendingRequestStartTime",e:"sendingRequestEndTime", m:new MovieClip(),g:[ 0x46eb56, 0x1ec436 ]});
			this.displayObjects.push({s:"sendingRequestEndTime",e:"receivingResponseStartTime", m:new MovieClip(),g:[ 0xfc5246, 0xdc311e ]});
			this.displayObjects.push({s:"receivingResponseStartTime",e:"receivingResponseEndTime", m:new MovieClip(),g:[ 0x006161, 0x004848]});
			
			for(var i = 0 ; i < this.displayObjects.length; i ++)
				this.graph_mc.addChild(this.displayObjects[i].m);
			
			with(this.rowOverlay_mc)
			{
				graphics.clear();
				graphics.lineStyle(0,0xCCFFFF);
				graphics.beginFill(0xCCFFFF,0.20);
				graphics.moveTo(0,0);
				graphics.lineTo(this.graphBounds.width,0);
				graphics.lineTo(this.graphBounds.width,LINE_HEIGHT);
				graphics.lineTo(0,LINE_HEIGHT);
				graphics.lineTo(0,0);
				graphics.endFill();
			}
			
			this.rowOverlay_mc.visible = false;
			drawBG();
		}
		private function drawBG():void
		{
			if(this.graph_mc.height == 0){
				//this.graph_mc.graphics.clear();
				return;
			}
			
			var h = this.graph_mc.height;
			
			with(this.graph_mc)
			{
				graphics.lineStyle(0,0xFFFFFF);
				graphics.beginFill(0xFFFFFF);
				graphics.moveTo(0,0);
				graphics.lineTo(this.graphBounds.width,0);
				graphics.lineTo(this.graphBounds.width,h);
				graphics.lineTo(0,h);
				graphics.lineTo(0,0);
				graphics.endFill();
			}
		}
		
		private function _mouseMove(e:MouseEvent):void
		{
			
			var ds:DownloadState = findCurrentBar(e);
			
			if(ds != null){
				this.downloadSummery.downloadState = ds;
				this.tooltip.x = e.stageX + 15;
				this.tooltip.y = e.stageY + 15;
				this.tooltip.visible = true;
				this.rowOverlay_mc.visible = true;
				this.rowOverlay_mc.y = findCurrentBarY(e);
			}else{
				this.rowOverlay_mc.visible = false;
				this.tooltip.visible = false;
			}
		}

		
		private function findCurrentBar(e:MouseEvent):DownloadState
		{
			var p = this.localToGlobal(new Point(this.urlsmask_mc.x,this.mask_mc.y));
			var ux = p.x;
			
			if(e.stageX < p.x || e.stageX > p.x+this.mask_mc.width+this.urlsmask_mc.width || e.stageY > this.urlsmask_mc.height){
				return null;
			}

			var ds:DownloadState = null;
			
			p.y = e.stageY-this.graph_mc.y;
			
			for(var i = 0 ; i < hoverPoints.length; i ++)
			{
				p.x = this.graph_mc.localToGlobal(new Point(hoverPoints[i].x + hoverPoints[i].width,0)).x;
				if((e.stageX < p.x || e.stageX < ux + this.urlsmask_mc.width) && 
					p.y > hoverPoints[i].y && p.y < hoverPoints[i].y + hoverPoints[i].height){
					return ((DownloadState)(perfData.downloadData[i]));
				}
			}
			return null;
		}
		
		
		private function findCurrentBarY(e:MouseEvent):Number
		{
			var p = this.localToGlobal(new Point(this.urlsmask_mc.x,this.mask_mc.y));
			var ux = p.x;
			
			if(e.stageX < p.x || e.stageX > p.x+this.mask_mc.width+this.urlsmask_mc.width || e.stageY > this.urlsmask_mc.height){
				return -20;
			}

			var ds:DownloadState = null;
			
			p.y = e.stageY-this.graph_mc.y;

			for(var i = 0 ; i < hoverPoints.length; i ++)
			{
				p.x = this.graph_mc.localToGlobal(new Point(hoverPoints[i].x + hoverPoints[i].width,0)).x;
				if((e.stageX < p.x || e.stageX < ux + this.urlsmask_mc.width) && 
					p.y > hoverPoints[i].y && p.y < hoverPoints[i].y + hoverPoints[i].height){
					return hoverPoints[i].y+this.graph_mc.y;
				}
			}
			return -20;
		}
		
		//Resize
		private function _paneResized(e:Event):void
		{
			this.mask_mc.width = this.containerBounds.width;
			this.mask_mc.height = this.containerBounds.height;

			this.urlsmask_mc.height = this.containerBounds.height;

			this.scrollbar.y = this.containerBounds.y;
			this.scrollbar.x = this.containerBounds.width;
			this.scrollbar.height = this.containerBounds.height;
		}		
		
		private function _graphMoved(e:Event):void{this.graph_mc.x = this.graphBounds.x;}				
		private function _graphResized(e:Event):void{this.graph_mc.width = this.graphBounds.width;}		

		public function set url_width(u:Number):void
		{
			this.urlsmask_mc.width = u;
			this.urlsmask_mc.x = -u;
			this.urls_mc.x = -u;
		}
		
		public function get url_width():Number
		{
			return this.urlsmask_mc.width;
		}
		public function get scroll_width():Number
		{
			return this.scrollbar.width;
		}		
		
		// Scrollable
		public function scroll(_xp:Number,_yp:Number):void{}
		public function scrollTo(_x:Number,_y:Number):void{
			this.urls_mc.y = _y;
			this.graph_mc.y = _y;
		}
		public function getContentBounds():Rectangle{return this.graph_mc.getBounds(this);}
		public function getPaneBounds():Rectangle{return this.mask_mc.getBounds(this);}		
		
		// AbsDataGraphDisplay
		public override function onNewPerfData(perfData:PerfData):void
		{
			clearDisplay();
			
			if(perfData != null){
				drawDisplay(perfData);
			}
		}
		
		private function clearDisplay():void
		{
			this.hoverPoints = new Array();
						
			while(this.urls_mc.numChildren > 0)
				this.urls_mc.removeChild(this.urls_mc.getChildAt(0));
			
			for(var i = 0 ; i < this.displayObjects.length; i ++)
			{
				this.displayObjects[i].m.graphics.clear();
			}
			drawBG();
		}

		private function drawDisplay(perfData:PerfData):void
		{	
			var ds:DownloadState = null;
			var downloadResults = perfData.downloadData;
			
			var _x = this.graphBounds.x;
			
			var _x1 = 0;
			var _x2 = 0;
			
			var _y = 0;
			
			for(var d:int ; d < downloadResults.length ; d++)
			{
				ds = ((DownloadState)(downloadResults[d]));
				
				_y = this.graphBounds.y + (d * LINE_HEIGHT);

				this.urls_mc.addChild(createTextField(ds.url,_y));
				
				this.displayObjects[0].m.graphics.beginFill(0xfafafa);

				this.displayObjects[0].m.graphics.moveTo(_x,_y);
				this.displayObjects[0].m.graphics.lineTo(_x + ((ds.connectionStartTime - perfData.startTime)*ratio),_y);
				this.displayObjects[0].m.graphics.lineTo(_x + ((ds.connectionStartTime - perfData.startTime)*ratio),_y + (LINE_HEIGHT-1));
				this.displayObjects[0].m.graphics.lineTo(_x,_y + (LINE_HEIGHT-1));
				this.displayObjects[0].m.graphics.lineTo(_x,_y);

				if(ds.connectionEndTime == -1)
					ds.connectionEndTime = perfData.endTime;
				
				this.hoverPoints.push(new Rectangle(_x, _y, (ds.connectionEndTime - perfData.startTime)*ratio, LINE_HEIGHT));
				
				for(var i = 0 ; i < this.displayObjects.length; i ++)
				{
					if(ds[this.displayObjects[i].e] == -1 || ds[this.displayObjects[i].s] == -1){
						ds[this.displayObjects[i].e] = perfData.endTime;
					}
					
					_x1 = _x + ((ds[this.displayObjects[i].s]- perfData.startTime)*ratio);
					_x2 = _x + ((ds[this.displayObjects[i].e]- perfData.startTime)*ratio);
					
					if(_x1 == _x2)
					{
						_x2 += 1;
						_x1 -= 1;
					}
					
					this.displayObjects[i].m.graphics.moveTo(_x1,_y);
					
					this.displayObjects[i].m.graphics.beginGradientFill(GradientType.LINEAR,this.displayObjects[i].g,[ 1.0, 1.0 ],[ 0, 255 ],matrix,SpreadMethod.REPEAT,InterpolationMethod.LINEAR_RGB);
																		
					this.displayObjects[i].m.graphics.lineTo(_x2,_y);
					this.displayObjects[i].m.graphics.lineTo(_x2,_y + (LINE_HEIGHT-1));
					this.displayObjects[i].m.graphics.lineTo(_x1,_y + (LINE_HEIGHT-1));
					this.displayObjects[i].m.graphics.lineTo(_x1,_y);
					this.displayObjects[i].m.graphics.moveTo(_x1,_y);
				}
			}
			this.scrollbar.update();
			drawBG();
			this.dispatchEvent(new Event(Event.RESIZE));
		}
		
		private function createTextField(st:String,_y:Number):TextField{
			var t = new TextField();
			t.width = 1200;
			t.text = st;
			t.y = _y-2;
			t.selectable = false;
			t.setTextFormat(URL_TEXT_FORMAT);
			return t;
		}
	}
}

