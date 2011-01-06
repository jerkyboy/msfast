package MSFast.Engine.GUI
{
	import flash.display.*;
	
	public class ThumbnailPreview extends Sprite
	{
		
		public function setThumbnail(url:String):void
		{
			thumb.loadThumbnail(url);
		}
	}
}
