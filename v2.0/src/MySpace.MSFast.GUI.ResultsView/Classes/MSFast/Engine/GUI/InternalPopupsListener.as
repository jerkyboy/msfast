package MSFast.Engine.GUI
{
	import MSFast.Engine.GUI.Popup;
	import flash.display.MovieClip;
	import flash.external.ExternalInterface;
	import flash.events.MouseEvent;
	
	public class InternalPopupsListener implements PopupsListener
	{
		private var thumbnailPreview_popup:Popup = null;
		private var thumbnailPreview_mc:ThumbnailPreview = null;
		
		private var thumbnailPairPreview_popup:Popup = null;
		private var thumbnailPairPreview_mc:ThumbnailsPair = null;
		
		private var mouseEvent:MouseEvent;
		
		public function InternalPopupsListener(mc:MovieClip)
		{
			this.thumbnailPairPreview_popup = new Popup();
			this.thumbnailPairPreview_mc = new ThumbnailsPair();
			this.thumbnailPairPreview_mc.x = this.thumbnailPairPreview_mc.width/2;
			this.thumbnailPairPreview_popup.addChild(thumbnailPairPreview_mc);
			this.thumbnailPairPreview_popup.visible = false;
			mc.addChild(this.thumbnailPairPreview_popup);
			
			this.thumbnailPreview_mc = new ThumbnailPreview();
			this.thumbnailPreview_popup = new Popup();
			this.thumbnailPreview_popup.addChild(this.thumbnailPreview_mc);
			this.thumbnailPreview_popup.visible = false;
			mc.addChild(this.thumbnailPreview_popup);
			
			mc.addEventListener(MouseEvent.MOUSE_MOVE, onMousemove);
			
		}
		
		private function onMousemove(e:MouseEvent):void
		{
			mouseEvent = e;
		}
		
		public function showPairPopup(txt:String,urlA:String,urlB:String):void
		{
			this.thumbnailPairPreview_popup.visible = true;
			
			this.thumbnailPairPreview_mc.setText(txt);
			this.thumbnailPairPreview_mc.showText();
			this.thumbnailPairPreview_mc.setThumbnails(urlA, urlB);

			this.thumbnailPairPreview_popup.x = mouseEvent.stageX-(thumbnailPairPreview_mc.width/2);
			this.thumbnailPairPreview_popup.y = mouseEvent.stageY - 100 - this.thumbnailPairPreview_mc.height;
		}
		
		public function hidePairPopup():void
		{
			this.thumbnailPairPreview_popup.visible = false;
		}

		
		
		public function showThumbnailPopup(url:String):void
		{
			this.thumbnailPreview_popup.visible = true;
			this.thumbnailPreview_popup.x = mouseEvent.stageX- (this.thumbnailPreview_mc.width/2);
			this.thumbnailPreview_popup.y = mouseEvent.stageY - (this.thumbnailPreview_mc.height + 15);
			this.thumbnailPreview_mc.setThumbnail(url);
		}
		
		public function hideThumbnailPopup():void
		{
			this.thumbnailPreview_popup.visible = false;
		}
		
		public function showSourcePopup(s:Number,l:Number, p:Number):void
		{
			ExternalInterface.call("function() { showSourceForRender(" + s + "," + l + "); }");
		}
	}
}
