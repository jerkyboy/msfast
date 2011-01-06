using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.DataProcessors.Markers
{
	public class Marker
	{
		public String Name = String.Empty;
		public long Timestamp = 0;

        public Marker(String Name, long Timestamp)
		{
			this.Name = Name;
            this.Timestamp = Timestamp;            
		}
	}


	public class MarkersData : LinkedList<Marker>, ProcessedData
	{
		public String MarkersDumpFilename = null;
		
		public long MaxMarkerTime = long.MinValue;
		public long MinMarkerTime = long.MaxValue;
	}
}
