package MSFast.Engine.Data
{
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.PerformanceState;
	import MSFast.Engine.Data.DownloadState;
	import MSFast.Engine.Data.RenderedPeice;
	
	import flash.events.EventDispatcher;
	
	public class PerfDataXMLReader extends EventDispatcher
	{
		public static function loadData(url:String):PerfData
		{
			var xmlLoader:URLLoader = new URLLoader(); 
			var xmlData:XML = new XML(); 
			  
			xmlLoader.addEventListener(Event.COMPLETE, LoadXML); 
			  
			xmlLoader.load(new URLRequest("http://www.kirupa.com/net/files/sampleXML.xml")); 
			  
			function LoadXML(e:Event):void { 
				xmlData = new XML(e.target.data); 
				trace(xmlData); 
			} 
		}
	}
}