
package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import flash.geom.*;
	import MSFast.Engine.GUI.Scrollable;
	import flash.events.Event;
	
	public class Navigator extends MovieClip
	{
		private var scrollable:Scrollable;
				
		private var dragO:Number = 0;
		private var dragX:Number = 0;
		
		private var attachedToStage:Boolean = false;
		
		public function Navigator()
		{
			this.marker.x = this.trackbar.x;
			update();
		}
		
		private function _parentResized(e:Event):void
		{
			update();
		}		

		public function setScrollable(s:Scrollable):void
		{
			if(this.scrollable != null){
				MovieClip(this.scrollable).removeEventListener(Event.RESIZE,_parentResized);
			}
				
			this.scrollable = s;
			
			MovieClip(this.scrollable).addEventListener(Event.RESIZE,_parentResized);
		}
		
		public override function set width(h:Number):void
		{
			this.trackbar.width = 100 + (h-130);
		}
		
		public function update():void
		{
			var pane:Rectangle = null;
			var content:Rectangle = null;
			
			if(this.scrollable != null){
				pane = this.scrollable.getPaneBounds();
				content = this.scrollable.getContentBounds();
			}
			
			if(pane == null || content == null || pane.width > content.width)
			{
				this.marker.visible = false;
				this.trackbar.removeEventListener(MouseEvent.MOUSE_DOWN, gotoClick);
				this.marker.removeEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
			}
			else
			{
				this.marker.visible = true;
				this.marker.width = this.trackbar.width * (pane.width / content.width);
				this.marker.x = (Math.abs(content.x / content.width)*(this.trackbar.width));// * (this.trackbar.width/pane.width));
				this.trackbar.addEventListener(MouseEvent.MOUSE_DOWN, gotoClick);
				this.marker.addEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
			}
		}
		
	
		private function gotoClick(e:MouseEvent):void
		{
			onMousemove(e);
		}
		
		private function onMousedown(e:MouseEvent):void
		{
			dragO = e.stageX - this.marker.x;
			
			stage.addEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			this.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
	
		private function onMousemove(e:MouseEvent):void
		{
			var i = e.stageX - dragO;

			if(i > (this.trackbar.x + this.trackbar.width)-this.marker.width){
				i = (this.trackbar.x + this.trackbar.width)-this.marker.width;
			}else if(i < this.trackbar.x){
				i = this.trackbar.x;
			}

			i = (i-this.trackbar.x) / (((this.trackbar.width)-this.marker.width)) * 100;
			
			this.scrollX = i;
			
			fireScroll(i,0);
		}
		
		public function set scrollX(s:Number):void
		{
			if(s > 100) s = 100;
			if(s < 0) s = 0;
			this.marker.x = this.trackbar.x + ((s/100)*(this.trackbar.width-this.marker.width));
		}
		
		public function get scrollX():Number
		{
			var s = (this.marker.x-this.trackbar.x) / (((this.trackbar.width)-this.marker.width)) * 100;
			if(s > 100) s = 100;
			if(s < 0) s = 0;
			return s;
		}

		private function onMouseupout(e:Event):void
		{
			stage.removeEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.removeEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			this.removeEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.removeEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
		
		private function scrollUp(e:MouseEvent):void
		{
			this.scrollX -= 15;
			fireScroll(this.scrollX,0);
		}
		
		private function scrollDown(e:MouseEvent):void
		{
			this.scrollX += 15;
			fireScroll(this.scrollX,0);
		}
		
		private function fireScroll(_x:Number,_y:Number)
		{
			if(this.scrollable != null){
				this.scrollable.scroll(this.scrollX,0);
				this.scrollable.scrollTo(precentesToX(this.scrollX),0);
			}
		}
		
		private function precentesToX(precents:Number)
		{
			var pane:Rectangle = null;
			var content:Rectangle = null;
			
			if(this.scrollable != null){
				pane = this.scrollable.getPaneBounds();
				content = this.scrollable.getContentBounds();
			}
			
			if(pane == null || content == null || pane.width >= content.width)
				return 0;
				
			return -((content.width - pane.width) * (precents/100));
		}
		
	}
}
