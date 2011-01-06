
package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import flash.geom.*;
	import MSFast.Engine.GUI.Scrollable;
	import flash.events.Event;
	
	public class Scrollbar extends MovieClip
	{
		private var scrollable:Scrollable;
				
		private var dragO:Number = 0;
		private var dragX:Number = 0;
		
		private var attachedToStage:Boolean = false;
		
		public function Scrollbar()
		{
			this.marker.x = this.trackbar.x;
			this.marker.y = this.trackbar.y;
			
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
			if(attachedToStage == false){
				stage.addEventListener(MouseEvent.MOUSE_WHEEL,_onMousewheel);
				attachedToStage =true;
			}
		}
		
		public override function set height(h:Number):void
		{
			this.trackbar.height = 100 + (h-130);
			this.upBtn.y = this.trackbar.y - 15;
			this.downBtn.y = this.trackbar.y + this.trackbar.height;
		}
		
		public function update():void
		{
			var pane:Rectangle = null;
			var content:Rectangle = null;
			
			if(this.scrollable != null){
				pane = this.scrollable.getPaneBounds();
				content = this.scrollable.getContentBounds();
			}
			
			if(pane == null || content == null || pane.height > content.height)
			{
				this.marker.visible = false;
				this.trackbar.removeEventListener(MouseEvent.MOUSE_DOWN, gotoClick);
				this.marker.removeEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
				this.upBtn.removeEventListener(MouseEvent.MOUSE_DOWN, scrollUp);
				this.downBtn.removeEventListener(MouseEvent.MOUSE_DOWN, scrollDown);
			}else{
				this.marker.visible = true;
				this.marker.height = this.trackbar.height * (pane.height / content.height);
				this.marker.y = (Math.abs(content.y / content.height)*(this.trackbar.height));// * (this.trackbar.width/pane.width));

				this.trackbar.addEventListener(MouseEvent.MOUSE_DOWN, gotoClick);
				this.marker.addEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
				this.upBtn.addEventListener(MouseEvent.MOUSE_DOWN, scrollUp);
				this.downBtn.addEventListener(MouseEvent.MOUSE_DOWN, scrollDown);
			}
		}
		
		private function _onMousewheel(e:MouseEvent):void
		{

			if(e.ctrlKey || e.shiftKey || e.altKey || e.stageY > this.scrollable.getPaneBounds().height)
				return;
				
			var d = e["delta"];
			if(d > 0){
				scrollUp(e);
			}else if(d < 0){
				scrollDown(e);
			}
		}
		
		private function gotoClick(e:MouseEvent):void
		{
			onMousemove(e);
		}
		
		private function onMousedown(e:MouseEvent):void
		{
			dragO = e.stageY - this.marker.y;
			
			stage.addEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			this.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
	
		private function onMousemove(e:MouseEvent):void
		{
			var i = e.stageY - dragO;

			if(i > (this.trackbar.y + this.trackbar.height)-this.marker.height){
				i = (this.trackbar.y + this.trackbar.height)-this.marker.height;
			}else if(i < this.trackbar.y){
				i = this.trackbar.y;
			}

			i = (i-this.trackbar.y) / (((this.trackbar.height)-this.marker.height)) * 100;
			
			this.scrollY = i;
			
			fireScroll(0,i);
		}
		
		public function set scrollY(s:Number):void
		{
			if(s > 100) s = 100;
			if(s < 0) s = 0;
			this.marker.y = this.trackbar.y + ((s/100)*(this.trackbar.height-this.marker.height));
		}
		
		public function get scrollY():Number
		{
			var s = (this.marker.y-this.trackbar.y) / (((this.trackbar.height)-this.marker.height)) * 100;
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
			this.scrollY -= 15;
			fireScroll(0,this.scrollY);
		}
		
		private function scrollDown(e:MouseEvent):void
		{
			this.scrollY += 15;
			fireScroll(0,this.scrollY);			
		}
		
		private function fireScroll(_x:Number,_y:Number)
		{
			if(this.scrollable != null){
				this.scrollable.scroll(0,this.scrollY);
				this.scrollable.scrollTo(0,precentesToY(this.scrollY));
			}
		}
		
		private function precentesToY(precents:Number)
		{
			var pane:Rectangle = null;
			var content:Rectangle = null;
			
			if(this.scrollable != null){
				pane = this.scrollable.getPaneBounds();
				content = this.scrollable.getContentBounds();
			}
			
			if(pane == null || content == null || pane.height > content.height)
				return 0;
				
			return -((content.height - pane.height) * (precents/100));
		}
		
	}
}
