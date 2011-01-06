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

			/*var pd:PerfData = new PerfData();

			pd.startTime = 1000;
			pd.endTime = 10000;
			pd.performanceResults = new Array();
			pd.testId = 81;
			for(var i = 0 ; i < 1000; i ++)
			{
				pd.performanceResults.push(new PerformanceState(pd.startTime + (i * ((pd.endTime-pd.startTime)/1000)),
																Math.random()*100,
																Math.random()*100,
																Math.random()*8000,
																Math.random()*8000));
				
				pd.maxPrivateWorkingSet = Math.max(pd.maxPrivateWorkingSet,pd.performanceResults[pd.performanceResults.length-1].privateWorkingSet);
				pd.minPrivateWorkingSet = Math.min(pd.minPrivateWorkingSet,pd.performanceResults[pd.performanceResults.length-1].privateWorkingSet);
				
				pd.maxWorkingSet = Math.max(pd.maxWorkingSet,pd.performanceResults[pd.performanceResults.length-1].workingSet);
				pd.minWorkingSet = Math.min(pd.minWorkingSet,pd.performanceResults[pd.performanceResults.length-1].workingSet);
				
				pd.maxProcessorTime = Math.max(pd.maxProcessorTime,pd.performanceResults[pd.performanceResults.length-1].processorTime);
				pd.minProcessorTime = Math.min(pd.minProcessorTime,pd.performanceResults[pd.performanceResults.length-1].processorTime);
				
				pd.maxUserTime = Math.max(pd.maxUserTime,pd.performanceResults[pd.performanceResults.length-1].userTime);
				pd.minUserTime = Math.min(pd.minUserTime,pd.performanceResults[pd.performanceResults.length-1].userTime);

			}
			
			var ar = [0,0,0,0];
			pd.downloadData = new Array();
			for(i = 0 ; i < 20; i ++)
			{
				ar[0] = Math.min(pd.endTime,pd.startTime + (i * ((pd.endTime-pd.startTime)/20)));
				ar[1] = Math.min(pd.endTime,ar[0] + (Math.random()*1000));
				ar[2] = Math.min(pd.endTime,ar[1] + (Math.random()*1000));
				ar[3] = Math.min(pd.endTime,ar[2] + (Math.random()*1000));
				
				pd.downloadData.push(new DownloadState(ar[1],ar[1]+10,ar[2],ar[3],ar[0],ar[3],"http://www.google.com" + Math.random()));
			}
			
			
			pd.renderData = new Array();
			var l = pd.startTime;
			var n = 0;
			for(i = 0 ; i < 40; i ++)
			{
				n = l + (Math.random()*500);
				
				if(n > pd.endTime)
					break;
					
				pd.renderData.push(new RenderedPeice(l,n,i));
				l = n;
			}
			
			
			
			this.dispatchEvent(new PerfDataEvent(PerfDataEvent.ON_NEW_DATA_RECEIVED, pd));*/
		}
	}
}