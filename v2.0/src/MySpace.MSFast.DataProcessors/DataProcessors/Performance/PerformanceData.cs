//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;

namespace MySpace.MSFast.DataProcessors.Performance
{
	public class PerformanceState
	{
		public long TimeStamp = -1;
		public double ProcessorTime = -1;
		public double UserTime = -1;
		public int WorkingSet = -1;
		public int PrivateWorkingSet = -1;
	}

	public class PerformanceData : LinkedList<PerformanceState>, ProcessedData
	{
		public String PerformanceDumpFilename = null;

		public double MaxProcessorTime = double.MinValue;
		public double MinProcessorTime = double.MaxValue;
		public double AvgProcessorTime = 0;
		public double MaxUserTime = double.MinValue;
		public double MinUserTime = double.MaxValue;
		public double AvgUserTime = 0;
		public int MaxWorkingSet = int.MinValue;
		public int MinWorkingSet = int.MaxValue;
		public int AvgWorkingSet = 0;
		public int MaxPrivateWorkingSet = int.MinValue;
		public int MinPrivateWorkingSet = int.MaxValue;
		public int AvgPrivateWorkingSet = 0;
	}
}
