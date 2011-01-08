package MSFast.Engine.Data
{
	import flash.utils.Dictionary;
	
	public class PerfData
	{
		public var testId:int = -1;
		
		public var performanceResults:Array = null;
		public var maxProcessorTime:Number = 0;
		public var minProcessorTime:Number = 0;
		public var maxUserTime:Number =  0;
		public var minUserTime:Number =  0;
		public var maxWorkingSet:int = 0;
		public var minWorkingSet:int = 0;
		public var maxPrivateWorkingSet:int = 0;
		public var minPrivateWorkingSet:int = 0;

		public var markersData:Array  = null;
		public var downloadData:Array = null;
		public var renderData:Array  = null;

  		public var timeReadyStateUninitialized:Number = -1;
        public var timeReadyStateLoading:Number = -1;
        public var timeReadyStateLoaded:Number = -1;
        public var timeReadyStateInteractive:Number = -1;
        public var timeReadyStateComplete:Number = -1;
        public var timeOnLoad:Number = -1;		
		
		
		public var thumbnail_path:String = "http://perftracker.ms.fimdev.com/perfdump/";
		
		public var startTime:Number = 0;
		public var endTime:Number = 0;
	}
}