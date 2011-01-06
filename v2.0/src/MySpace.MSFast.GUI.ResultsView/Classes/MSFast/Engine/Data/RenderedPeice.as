package MSFast.Engine.Data
{
	public class RenderedPeice
	{
		public var id:Number = -1;
		public var startTime:Number = -1;
		public var endTime:Number = -1;
		public var sourceIndex:Number = -1;
		public var sourceLength:Number = -1;
		public var sourceType:String = "";
		
		public function RenderedPeice(_startTime:Number,_endTime:Number,_id:Number,_sourceIndex:Number,_sourceLength:Number,_sourceType:String):void
		{
			this.sourceIndex = _sourceIndex;
			this.sourceLength = _sourceLength;
			this.startTime = _startTime;
			this.id = _id;
			this.endTime = _endTime;
			this.sourceType = _sourceType;
		}
	}
}		