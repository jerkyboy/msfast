package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.events.MouseEvent;
	import flash.geom.Rectangle;
	import flash.events.Event;
	import MSFast.Engine.GUI.CustomMouseCursor;
	
	public class Seperator extends MovieClip
	{

		private var dragO:Number = 0;
		private var mouseCursor:CustomMouseCursor = null;
		
		public function Seperator()
		{
			this.hover.visible = !(this.idle.visible = true);
			this.addEventListener(MouseEvent.MOUSE_DOWN, onMousedown);
			this.addEventListener(MouseEvent.MOUSE_MOVE, onMousemoveCursor);
			this.addEventListener(MouseEvent.MOUSE_OUT, onMouseoutCursor);
		}
		
		private function onMousedown(e:MouseEvent):void
		{
			this.parent.setChildIndex(this,this.parent.numChildren-1);
			this.hover.visible = true;

			dragO = e.stageX - this.hover.x;
			
			stage.addEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			this.addEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
		}
	
		private function onMouseoutCursor(e:MouseEvent):void
		{
			if(mouseCursor != null && mouseCursor.visible != false)
				mouseCursor.hide(e);
		}	
		
		private function onMousemoveCursor(e:MouseEvent):void
		{
			if(mouseCursor == null)
			{
				this.mouseCursor = new CustomMouseCursor();
				stage.addChild(this.mouseCursor);
			}
			
			if(mouseCursor.visible == false){
				mouseCursor.show(e);
			}
				
		}
		
		private function onMousemove(e:MouseEvent):void
		{
			var i = e.stageX - dragO;
			if(i + this.x < 0)
				i = -this.x;
			else if(i+this.x > stage.stageWidth-this.hover.width)
				i = stage.stageWidth-this.x-this.hover.width;
				
			this.hover.x = i;
		}
		
		private function onMouseupout(e:Event):void
		{
			mouseCursor.hide(null);
			
			this.x = this.x + this.hover.x;
			this.idle.x = 0;
			this.hover.x = 0;
			this.hover.visible = !(this.idle.visible = true);
			
			stage.removeEventListener(Event.MOUSE_LEAVE, onMouseupout);
			stage.removeEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			this.removeEventListener(MouseEvent.MOUSE_UP, onMouseupout);
			stage.removeEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
			
			this.dispatchEvent(new Event("OnMove"));
			
		}
	}
}
