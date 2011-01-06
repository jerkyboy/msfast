package MSFast.Engine.Data
{
	import flash.utils.Dictionary;
	
	public class PerfData
	{
		public var testId:int = -1;
		/*
		public var folderName:String = null;

		// HTML Source Data
		public var sourceDumpFilename:String = null;
		public var savedSource:Dictionary  = null;
		public var sourceDumpBreaks:Array = null;
		public var sourceDumpBreakResolution:int = 0;
		
		// HTML Source Validation
		public var htmlSourceValidationResults:Array = null; 

		// Performance Data
		*/
		public var performanceResults:Array = null;
		/*
		public var performanceDumpFilename:String = null;
		*/
		public var maxProcessorTime:Number = 0;
		public var minProcessorTime:Number = 0;
		//public var avgProcessorTime:Number = 0;
		public var maxUserTime:Number =  0;
		public var minUserTime:Number =  0;
		//public var avgUserTime:Number = 0;
		public var maxWorkingSet:int = 0;
		public var minWorkingSet:int = 0;
		//public var avgWorkingSet:int = 0;
		public var maxPrivateWorkingSet:int = 0;
		public var minPrivateWorkingSet:int = 0;
		//public var avgPrivateWorkingSet:int = 0;
		/*
		// Render Data
		public var renderDumpFilename:String = null;
		*/
		public var renderData:Array  = null;
  		public var timeReadyStateUninitialized:Number = -1;
        public var timeReadyStateLoading:Number = -1;
        public var timeReadyStateLoaded:Number = -1;
        public var timeReadyStateInteractive:Number = -1;
        public var timeReadyStateComplete:Number = -1;
        public var timeOnLoad:Number = -1;		
		/*
		public var maxRenderTime:Number = 0;
		public var minRenderTime:Number = 0;
		public var avgRenderTime:Number = 0;
		
		// Download Data
		public var downloadDumpFilename:String = null;
		*/
		public var downloadData:Array = null;
		/*
		public var totalCSS:int = 0;
		public var totalCSSWeight:int = 0;
		public var totalImages:int = 0;
		public var totalImagesWeight:int = 0;
		public var totalJS:int = 0;
		public var totalJSWeight:int = 0;
		public var totalFiles:int = 0;
		public var totalDataReceived:int = 0;
		public var totalDataSent:int = 0;
		
		// Thumbnails Data
		public var defaultThumbnail:String = null;
		public var thumbnailsFilenameStructure:String = null;
		public var thumbnailsCount:int = 0;
		*/
		
		public var thumbnail_path:String = "http://perftracker.ms.fimdev.com/perfdump/";
		
		public var startTime:Number = 0;
		public var endTime:Number = 0;
	}
}