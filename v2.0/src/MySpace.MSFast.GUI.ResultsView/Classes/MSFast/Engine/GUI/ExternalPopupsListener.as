package MSFast.Engine.GUI
{
		import flash.external.ExternalInterface;

	public class ExternalPopupsListener implements PopupsListener
	{
		
		
		public function showPairPopup(txt:String,urlA:String,urlB:String):void
		{
			ExternalInterface.call("showPairPopup",txt + "," + urlA + "," + urlB);
		}
		
		public function hidePairPopup():void
		{
			ExternalInterface.call("hidePairPopup","");
		}

		public function showThumbnailPopup(url:String):void
		{
			ExternalInterface.call("showThumbnailPopup",url);
		}
		
		public function hideThumbnailPopup():void
		{
			ExternalInterface.call("hideThumbnailPopup","");
		}
		
		public function showSourcePopup(s:Number,l:Number,p:Number):void
		{
			ExternalInterface.call("showSourcePopup",s+","+l+","+p);
		}
	}
}
