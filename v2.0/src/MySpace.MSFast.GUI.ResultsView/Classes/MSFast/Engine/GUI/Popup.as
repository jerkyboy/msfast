package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import flash.geom.Point;
	import flash.utils.*;
	
	public class Popup extends MovieClip
	{
		private var target_x:Number = 0;
		private var target_y:Number = 0;
		private var target_alpha:Number = 1;
		private var target_visible:Boolean = true;
		
		private var threadId:Number = 0;
		
		public function Popup()
		{
			
		}
		
		public override function set x(__x:Number):void
		{	
			if(__x + this.width > stage.stageWidth){__x = stage.stageWidth - this.width;}
			else if(__x < 0 ){__x = 0;}

			target_x = __x;
			checkTarget();
		}
		public override function get x():Number{return target_x;}
		
		
		public override function set y(__y:Number):void
		{
			if(__y + this.height > stage.stageHeight){__y = stage.stageHeight - this.height;}
			else if(__y < 0 ){__y = 0;}
			
			target_y = __y;
			checkTarget();
		}
		public override function get y():Number{return target_y;}
		
		public override function set visible(__visible:Boolean):void
		{
			if(__visible){
				target_alpha = 1;
			}else{
				target_alpha = 0;
			}
			target_visible = __visible;
			checkTarget();
		}
		public override function get visible():Boolean{return target_visible;}
		
		
		private function checkTarget():void
		{
			super.x = target_x;
			super.y = target_y;
			super.visible = target_visible;
			//clearInterval(threadId);
			//threadId = setInterval(function(){moveToTarget();},25);
			
			//moveToTarget();
		}
		
		private function moveToTarget():void
		{
 		    super.x = super.x + ((target_x-super.x)/5);
			super.y = super.y + ((target_y-super.y)/5);
			super.alpha = super.alpha + ((target_alpha-super.alpha)/15);
		}
		
	}
}
