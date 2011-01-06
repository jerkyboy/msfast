package MSFast.Engine.Data
{
	public class DownloadState
	{
		public var sendingRequestStartTime:Number = -1;
		public var sendingRequestEndTime:Number = -1;
		public var receivingResponseStartTime:Number = -1;
		public var receivingResponseEndTime:Number = -1;
		public var connectionStartTime:Number = -1;
		public var connectionEndTime:Number = -1;
		public var url:String = "";
		public var totalSent:Number = -1;
		public var totalReceived:Number = -1;
		
		public function DownloadState(_sendingRequestStartTime:Number,
										 _sendingRequestEndTime:Number,
										 _receivingResponseStartTime:Number,
										 _receivingResponseEndTime:Number,
										 _connectionStartTime:Number,
										 _connectionEndTime:Number,
										 uurl:String,
										 _totalSent:Number,
										 _totalReceived:Number):void
		{
			this.sendingRequestStartTime = _sendingRequestStartTime;
			this.sendingRequestEndTime = _sendingRequestEndTime;
			this.receivingResponseStartTime = _receivingResponseStartTime;
			this.receivingResponseEndTime = _receivingResponseEndTime;
			this.connectionStartTime = _connectionStartTime;
			this.connectionEndTime = _connectionEndTime;
			this.totalReceived = _totalReceived;
			this.totalSent = _totalSent;
			this.url = uurl;
		}
	}
}	
			