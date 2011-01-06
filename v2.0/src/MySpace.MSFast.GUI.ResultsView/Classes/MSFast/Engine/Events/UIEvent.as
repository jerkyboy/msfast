package MSFast.Engine.Events
{
	import flash.events.Event;
	
	public class UIEvent extends Event
	{
		public static const DEFAULT_NAME:String = "MSFast.Engine.Events.UIEvent";
		public static const ON_HEIGHT_CHANGED:String = "onHeightChanged";
		public var params:Object;

		public function UIEvent($type:String, $params:Object, $bubbles:Boolean = false, $cancelable:Boolean = false)
		{
			super($type, $bubbles, $cancelable);
			this.params = $params;
		}
		public override function clone():Event
		{
			return new UIEvent(type, this.params, bubbles, cancelable);
		}
		public override function toString():String
		{
			return formatToString("onHeightChanged", "params", "type", "bubbles", "cancelable");
		}
	}
}