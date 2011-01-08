package MSFast.Engine.Data
{
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.PerformanceState;
	import MSFast.Engine.Data.DownloadState;
	import MSFast.Engine.Data.RenderedPeice;
	import MSFast.Engine.Data.MarkerPeice;
	import flash.net.URLLoader;
	import flash.net.URLRequest;
	import flash.events.EventDispatcher;
	import flash.events.Event;
	import flash.xml.XMLDocument;
	import flash.xml.XMLNode;
	
	public class PerfDataProvider extends EventDispatcher
	{
		private static var _instance:PerfDataProvider = new PerfDataProvider();
		
		public static function getInstance():PerfDataProvider
		{
			return _instance;
		}
		
		public function loadData(url:String):void
		{
			var xmlLoader:URLLoader = new URLLoader(); 
			xmlLoader.addEventListener(Event.COMPLETE, LoadXML); 
			xmlLoader.load(new URLRequest(url)); 
		}
		
		private function LoadXML(e:Event):void { 
			var xDoc:XMLDocument = new XMLDocument();
			xDoc.ignoreWhite = true;
		    xDoc.parseXML(XML(e.target["data"]).toXMLString());
			parseResults(xDoc);
		}

		private function parseResults(res:XMLDocument):void
		{
			var pd:PerfData = new PerfData();
			
			for(var i = 0 ; i < res.firstChild.childNodes.length ; i ++)
			{
				if(res.firstChild.childNodes[i].nodeName == "testid"){
					pd.testId = parseInt(res.firstChild.childNodes[i].firstChild.nodeValue);
				}else if(res.firstChild.childNodes[i].nodeName == "starttime"){
					pd.startTime = parseInt(res.firstChild.childNodes[i].firstChild.nodeValue);
				}else if(res.firstChild.childNodes[i].nodeName == "endtime"){
					pd.endTime = parseInt(res.firstChild.childNodes[i].firstChild.nodeValue);
				}else if(res.firstChild.childNodes[i].nodeName == "thumbnailroot"){
					pd.thumbnail_path = res.firstChild.childNodes[i].firstChild.nodeValue;
				}else if(res.firstChild.childNodes[i].nodeName == "render"){
					parseRender(pd,res.firstChild.childNodes[i]);
				}else if(res.firstChild.childNodes[i].nodeName =="markers"){
					parseMarkers(pd,res.firstChild.childNodes[i]);
				}else if(res.firstChild.childNodes[i].nodeName == "download"){
					parseDownload(pd,res.firstChild.childNodes[i]);
				}else if(res.firstChild.childNodes[i].nodeName == "performance"){
					parsePerformance(pd,res.firstChild.childNodes[i]);
				}
			}
			
			this.dispatchEvent(new PerfDataEvent(PerfDataEvent.ON_NEW_DATA_RECEIVED, pd));
		}
		
		private function parseMarkers(pd:PerfData, xml:XMLNode)
		{ 
			pd.markersData = new Array();
			
			for(var i = 0 ; i < xml.childNodes.length ; i ++)
			{
				pd.markersData.push(new MarkerPeice(parseInt(xml.childNodes[i].attributes["t"]),xml.childNodes[i].attributes["n"]));
			}
		}
		
		private function parseRender(pd:PerfData, xml:XMLNode)
		{ 
			if(xml.attributes["un"] != undefined && xml.attributes["un"] != "" && xml.attributes["un"] != "-1"){ 
				pd.timeReadyStateUninitialized = Math.max(0,parseInt(xml.attributes["un"]));
			}
			if(xml.attributes["li"] != undefined && xml.attributes["li"] != "" && xml.attributes["li"] != "-1"){ 
				pd.timeReadyStateLoading = Math.max(0,parseInt(xml.attributes["li"]));
			}
			if(xml.attributes["lo"] != undefined && xml.attributes["lo"] != "" && xml.attributes["lo"] != "-1"){ 
				pd.timeReadyStateLoaded = Math.max(0,parseInt(xml.attributes["lo"]));
			}
			if(xml.attributes["in"] != undefined && xml.attributes["in"] != "" && xml.attributes["in"] != "-1"){ 
				pd.timeReadyStateInteractive = Math.max(0,parseInt(xml.attributes["in"]));
			}
			if(xml.attributes["co"] != undefined && xml.attributes["co"] != "" && xml.attributes["co"] != "-1"){ 
				pd.timeReadyStateComplete = Math.max(0,parseInt(xml.attributes["co"]));
			}
			if(xml.attributes["ol"] != undefined && xml.attributes["ol"] != "" && xml.attributes["ol"] != "-1"){ 
				pd.timeOnLoad = Math.max(0,parseInt(xml.attributes["ol"]));
			}
			
			pd.renderData = new Array();
			
			for(var i = 0 ; i < xml.childNodes.length ; i ++)
			{
				pd.renderData.push(new RenderedPeice(parseInt(xml.childNodes[i].attributes["s"]),
													 parseInt(xml.childNodes[i].attributes["e"]),
													 parseInt(xml.childNodes[i].attributes["i"]),
													 parseInt(xml.childNodes[i].attributes["ss"]),
													 parseInt(xml.childNodes[i].attributes["sl"]),
													 xml.childNodes[i].attributes["st"]));
			}
		}
		
		private function parseDownload(pd:PerfData, xml:XMLNode)
		{
			pd.downloadData = new Array();
			
			for(var i = 0 ; i < xml.childNodes.length ; i ++)
			{
				pd.downloadData.push(new DownloadState(
														parseInt(xml.childNodes[i].attributes["srst"]),
														parseInt(xml.childNodes[i].attributes["sret"]),
														parseInt(xml.childNodes[i].attributes["rrst"]),
														parseInt(xml.childNodes[i].attributes["rret"]),
														parseInt(xml.childNodes[i].attributes["cnst"]),
														parseInt(xml.childNodes[i].attributes["cnet"]),
														xml.childNodes[i].firstChild.nodeValue,
														parseInt(xml.childNodes[i].attributes["ttsn"]),
														parseInt(xml.childNodes[i].attributes["ttrc"])));
			}
		}
		
		private function parsePerformance(pd:PerfData, xml:XMLNode)
		{
			pd.performanceResults = new Array();
			
			pd.maxPrivateWorkingSet = parseInt(xml.attributes["xpws"]);
			pd.minPrivateWorkingSet = parseInt(xml.attributes["npws"]);
			
			pd.maxWorkingSet = parseInt(xml.attributes["xws"]);
			pd.minWorkingSet = parseInt(xml.attributes["nws"]);
			
			pd.maxProcessorTime = parseInt(xml.attributes["xpt"]);
			pd.minProcessorTime = parseInt(xml.attributes["npt"]);
			
			pd.maxUserTime = parseInt(xml.attributes["xut"]);
			pd.minUserTime = parseInt(xml.attributes["nut"]);
			
			pd.maxPrivateWorkingSet = Math.max(pd.maxPrivateWorkingSet,pd.maxWorkingSet);
			pd.maxWorkingSet = Math.max(pd.maxPrivateWorkingSet,pd.maxWorkingSet);
			
			pd.minPrivateWorkingSet = Math.min(pd.minPrivateWorkingSet,pd.minWorkingSet);
			pd.minWorkingSet = Math.min(pd.minPrivateWorkingSet,pd.minWorkingSet);
			
			for(var i = 0 ; i < xml.childNodes.length ; i ++)
			{
				pd.performanceResults.push(new PerformanceState(parseInt(xml.childNodes[i].attributes["ts"]),
													 	parseInt(xml.childNodes[i].attributes["pt"]),
													 	parseInt(xml.childNodes[i].attributes["ut"]),
													 	parseInt(xml.childNodes[i].attributes["ws"]),
													 	parseInt(xml.childNodes[i].attributes["pws"])));
			}
		}
	}
}


