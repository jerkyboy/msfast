package {

	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.ui.Mouse;
	
	public class Test extends Sprite {
		
		private var cursor:Sprite = new Sprite();
		
		public function Test()
		{
			cursor.graphics.beginFill(0xFF);
			cursor.graphics.drawRect(0, 0, 25, 25);
			addChild(cursor);
			stage.addEventListener(Event.MOUSE_LEAVE, cursorHide);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, cursorFollow);
			Mouse.hide();
		}
		
		public function cursorHide(evt:Event):void
		{
			cursor.visible = false;
		}                
		
		public function cursorFollow(evt:MouseEvent):void
		{
			if (!cursor.visible) cursor.visible = true;
			cursor.x = stage.mouseX;
			cursor.y = stage.mouseY;
			evt.updateAfterEvent();
		}
	}
}