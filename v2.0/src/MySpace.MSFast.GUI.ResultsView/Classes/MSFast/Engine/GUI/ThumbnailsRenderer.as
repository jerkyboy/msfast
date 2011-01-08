package MSFast.Engine.GUI
{
	import flash.display.MovieClip;
	import MSFast.Engine.Events.PerfDataEvent;
	import MSFast.Engine.Data.PerfData;
	import MSFast.Engine.Data.RenderedPeice;
	import MSFast.Engine.GUI.ThumbnailsPair;
	import flash.geom.*;
	import flash.events.Event;
	import MSFast.Engine.GUI.ThumbnailPreview;
	import MSFast.Engine.GUI.Popup;
	import flash.events.MouseEvent;
	import flash.external.ExternalInterface;
	import MSFast.Engine.Events.UIEvent;
	
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	
	public class ThumbnailsRenderer extends AbsDataGraphDisplay
	{
		private static var HEAD_TAG_TEXTFORMAT:TextFormat;
		private static var BODY_TAG_TEXTFORMAT:TextFormat;
		private static var EVENTS_TEXTFORMAT:TextFormat;
		private static var AFTER_BODY_TAG_TEXTFORMAT:TextFormat;
		
		private var graph_mc:MovieClip = null;
		private var mask_mc:MovieClip = null;
		
		private var mouseHoverSegment_mask_mc:MovieClip = null;
		private var mouseHoverSegment:MovieClip = null;

		private var thumbnails:Array = null;
		
		private var isPairPopupVisible:Boolean = false;
		private var isThumbPopupVisible:Boolean = false;
		
		private var segments:Array;
		private var htmlHeadTagText:TextField;
		private var htmlBodyTagText:TextField;
		private var htmlOutOfBodyTagText:TextField;
		
		
		private var eventsDisplay:Array;
		
		public function ThumbnailsRenderer()
		{
			super();

			HEAD_TAG_TEXTFORMAT = new TextFormat(); 
			HEAD_TAG_TEXTFORMAT.size = 10;
			HEAD_TAG_TEXTFORMAT.color = 0x20d423;
			HEAD_TAG_TEXTFORMAT.font = "Tahoma";
			HEAD_TAG_TEXTFORMAT.align = "center";

			EVENTS_TEXTFORMAT = new TextFormat(); 
			EVENTS_TEXTFORMAT.size = 10;
			EVENTS_TEXTFORMAT.color = 0x0d8aa8;
			EVENTS_TEXTFORMAT.font = "Tahoma";
			EVENTS_TEXTFORMAT.align = "center";

			BODY_TAG_TEXTFORMAT = new TextFormat(); 
			BODY_TAG_TEXTFORMAT.size = 10;
			BODY_TAG_TEXTFORMAT.color = 0xffa800;
			BODY_TAG_TEXTFORMAT.font = "Tahoma";
			BODY_TAG_TEXTFORMAT.align = "center";
						
			AFTER_BODY_TAG_TEXTFORMAT = new TextFormat(); 
			AFTER_BODY_TAG_TEXTFORMAT.size = 10;
			AFTER_BODY_TAG_TEXTFORMAT.color = 0xfba291;
			AFTER_BODY_TAG_TEXTFORMAT.font = "Tahoma";
			AFTER_BODY_TAG_TEXTFORMAT.align = "center";

			eventsDisplay = new Array();
			eventsDisplay.push({fi:"timeReadyStateUninitialized",mc:createTextField("ReadyState: Uninitialized", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			eventsDisplay.push({fi:"timeReadyStateLoading",mc:createTextField("ReadyState: Loading", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			eventsDisplay.push({fi:"timeReadyStateLoaded",mc:createTextField("ReadyState: Loaded", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			eventsDisplay.push({fi:"timeReadyStateInteractive",mc:createTextField("ReadyState: Interactive", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			eventsDisplay.push({fi:"timeReadyStateComplete",mc:createTextField("ReadyState: Complete", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			eventsDisplay.push({fi:"timeOnLoad",mc:createTextField("OnLoad", EVENTS_TEXTFORMAT,0xe7f6ff, 0x54b2c9)});
			
			this.mask_mc = new MovieClip();
			this.graph_mc = new MovieClip();
			this.mouseHoverSegment = new MovieClip();
			this.mouseHoverSegment_mask_mc = new MovieClip();
			
			this.htmlHeadTagText = createTextField("<head>...</head>",HEAD_TAG_TEXTFORMAT,0xf3fff3, 0xbcffbd);
			this.htmlBodyTagText = createTextField("<body>...</body>",BODY_TAG_TEXTFORMAT,0xffffc0, 0xffcc00);
			this.htmlOutOfBodyTagText = createTextField("</body>...</html>", AFTER_BODY_TAG_TEXTFORMAT,0xfff5f3, 0xffc7bc);
			
			this.bounds_mc.visible = false;
			
			addChild(this.mask_mc);
			addChild(this.graph_mc);
			addChild(this.mouseHoverSegment);
			addChild(this.mouseHoverSegment_mask_mc);

			this.graph_mc.addChild(this.htmlHeadTagText);
			this.graph_mc.addChild(this.htmlBodyTagText);
			this.graph_mc.addChild(this.htmlOutOfBodyTagText);
			
			for(var i = 0 ; i < eventsDisplay.length ; i++)
			{
				this.graph_mc.addChild(eventsDisplay[i].mc);
				eventsDisplay[i].mc.y = i * eventsDisplay[i].mc.height;
				eventsDisplay[i].mc.width = 120;
			}
			
			drawRect(this.mouseHoverSegment);
			drawRect(this.mouseHoverSegment_mask_mc);
			drawRect(this.mask_mc);
			drawRect(this.graph_mc);
			
			this.graph_mc.mask = (this.mask_mc);
			this.mouseHoverSegment.mask = (this.mouseHoverSegment_mask_mc);
			
			this.addEventListener(Event.RESIZE,_paneResized);
			this.addEventListener("GraphSizeChanged",_graphResized);
			this.addEventListener("GraphMoved",_graphMoved);
		}
		
		private function createTextField(st:String,textFormat:TextFormat,backColor:Number,borderColor:Number):TextField{
			var t = new TextField();
			t.text = st;
			t.selectable = false;
			t.setTextFormat(textFormat);
			t.background = true;
			t.height = 16;
			t.backgroundColor = backColor;
			t.border = true;
			t.borderColor = borderColor;
			return t;
		}
		
		public override function init():void
		{
			this.thumbnails = createThumbnails(7);
			stage.addEventListener(MouseEvent.MOUSE_MOVE,_mouseMove);
			stage.addEventListener(MouseEvent.MOUSE_DOWN,_mouseDown);
			stage.addEventListener(MouseEvent.MOUSE_UP,_mouseUp);
		}
		
		
		
		
		
		
		
		
		
		private var whenDown:Date;
		
		private function _mouseDown(e:MouseEvent):void
		{
			whenDown = new Date();
			whenDown.milliseconds += 100;
		}
		
		
		private function _mouseUp(e:MouseEvent):void
		{
			if(whenDown < new Date())
			{
				return;
			}
			var rs = getSegment(e);
			if(rs!=null){
				this.dispatchEvent(new UIEvent("ShowSourcePopup",{s:rs.sourceIndex,l:rs.sourceLength,p:rs.id},true,true));
			}
		}
		
		private function getSegment(e:MouseEvent):RenderedPeice
		{
			var p = this.globalToLocal(new Point(e.stageX,e.stageY));

			if(p.x < 0) return null;

			var rx = (p.x-this.graph_mc.x);
			
			var s = this.segments[0];
			for(var i = 0 ; i < this.segments.length ; i++)
			{
				if(this.segments[i].x1 < rx && this.segments[i].x2 > rx)
				{
					s = this.segments[i];
					break;
				}
				if(i == this.segments.length-1 && this.segments[i].x2 < rx)
					s = null;
			}
			return s.rp;
		}		
			
		
		private function _mouseMove(e:MouseEvent):void
		{

			var p = this.globalToLocal(new Point(e.stageX,e.stageY));
			if(p.x < 0) return;

			var mouseRect = new Rectangle(e.stageX, e.stageY, 1, 1);
			
			var i = 0;
			
			if(this.thumbnails == null || this.thumbnails.length == 0 || this.segments == null || this.segments.length == 0)
				return;
			
			for(i = this.thumbnails.length-1 ; i >= 0; i--)
			{
				this.thumbnails[i].setTint(0xFFFFFF,0.7);
			}

			var rx = (p.x-this.graph_mc.x);
			var s = this.segments[0];
			
			for(i = 0 ; i < this.segments.length ; i++)
			{
				if(this.segments[i].x1 < rx && this.segments[i].x2 > rx)
				{
					s = this.segments[i];
					break;
				}
			}
			
			this.mouseHoverSegment.graphics.clear();
			if(s != null)
			{
				this.mouseHoverSegment.graphics.moveTo(s.x1, 0);
				this.mouseHoverSegment.graphics.beginFill(0xffFF00,0.25);
				this.mouseHoverSegment.graphics.lineStyle(0,0xffcc00);
				this.mouseHoverSegment.graphics.lineTo(s.x1,(this.containerBounds.height - ThumbnailsPair.DEFAULT_HEIGHT - 5)+2);
				this.mouseHoverSegment.graphics.lineTo(s.x2,(this.containerBounds.height - ThumbnailsPair.DEFAULT_HEIGHT - 5)+2);
				this.mouseHoverSegment.graphics.lineTo(s.x2,0);
				this.mouseHoverSegment.graphics.lineTo(s.x1,0);
			}
			
			var hover_thumbnailsPair:ThumbnailsPair = null;
			for(i = this.thumbnails.length-1 ; i >= 0; i--)
			{
				if(this.thumbnails[i].visible && this.thumbnails[i].getBounds(stage).intersects(mouseRect)){
					hover_thumbnailsPair = this.thumbnails[i];
					break;
				}
			}
			if(hover_thumbnailsPair != null)
			{
				hover_thumbnailsPair.setTint(0xFFFFFF,0);
				this.graph_mc.setChildIndex(hover_thumbnailsPair,this.graph_mc.numChildren-1);
					showPairPopup(hover_thumbnailsPair.getText(), hover_thumbnailsPair.getStartThumbnail(), hover_thumbnailsPair.getEndThumbnail());
			}
			else {
				hidePairPopup();
			}
		}
		
		
		
		
		private function showPairPopup(txt:String,urla:String,urlb:String):void
		{
			isPairPopupVisible = true;
			this.dispatchEvent(new UIEvent("ShowPairPopup",{txt:txt,urla:urla,urlb:urlb},true,true));
		}
		
		private function hidePairPopup():void
		{
			if(isPairPopupVisible == false) return;
			isPairPopupVisible = false;
			this.dispatchEvent(new UIEvent("HidePairPopup",{},true,true));
		}
		private function showThumbPopup(url:String):void
		{
			isThumbPopupVisible = true;
			this.dispatchEvent(new UIEvent("ShowThumbPopup",{url:url},true,true));
		}
		
		private function hideThumbPopup():void
		{
			if(isThumbPopupVisible == false) return;
			isThumbPopupVisible = false;
			this.dispatchEvent(new UIEvent("HideThumbPopup",{},true,true));
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		

		//Resize
		private function _paneResized(e:Event):void
		{
			this.mask_mc.width = this.containerBounds.width;
			this.mask_mc.height = this.containerBounds.height;
			this.mouseHoverSegment_mask_mc.width = this.containerBounds.width;
			this.mouseHoverSegment_mask_mc.height = this.containerBounds.height;
			onNewPerfData(this.perfData);
		}		
		
		private function _graphResized(e:Event):void{onNewPerfData(this.perfData);}		
		private function _graphMoved(e:Event):void{
			this.mouseHoverSegment.x = this.graphBounds.x;
			this.graph_mc.x = this.graphBounds.x;
		}				
		
		public override function onNewPerfData(perfData:PerfData):void
		{
			clearDisplay(this.perfData != perfData);
			
			if(perfData != null && perfData.renderData != null && perfData.renderData.length > 0)
			{
				this.mask_mc.visible = true;
				this.graph_mc.visible = true;
				this.mouseHoverSegment.visible = true;
				this.mouseHoverSegment_mask_mc.visible = true;
				drawDisplay(perfData);
			}
			else
			{
				this.mask_mc.visible = false;
				this.graph_mc.visible = false;
				this.mouseHoverSegment.visible = false;
				this.mouseHoverSegment_mask_mc.visible = false;
			}
		}

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		private function drawDisplay(perfData:PerfData):void
		{	
			if(perfData == null || perfData.renderData == null || perfData.renderData.length == 0)
			{
				return;
			}
			
			var rp:RenderedPeice = null;
			var renderData = perfData.renderData;
			
			var x1 = 0;
			var x2 = 0;
			var h = this.containerBounds.height - ThumbnailsPair.DEFAULT_HEIGHT - 5;
			var min = getMinimumRenderTime(renderData, this.thumbnails.length);
			var tcount = 0;
			var url = "";
			var id = "";
			segments = new Array();
			
			var endOfHead:Number = 0;
			var endOfBody:Number = 0;

			for(var d = 0 ; d < renderData.length; d++)
			{
				rp = ((RenderedPeice)(renderData[d]));
				
				if(rp.startTime != 0 && rp.endTime != 0){
					
					id = "" + rp.id;
					while(id.length < 5)
						id = "0" + id;
					
					if(d == 0)
						url = perfData.thumbnail_path + "TC_" + perfData.testId + "_" + id + ".jpg";
					
					x1 =  ((rp.startTime - perfData.startTime) * ratio);
					x2 =  ((rp.endTime - perfData.startTime) * ratio);
					
					segments.push({x1:x1,x2:x2,rp:rp});
					
						this.graph_mc.graphics.lineStyle(0,0xffCC00,0.25);
						this.graph_mc.graphics.moveTo(x1,0);
						this.graph_mc.graphics.lineTo(x1,h);
					
						this.graph_mc.graphics.moveTo(x1,h);
						this.graph_mc.graphics.beginFill(0xFF7700);
						this.graph_mc.graphics.drawCircle(x1, h, 2);
						this.graph_mc.graphics.endFill();
					

					if(rp.sourceType == "Head")
					{
						
						this.htmlHeadTagText.width = x2-x1;
						this.htmlHeadTagText.x = x1;//(x1 + ((x2-x1)/2)) - (this.htmlHeadTagText.width/2);
						endOfHead = this.htmlHeadTagText.x + this.htmlHeadTagText.width;
						
					}else if(rp.sourceType == "BodyContent")
					{
						endOfBody = x2;
					}
					
					if((rp.endTime-rp.startTime) > min && tcount < this.thumbnails.length)
					{
						this.graph_mc.graphics.lineStyle(0,0xffCC00);
						this.graph_mc.graphics.moveTo(x1,h+2);
						this.graph_mc.graphics.lineTo(x2,h+2);
						this.graph_mc.graphics.moveTo(x1 + ((x2-x1)/2),h+2);
						this.graph_mc.graphics.lineTo(x1 + ((x2-x1)/2),h+8);
						
						this.thumbnails[tcount].x = x1 + ((x2-x1)/2);
						this.thumbnails[tcount].y = h+3;
						this.thumbnails[tcount].setThumbnails(url, perfData.thumbnail_path + "TC_" + perfData.testId + "_" + id + ".jpg");
						this.thumbnails[tcount].segment = rp;
						this.thumbnails[tcount].setText( (Math.round((((rp.endTime-rp.startTime)/(perfData.endTime - perfData.startTime) )*10000))/100) + "%");
						tcount ++;
					}
					
					url = perfData.thumbnail_path + "TC_" + perfData.testId + "_" + id + ".jpg";
				}
			}
			
			this.graph_mc.graphics.moveTo(x2,0);
			this.graph_mc.graphics.lineTo(x2,h);
			
			this.htmlBodyTagText.x = endOfHead;
			this.htmlBodyTagText.width = endOfBody-endOfHead;
			
			this.htmlOutOfBodyTagText.x = this.htmlBodyTagText.x + this.htmlBodyTagText.width;
			this.htmlOutOfBodyTagText.width = x2-(this.htmlBodyTagText.x + this.htmlBodyTagText.width);

			this.htmlOutOfBodyTagText.y = this.containerBounds.height-this.htmlHeadTagText.height-5;
			this.htmlHeadTagText.y = this.containerBounds.height-this.htmlHeadTagText.height-5;
			this.htmlBodyTagText.y = this.containerBounds.height-this.htmlHeadTagText.height-5;

			for(var i = 0 ; i < eventsDisplay.length ; i++)
			{
				trace(perfData[eventsDisplay[i].fi]);
				if(perfData[eventsDisplay[i].fi] > 0)
				{
					eventsDisplay[i].mc.visible = true;
					eventsDisplay[i].mc.x = (perfData[eventsDisplay[i].fi] - perfData.startTime) * ratio;
					drawEvent(this.containerBounds.height, eventsDisplay[i].mc.x);
				}else{
					eventsDisplay[i].mc.visible = false;
				}
			}

			fixThumbnailsScaleAndPosition();
		}
		
		private function drawEvent(h:Number,xx:Number):void
		{
				this.graph_mc.graphics.lineStyle(0, 0x2594af , 0.25);
				this.graph_mc.graphics.moveTo(xx,0);
				this.graph_mc.graphics.lineTo(xx,h);
		}
			
		public override function get ratio():Number{
			if(perfData == null) return 0;
			return (this.graphBounds.width / (perfData.endTime - perfData.startTime));
		}		
		
		private function createThumbnails(count:Number)
		{
			var ar = new Array();
			for(var i = 0 ; i < count; i++)
			{
				ar.push(new ThumbnailsPair());
				this.graph_mc.addChild(ar[i]);
				ar[i].setTint(0xFFFFFF,0.7);

			}
			return ar;
		}		
		
		private function fixThumbnailsScaleAndPosition():void
		{
			for(var i = 0 ; i < thumbnails.length; i++)
			{
				this.thumbnails[i].scaleX = 0.17;
				this.thumbnails[i].scaleY = 0.17;
			}
		}		
		
		private function clearDisplay(clearThumbnails:Boolean):void
		{
			this.mouseHoverSegment.graphics.clear();
			
			if(clearThumbnails){
				for(var i = 0 ; i < this.thumbnails.length; i++)
				{
					this.thumbnails[i].setThumbnails(null,null);
				}
			}
			this.graph_mc.graphics.clear();
		}
		
		private function getMinimumRenderTime(renderData:Array, top:Number):Number
		{
			var avgc:Number = 0;
			
			var res = new Array();

			for(var d:Number = 0 ; d < renderData.length; d++)
			{
				if (renderData[d].startTime != 0 && renderData[d].endTime != 0)
				{
					res.push(renderData[d].endTime-renderData[d].startTime);
				}
			}

			res = res.sort(Array.DESCENDING | Array.NUMERIC);
			
			if (top < res.length)
			{
				return res[top];
			}
			
			return res[0];
		}				
		
	}	
}