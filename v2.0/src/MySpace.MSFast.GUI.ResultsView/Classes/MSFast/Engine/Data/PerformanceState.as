package MSFast.Engine.Data
{
	public class PerformanceState
	{
		public var timeStamp:Number = -1;
		public var processorTime:Number = -1;
		public var userTime:Number = -1;
		public var workingSet:Number = -1;
		public var privateWorkingSet:Number = -1;
		
		public function PerformanceState(_timeStamp:Number,_processorTime:Number,_userTime:Number,_workingSet:Number,_privateWorkingSet:Number):void
		{
			this.timeStamp = _timeStamp;
			this.processorTime = _processorTime;
			this.userTime = _userTime;
			this.workingSet = _workingSet;
			this.privateWorkingSet = _privateWorkingSet;
		}
	}
}	