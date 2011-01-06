package MSFast.Engine.Events
{
	import flash.events.Event;
	import MSFast.Engine.Data.PerfData;
	
	public class PerfDataEvent extends Event
	{
		public static const DEFAULT_NAME:String = "MSFast.Engine.Events.PerfDataEvent";
		public static const ON_NEW_DATA_RECEIVED:String = "onNewDataReceived";
		public var perfData:PerfData;

		public function PerfDataEvent($type:String, $params:PerfData, $bubbles:Boolean = false, $cancelable:Boolean = false)
		{
			super($type, $bubbles, $cancelable);
			this.perfData = $params;
		}
		public override function clone():Event
		{
			return new PerfDataEvent(type, this.perfData, bubbles, cancelable);
		}
		public override function toString():String
		{
			return formatToString("PerfDataEvent", "perfData", "type", "bubbles", "cancelable");
		}
	}
}