package MSFast.Engine.GUI
{
	public interface PopupsListener
	{
		function showPairPopup(txt:String,urlA:String,urlB:String):void;
		function hidePairPopup():void;

		function showThumbnailPopup(url:String):void;
		function hideThumbnailPopup():void;
		
		function showSourcePopup(s:Number,l:Number,p:Number):void;
	}
}
