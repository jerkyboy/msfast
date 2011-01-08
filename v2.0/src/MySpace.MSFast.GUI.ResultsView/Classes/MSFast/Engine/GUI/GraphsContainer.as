package MSFast.Engine.GUI
{
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.PerfDataProvider;

	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.geom.Rectangle;
	import flash.geom.Point;
	import flash.utils.setInterval;
	import flash.utils.clearInterval;
	
	import MSFast.Engine.GUI.Zoomable;
	import MSFast.Engine.GUI.DownloadGraph;
	import MSFast.Engine.GUI.PerformanceGraph;
	import MSFast.Engine.GUI.ThumbnailsRenderer;
	import MSFast.Engine.GUI.Seperator;
	import MSFast.Engine.GUI.MapRead;
	import MSFast.Engine.GUI.PopupsListener;
	import MSFast.Engine.GUI.InternalPopupsListener;
	
	public class GraphsContainer extends MovieClip implements Zoomable
	{
		private var _zoom:Number = 0;
		private var _width:Number = 0;
		private var _height:Number = 0;
		
		private var downloadGraph:DownloadGraph = null;
		private var performanceGraph:PerformanceGraph = null;
		private var thumbnailsRenderer:ThumbnailsRenderer = null;
		private var markersGraph:MarkersGraph = null;
		private var wireframe:MovieClip = null;
		private var mapRead:MapRead = null;
		
		private var seperator:Seperator = null;
		
		private var graphs_x:Number = 0;
		private var dragO:Number = 0;
		private var dragX:Number = 0;
		
		private var popupsListener:PopupsListener = undefined;
		
		private var downloadHeight:Number = 140;
		
		private var mouse_x:Number = 0;
		
		public function setPopupListener(_popupsListener:PopupsListener):void
		{
			this.popupsListener = _popupsListener;
			
			this.thumbnailsRenderer.addEventListener("ShowPairPopup",function(e){
				popupsListener.showPairPopup(e.params["txt"],e.params["urla"],e.params["urlb"]);
			});
			this.thumbnailsRenderer.addEventListener("HidePairPopup",function(e){
				popupsListener.hidePairPopup();
			});
			this.thumbnailsRenderer.addEventListener("ShowThumbPopup",function(e){
				popupsListener.showThumbnailPopup(e.params["url"]);
			});
			this.thumbnailsRenderer.addEventListener("HideThumbPopup",function(e){
				popupsListener.hideThumbnailPopup();
			});
			this.thumbnailsRenderer.addEventListener("ShowSourcePopup",function(e){
				popupsListener.showSourcePopup(e.params["s"],e.params["l"],e.params["p"]);
			});
		} 
		
		public function GraphsContainer()
		{
			this._width = this.bounds_mc.width;
			this._height = this.bounds_mc.height;
			
			this.bounds_mc.visible = false;
			
			this.downloadGraph = new DownloadGraph();
			this.performanceGraph = new PerformanceGraph();
			this.thumbnailsRenderer = new ThumbnailsRenderer();
			this.markersGraph = new MarkersGraph();
			
			this.wireframe = new MovieClip();
			this.seperator = new Seperator();
			this.mapRead = new MapRead(this);
			
			this.seperator.x = 200;
			
			addChild(this.downloadGraph);
			addChild(this.performanceGraph);
			addChild(this.thumbnailsRenderer);
			addChild(this.markersGraph);
			addChild(this.wireframe);
			addChild(this.seperator);
			addChild(this.mapRead);
			
			this.downloadGraph.visible = false;
			this.performanceGraph.visible = false;
			this.thumbnailsRenderer.visible = false;
			this.markersGraph.visible = false;
			this.wireframe.visible = false;
			this.seperator.visible = false;
			this.mapRead.visible = false;
			
			this.downloadGraph.init();
			this.performanceGraph.init();
			this.thumbnailsRenderer.init();
			this.markersGraph.init();
			this.mapRead.init();
			
			PerfDataProvider.getInstance().addEventListener(PerfDataEvent.ON_NEW_DATA_RECEIVED, function(){
				dataLoaded();
			});
			
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onSaveMouse);
			this.seperator.addEventListener("OnMove",function(){fixSizeAndPosition();});
			stage.addEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
			this.addEventListener(MouseEvent.MOUSE_WHEEL, onMousewheel);
		}
		
		private function onSaveMouse(e:MouseEvent):void
		{
			mouse_x = e.stageX;
		}		
		
		private function dataLoaded()
		{
			this.pleaseWait.visible = false;
			this.seperator.visible = true;
			this.downloadGraph.visible = true;
			this.performanceGraph.visible = true;
			this.thumbnailsRenderer.visible = true;
			this.markersGraph.visible = true;
			this.wireframe.visible = true;			
			this.mapRead.visible = true;
			
			zoom = 0;
		}
		
		
		
		
		
		public function set zoom(z:Number):void
		{
			var m = this.mouse_x-(this.seperator.x+this.seperator.width);
			
			if(m < 0)
			  m = (this.width - this.downloadGraph.url_width - this.downloadGraph.scroll_width)/2;
			  
			var _a = (Math.abs(this.downloadGraph.graphBounds.x) + m ) / this.downloadGraph.graphBounds.width;//p.x/this.performanceGraph.width;
			
			dragX = -(((z/100)*AbsDataGraphDisplay.ZOOM_RATIO));			
			this._zoom = z;
			this.downloadGraph.zoom = z;
			this.performanceGraph.zoom = z;
			this.thumbnailsRenderer.zoom = z;
			this.markersGraph.zoom = z;
			this.setGraph_X(-((this.downloadGraph.graphBounds.width*_a)-(m)  ));
			this.mapRead.setZoom(z);
		}
 		public function get zoom():Number{return this._zoom;}

		public override function set width(w:Number):void{this._width = w;fixSizeAndPosition();}
		public override function set height(h:Number):void{this._height = h;fixSizeAndPosition();}
		
		public override function get width():Number{return this._width;}		
		public override function get height():Number{return this._height;}		
		
		private function fixSizeAndPosition():void
		{
			this.seperator.height = this.height;
			this.seperator.x = Math.max(this.seperator.x, 200 );
			this.seperator.x = Math.min(this.seperator.x, this.width - this.downloadGraph.scroll_width - this.seperator.width);
			
			this.pleaseWait.x = (this.width - this.pleaseWait.width)/2;
			this.pleaseWait.y = (this.height - this.pleaseWait.height)/2;
			
			this.downloadGraph.url_width = this.seperator.x+this.seperator.width;
			
			var w = this.width - this.downloadGraph.url_width - this.downloadGraph.scroll_width;
			
			this.downloadGraph.width = w;
			this.downloadGraph.height = (this.height - downloadHeight);
			this.downloadGraph.x = this.downloadGraph.url_width;
			this.downloadGraph.y = 0;

			this.performanceGraph.width = w;
			this.performanceGraph.height = 65;
			this.performanceGraph.x = this.downloadGraph.url_width;
			this.performanceGraph.y = this.downloadGraph.y + this.downloadGraph.height;
			
			this.thumbnailsRenderer.width = w;
			this.thumbnailsRenderer.height = this.height;//ThumbnailsPair.DEFAULT_HEIGHT;
			this.thumbnailsRenderer.x = this.downloadGraph.url_width;
			this.thumbnailsRenderer.y = 0;
			
			this.markersGraph.width = w;
			this.markersGraph.height = this.height;
			this.markersGraph.x = this.downloadGraph.url_width;
			this.markersGraph.y = 0;
			
			this.mapRead.y = this.height - (this.mapRead.height);
			
			this.mapRead.width = this.seperator.x;
			
			drawWireframe();
		}
		
		private function drawWireframe()
		{
			this.wireframe.x = 0;
			this.wireframe.y = 0;
			
			with(this.wireframe.graphics){
				clear();
				lineStyle(0,0xcccccc);
				moveTo(0,0);
				lineTo(this.width-1,0);
				lineTo(this.width-1,this.height-1);
				lineTo(0,this.height-1);
				lineTo(0,0);
				
				moveTo(0,(this.height - downloadHeight));
				lineTo(this.width-1,(this.height - downloadHeight));
				
				moveTo(this.downloadGraph.url_width,0);
				lineTo(this.downloadGraph.url_width,this.height);
				
				moveTo(this.width - this.downloadGraph.scroll_width ,(this.height - downloadHeight));
				lineTo(this.width - this.downloadGraph.scroll_width,this.height);
			}
		}
		
		private function onMousewheel(e:MouseEvent):void
		{
			if(!e.altKey)
				return;
	
			var d = e["delta"];

			if(d > 0 && this._zoom < 100){
				this.zoom = Math.min(this._zoom + 5,100);
			}else if(d < 0 && this._zoom > 0){
				this.zoom = Math.max(_zoom - 5,0);
			}
		}

		
		private function onMousedown(e:MouseEvent):void
		{
			if(e.stageX < this.seperator.x || e.stageX > this.width - this.downloadGraph.scroll_width){
				return ;
			}
			if(this._zoom == 0)
				return;
				
			dragO = e.stageX - graphs_x;
			
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.addEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
		
		private function onMouseupout(e:Event):void
		{
			stage.removeEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.removeEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.removeEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
				
		private function onMousemove(e:MouseEvent):void
		{
			setGraph_X(e.stageX - dragO);
		}
		
		public function setGraph_X(xx:Number):void
		{
			if(xx > 0){
				xx = 0;
			}else if(xx < dragX){
				xx = dragX;
			}
			
			graphs_x = xx;
			
			this.downloadGraph.graph_x = xx;
			this.performanceGraph.graph_x = xx;
			this.thumbnailsRenderer.graph_x = xx;
			this.markersGraph.graph_x = xx;
			
			this.mapRead.update();
		}
		
		
		public function getGraphBounds():Rectangle
		{
			return this.downloadGraph.graphBounds;
		}
		public function getContainerBounds():Rectangle
		{
			return this.downloadGraph.containerBounds;
		}
		
		
	}
}


