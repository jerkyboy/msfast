package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfDataProvider;
	import MSFast.Engine.Data.PerfData;
	import flash.geom.Rectangle;
	import flash.events.Event;
	
	public class AbsDataGraphDisplay extends MovieClip implements Zoomable
	{
		private var perfDataProvider:PerfDataProvider = null;
		
		private var _containerBounds:Rectangle;
		private var _graphBounds:Rectangle;
		private var _zoom:Number = 0;
		
		public var perfData:PerfData = null;;
		
		public static var ZOOM_RATIO:Number = 18000;
		
		public static var BASE_WIDTH:Number = 3500;
		
		public function AbsDataGraphDisplay()
		{
			this.perfDataProvider = PerfDataProvider.getInstance();
			this.perfDataProvider.addEventListener(PerfDataEvent.ON_NEW_DATA_RECEIVED, _onNewData);

			this._graphBounds = new Rectangle(0,0,BASE_WIDTH,100);
			this._containerBounds = new Rectangle(0,0,BASE_WIDTH,100);
		}
		
		private function _onNewData(e:PerfDataEvent):void{
			if(this.perfData == null) 
				perfData = e.perfData;
			
			onNewPerfData(e.perfData);
			perfData = e.perfData;
		}
		public function onNewPerfData(perfData:PerfData):void{}
		
		public function set zoom(z:Number):void{
			var b = this._zoom != z;
			this._zoom = z;
			fixZoomGraphSize();
		}		
		public function get zoom():Number{return this._zoom;}

		public function set graphBounds(r:Rectangle):void{_graphBounds = r;}
		public function get graphBounds():Rectangle{return _graphBounds;}
		
		public function set containerBounds(r:Rectangle):void{_containerBounds = r;}
		public function get containerBounds():Rectangle{return this._containerBounds;}
		
		public function set graph_x(_x:Number):void{
			var b = this._graphBounds.x != _x;
			this._graphBounds.x = _x;
			if(b)this.dispatchEvent(new Event("GraphMoved"));
		}
		public function get graph_x():Number{
			return this._graphBounds.x;
		}		
		
		public function get graph_width():Number{
			return this._graphBounds.width;
		}
		
		public override function set width(w:Number):void{
			var b = this._containerBounds.width != w;
			fixZoomGraphSize();
			this._containerBounds.width = w;
			if(b)this.dispatchEvent(new Event(Event.RESIZE));
		}
		public override function set height(h:Number):void{
			var b = this._containerBounds.height != h;
			fixZoomGraphSize();
			this._containerBounds.height = h;
			if(b)this.dispatchEvent(new Event(Event.RESIZE));
		}
		
		public function get ratio():Number{
			if(perfData == null) return 0;
			return (BASE_WIDTH / (perfData.endTime - perfData.startTime));
		}
		
		
		private function fixZoomGraphSize():void
		{
			var w = this._containerBounds.width + (ZOOM_RATIO *(this._zoom/100));
			var b = this._graphBounds.width != w;
			this._graphBounds.width = w;
			if(b)this.dispatchEvent(new Event("GraphSizeChanged"));			
		}

		public function init():void{}
		
		public override function get width():Number{return this._containerBounds.width;}		
		public override function get height():Number{return this._containerBounds.height;}		
		
		public function drawRect(mc:MovieClip)
		{
			with(mc)
			{
				graphics.clear();
				graphics.lineStyle(0,0x000000,0);
				graphics.beginFill(0xFFFFFF,0);
				graphics.moveTo(0,0);
				graphics.lineTo(this.width,0);
				graphics.lineTo(this.width,this.height);
				graphics.lineTo(0,this.height);
				graphics.lineTo(0,0);
				graphics.endFill();
			}
		}		
	}
}
