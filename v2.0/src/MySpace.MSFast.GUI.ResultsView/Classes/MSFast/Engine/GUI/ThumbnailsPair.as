package MSFast.Engine.GUI
{
	import flash.geom.*;
	import flash.display.*;
	import flash.events.*;
	import flash.net.URLRequest;
	import flash.events.MouseEvent;
	import flash.text.TextField;
	import fl.motion.Color;
	import MSFast.Engine.Data.*;
	
	public class ThumbnailsPair extends MovieClip
	{
		public static var DEFAULT_WIDTH:Number = 716;
		public static var MINIMUM_WIDTH:Number = 120;
		public static var DEFAULT_HEIGHT:Number = 70;
		
		public var segment:RenderedPeice = null;
		public var render:Rectangle = null;
		
		private var startUrl:String = "";
		private var endUrl:String = "";
		
		public function ThumbnailsPair()
		{
			this.visible = false;
			this.txtLabel.visible = false;
		}

		public function setThumbnails(startUrl:String,endUrl:String):void
		{
			
			this.visible = (startUrl != null && endUrl != null);
			
			if(this.visible == false)
			{
				return;
			}
			
			this.startUrl = startUrl;
			this.endUrl = endUrl;
			
			this.thumbA.loadThumbnail(startUrl);
			this.thumbB.loadThumbnail(endUrl);
		}
		
		public function getStartThumbnail():String{return this.startUrl;}
		public function getEndThumbnail():String{return this.endUrl;}
		
		public function setText(s:String):void{this.txtLabel.text = s;}
		public function getText():String{return this.txtLabel.text;}
		
		public function showText():void{this.txtLabel.visible = true;}
		
		public function setTint(colorNum:Number, alphaSet:Number):void {
			var cTint:Color = new Color();
			cTint.setTint(colorNum, alphaSet);
			this.transform.colorTransform = cTint;
		}
	}
}
