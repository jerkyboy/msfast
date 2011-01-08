package MSFast.Engine.GUI
{
	import flash.events.MouseEvent;	
	import flash.display.MovieClip;
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.PerformanceState;
	import flash.events.Event;
	import flash.geom.*;
	import MSFast.Engine.GUI.PerformanceSummery;
	import MSFast.Engine.GUI.Popup;
	
	public class PerformanceGraph extends AbsDataGraphDisplay
	{
		private var graph_mc:MovieClip = null;
		private var mask_mc:MovieClip = null;
		private var graphOverlay_mc:MovieClip = null;
		
		private var tooltip:Popup = null;
		private var displayObjects:Array = null;
		private var performanceSummery:PerformanceSummery = null;
		
		private var results:Array = null;
		
		public function PerformanceGraph()
		{
			super();
			
			this.mask_mc = new MovieClip();
			this.graph_mc = new MovieClip();
			this.graphOverlay_mc = new MovieClip();
			
			this.bounds_mc.visible = false;
			
			addChild(this.mask_mc);
			addChild(this.graph_mc);
			addChild(this.graphOverlay_mc);

			drawRect(this.mask_mc);
			drawRect(this.graph_mc);
			
			with(this.graphOverlay_mc)
			{
				graphics.clear();
				graphics.lineStyle(0,0xCCFFFF);
				graphics.beginFill(0xCCFFFF,0.20);
				graphics.moveTo(0,0);
				graphics.lineTo(2,0);
				graphics.lineTo(2,60);
				graphics.lineTo(0,60);
				graphics.lineTo(0,0);
				graphics.endFill();
			}

			this.graph_mc.mask = (this.mask_mc);
			
			this.addEventListener(Event.RESIZE,_paneResized);
			this.addEventListener("GraphSizeChanged",_graphResized);
			this.addEventListener("GraphMoved",_graphMoved);
			
		}
		
		public override function init():void
		{
			this.tooltip = new Popup();
			this.performanceSummery = new PerformanceSummery();
			this.tooltip.addChild(this.performanceSummery);
			this.tooltip.visible = false;
			stage.addChild(this.tooltip);
			
			this.displayObjects = new Array();
			this.displayObjects.push({n:"User Time",v:"userTime",x:"maxUserTime",m:new MovieClip(),c:0x5bf447});
			this.displayObjects.push({n:"Processor Time",v:"processorTime",x:"maxProcessorTime",m:new MovieClip(),c:0x0da018});
			this.displayObjects.push({n:"Working Set",v:"workingSet",x:"maxWorkingSet",m:new MovieClip(),c:0x980076});
			this.displayObjects.push({n:"Private Working Set",v:"privateWorkingSet",x:"maxPrivateWorkingSet",m:new MovieClip(),c:0xeb0ede});
			
			for(var i = 0 ; i < this.displayObjects.length; i ++)
				this.graph_mc.addChild(this.displayObjects[i].m);
				
			stage.addEventListener(MouseEvent.MOUSE_MOVE,_mouseMove);
		}
		
		private function _mouseMove(e:MouseEvent):void
		{
			var p = this.localToGlobal(new Point(this.mask_mc.x,this.mask_mc.y));
			var ux = p.x;
			
			if(e.stageX < p.x || 
			   e.stageX > p.x+this.mask_mc.width || 
			   e.stageY < p.y || 
			   e.stageY > p.y+this.mask_mc.height){
				
				if(this.graphOverlay_mc.visible)
				{
					this.graphOverlay_mc.visible = false;
				}
				hideTooltip();
				return;
			}
			
			if(this.graphOverlay_mc.visible == false)
			{
				this.graphOverlay_mc.visible = true;
			}
			
			var mx =  (e.stageX - p.x);
			
			var before = undefined;
			var after = undefined;
			var bx = 0;
			var ax = 0;
			var s = 0;
							
			for(var d:int = 0 ; d < results.length; d++)
			{
				s = (results[d].x*(this.graph_mc.width/BASE_WIDTH))+ this.graph_mc.x;

				if(s > mx)
				{
					after = results[d].p;
					ax = s;
				}
				else if(s < mx)
				{
					before = results[d].p;
					bx = s;
				}
				else if(s == mx)
				{
					after = results[d].p;
					before = results[d].p;
					bx = s;
					ax = s;
				}
				if(after != undefined && before != undefined)
				{
					showTooltip(before,after, ((mx-bx)/(ax-bx)),e);
					
					this.graphOverlay_mc.x = bx;
					this.graphOverlay_mc.width = ax-bx;
					
					return;
				}
			}
			hideTooltip();
		}
		
		private function showTooltip(before:PerformanceState, after:PerformanceState, r:Number, e:MouseEvent):void
		{
			this.performanceSummery.setDetails(before, after, r);
			this.tooltip.x = e.stageX + 15;
			this.tooltip.y = e.stageY + 15;
			this.tooltip.visible = true;
		}
		
		private function hideTooltip():void
		{
			this.tooltip.visible = false;
		}
		
		//Resize
		private function _paneResized(e:Event):void
		{
			this.mask_mc.width = this.containerBounds.width;
			this.mask_mc.height = this.containerBounds.height;
			this.graphBackground.width = this.containerBounds.width;
		}		
		
		private function _graphResized(e:Event):void{this.graph_mc.width = this.graphBounds.width;}
		private function _graphMoved(e:Event):void{this.graph_mc.x = this.graphBounds.x;}				

		public override function onNewPerfData(perfData:PerfData):void
		{
			clearDisplay();
			if(perfData != null)
			{
				drawDisplay(perfData);
			}
		}

		private function drawDisplay(perfData:PerfData):void
		{	
			var perfResults = perfData.performanceResults;
			
			var i = 0;
			var ps:PerformanceState = null;
			var _x = this.graphBounds.x;
			var _y = this.graphBounds.y + 2;
			var h = 60;

			for(i = 0 ; i < this.displayObjects.length; i ++)
			{
				this.displayObjects[i].m.graphics.moveTo(_x,this.graphBounds.y+h);
				this.displayObjects[i].m.graphics.lineStyle(0,this.displayObjects[i].c);
			}
			
			results = new Array();
			
			for(var d:int ; d < perfResults.length; d++)
			{
				ps = ((PerformanceState)(perfResults[d]));
				
				results.push({p:ps,x:(((ps.timeStamp-perfData.startTime)*ratio))});
				
				for(i = 0 ; i < this.displayObjects.length; i ++)
				{
					this.displayObjects[i].m.graphics.lineTo(_x + ((ps.timeStamp-perfData.startTime) * ratio),
															 _y + (h - ((ps[this.displayObjects[i].v] / perfData[this.displayObjects[i].x]) * h)));
				}
			}
		}
		
		
		private function clearDisplay():void
		{
			//this.graph_mc.graphics.clear();
			
			for(var i = 0 ; i < this.displayObjects.length; i ++)
			{
				this.displayObjects[i].m.graphics.clear();
			}
		}
	}
}

