
package MSFast.Engine.GUI
{
	import flash.geom.Rectangle;
	
	public interface Scrollable
	{
		function scroll(_xp:Number,_yp:Number):void;
		function scrollTo(_x:Number,_y:Number):void;
		function getPaneBounds():Rectangle;
		function getContentBounds():Rectangle;
	}
}
