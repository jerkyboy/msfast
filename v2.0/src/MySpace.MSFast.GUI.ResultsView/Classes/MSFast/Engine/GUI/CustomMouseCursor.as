package MSFast.Engine.GUI
{
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.ui.Mouse;
	
	public class CustomMouseCursor extends Sprite {
		
		public function CustomMouseCursor()
		{
			this.visible = false;
		}
		
		public function show(e:MouseEvent)
		{
			this.visible = true;
			stage.addEventListener(Event.MOUSE_LEAVE, _mouseout);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, _mousemove);
			Mouse.hide();
			_mousemove(e);
		}
		
		public function hide(e:MouseEvent):void
		{
			stage.removeEventListener(Event.MOUSE_LEAVE, _mouseout);
			stage.removeEventListener(MouseEvent.MOUSE_MOVE, _mousemove);
			this.visible = false;
			Mouse.show();
		}                
		
		public function _mouseout(evt:MouseEvent):void
		{
			hide(evt);
		}
		
		public function _mousemove(e:MouseEvent):void
		{
			this.x = e.stageX;
			this.y = e.stageY;
		}
		
	}
}