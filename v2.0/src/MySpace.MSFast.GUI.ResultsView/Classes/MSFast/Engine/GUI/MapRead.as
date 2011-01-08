
package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.text.TextField;
	import fl.controls.Slider;
	import fl.events.SliderEvent;
	import flash.events.MouseEvent;
	import MSFast.Engine.GUI.*;
	import flash.geom.*;

	public class MapRead extends MovieClip implements Scrollable
	{
		
		private var _width:Number = 0;
		private var owner:GraphsContainer=null;
		
		public function MapRead(own:GraphsContainer)
		{
			this.owner = own;
			
			this._width = 200;
			
			this.zoomsldr.addEventListener(SliderEvent.CHANGE, sliderChanged);
			this.help_icon.addEventListener(MouseEvent.MOUSE_MOVE,showHelp);
			this.help_icon.addEventListener(MouseEvent.MOUSE_OUT,hideHelp);
			this.zoomicon.addEventListener(MouseEvent.MOUSE_DOWN,zoomToZero);

			this.zoomtxt.text = "100%";
			
		}
		
		public function init():void
		{
			this.smallmap.setScrollable(this);
			fixPositions();
		}
		
		private function zoomToZero(e:MouseEvent):void
		{
			setZoom(0);
			this.owner.zoom = 0;
		}
		
		public function showHelp(e:MouseEvent):void
		{
			this.parent.setChildIndex(this,this.parent.numChildren-1);
			
			this.guide.scaleX = 1;
			this.guide.visible = true;
		}
		
		public function hideHelp(e:MouseEvent):void
		{
			this.guide.scaleX = 0;
			this.guide.visible = false;
		}
		
		
		public function setZoom(z:Number):void
		{
			this.zoomtxt.text = Math.max(100,(100*z)) + "%";
			this.zoomsldr.value = z;
			this.smallmap.update();
		}
		
		private function sliderChanged(e:SliderEvent):void
		{
			setZoom(this.zoomsldr.value);
			this.owner.zoom = Math.max(this.zoomsldr.value , 0);
		}
		
		private function fixPositions()
		{
			var bw = 200;
			this.guide.visible = false;
			this.guide.scaleX = 0;
			this.help_icon.x = bw - (this.help_icon.width+4);
			this.guide.x = this.help_icon.x;
			
			this.logo.x = 4;
			
			this.shadow_left.x = 4;
			this.shadow_bg.x = this.shadow_left.x + this.shadow_left.width;
			this.shadow_bg.width = bw - this.shadow_right.width - this.shadow_left.x - this.shadow_left.width;
			this.shadow_right.x = bw - (this.shadow_right.width + 4);
			
			this.zoomsldr.x = this.shadow_bg.x;
			this.zoomicon.x = bw - (this.zoomicon.width + 10);
			this.zoomtxt.x  = this.zoomicon.x - (this.zoomtxt.width + 2);
			this.zoomsldr.width = this.shadow_bg.width - (this.zoomicon.width + this.zoomtxt.width);
			
			this.smallmap.x = 4;
			
			this.graphics.clear();
			
			this.graphics.lineStyle(0,0xcccccc);
			this.graphics.moveTo(0,this.logo.y - 4);
			this.graphics.lineTo(this.width,this.logo.y-4);
			this.graphics.moveTo(0,this.shadow_left.y - 4);
			this.graphics.lineTo(this.width,this.shadow_left.y-4);

			this.graphics.moveTo(0,this.logo.y - 3);
			this.graphics.lineStyle(0,0x000000);
			this.graphics.beginFill(0x000000,1);
			this.graphics.lineTo(0,this.logo.y + this.logo.height);
			this.graphics.lineTo(this.width,this.logo.y + this.logo.height);
			this.graphics.lineTo(this.width,this.logo.y -3);
			this.graphics.lineTo(0,this.logo.y -3);

			this.graphics.endFill();
			
			this.smallmap.update();
		}
		
		public override function set width(w:Number):void{this._width = w;fixPositions()}
		public override function get width():Number{return this._width;}
		
		
		public function scroll(_xp:Number,_yp:Number):void{}
		public function scrollTo(_x:Number,_y:Number):void{
			this.owner.setGraph_X(_x);
		}		
		public function getPaneBounds():Rectangle{
			return this.owner.getContainerBounds();
		}
		public function getContentBounds():Rectangle{
			return this.owner.getGraphBounds();
		}
		
		public function update():void
		{
			this.smallmap.update();
		}
	}
}
