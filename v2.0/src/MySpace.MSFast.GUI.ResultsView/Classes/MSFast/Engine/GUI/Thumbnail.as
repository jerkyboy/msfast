package MSFast.Engine.GUI
{
	import flash.display.*;
	import flash.events.*;
	import flash.net.URLRequest;
	
	public class Thumbnail extends Sprite
	{
		public static var DEFAULT_WIDTH:Number = 113;
		public static var DEFAULT_HEIGHT:Number = 81;
		
		private var _width:Number = DEFAULT_WIDTH;
		private var _height:Number = DEFAULT_HEIGHT;
		
		private var url:String = "";
		
		private var thumbnail:MovieClip = null;
		
		private var cache:Object = new Object();
		
		public function Thumbnail()
		{
			this.bounds_mc.visible = false;
			this.thumbnail = new MovieClip();

			this.thumbnail.graphics.clear();
			this.thumbnail.graphics.lineStyle(0,0x3c7fb1,0);
			this.thumbnail.graphics.moveTo(1,1);
			this.thumbnail.graphics.lineTo(2,1);
			this.thumbnail.graphics.lineTo(2,2);
			this.thumbnail.graphics.lineTo(1,2);
			this.thumbnail.graphics.lineTo(1,1);
			
			addChild(this.thumbnail);
			
			redrawBG();
		}

		public function loadThumbnail(s:String):void
		{
			if(s != url){
				url = s;
				
				if(cache[url] != null)
				{
					showThumbnail();
				}
				else
				{
					var loader = new Loader();
					
					cache[url] = loader;
					
					loader.contentLoaderInfo.addEventListener(Event.OPEN,showPreloader);
					loader.contentLoaderInfo.addEventListener(ProgressEvent.PROGRESS,showProgress);
					loader.contentLoaderInfo.addEventListener(Event.COMPLETE,showLoadResult);
					
					var request:URLRequest = new URLRequest(s);
					loader.load(request);
				}
			}
		}
		
		private function showPreloader(ev:Event):void{
     		this.preloader.visible = true;
			this.thumbnail.visible = false;
 		}
		
		private function showProgress(ev:Event):void{}
		
		private function showLoadResult(ev:Event):void
		{
			showThumbnail();
 		}
		
		private function showThumbnail():void
		{
     		this.preloader.visible = false;

			while(this.thumbnail.numChildren > 0)
				this.thumbnail.removeChild(this.thumbnail.getChildAt(0));
			
			this.thumbnail.addChild(cache[url]);
			cache[url].width = this._width;
			cache[url].height = this._height;
			this.thumbnail.visible = true;			
		}
		
		public override function set width(__width:Number):void{
			this._width = __width;
			redrawBG();
		}
		
		public override function set height(__height:Number):void{
			this._height = __height;
			redrawBG();
		}
		
		private function redrawBG():void
		{
			this.graphics.clear();
			this.graphics.beginFill(0xFFFFFF);
			this.graphics.lineStyle(0,0x3c7fb1);
			this.graphics.moveTo(-1,-1);
			this.graphics.lineTo(this._width+2,-1);
			this.graphics.lineTo(this._width+2,this._height+2);
			this.graphics.lineTo(-1,this._height+2);
			this.graphics.lineTo(-1,-1);
			this.graphics.endFill();
		}

		
		public override function get width():Number{return this._width;}
		public override function get height():Number{return this._height;}
				
	}
}
