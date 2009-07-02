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
using System.Text.RegularExpressions;
using System.IO;

namespace MySpace.MSFast.DataProcessors.Performance
{
	public class PerformanceDataProcessor : DataProcessor<PerformanceData>
	{
		private static Regex performance_Dump_Pattern = new Regex("\\?([0-9]*):PT([0-9\\.]*):UT([0-9\\.]*):WS([0-9]*):PWS([0-9\\.]*)", RegexOptions.Compiled);
		private static String filenameFormat = "perfdump_{0}.dat";

		#region DataProcessor<PerformanceData> Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
			FileInfo fi = new FileInfo(state.DumpFolder + "\\" + String.Format(filenameFormat, state.CollectionID));
			return (fi.Exists && fi.Length > 0);
		}

		public override PerformanceData GetProcessedData(ProcessedDataPackage state)
		{
			if (IsDataExists(state) == false)
				return null;

			String filename = state.DumpFolder + "\\" + String.Format(filenameFormat, state.CollectionID);
			StreamReader source = new StreamReader(filename);

			PerformanceData performanceData = new PerformanceData();

			performanceData.PerformanceDumpFilename = String.Format(filenameFormat, state.CollectionID);

			String buffer = source.ReadToEnd();
			source.Close();
			source.Dispose();

			MatchCollection matchs = performance_Dump_Pattern.Matches(buffer);
			Match match = null;

			for (int i = 0; i < matchs.Count; i++)
			{
				match = matchs[i];

				if (match.Success)
				{
					performanceData.AddLast(GetNewPerformanceState(
							state,
							performanceData,
							match.Groups[1].Value,
							match.Groups[2].Value,
							match.Groups[3].Value,
							match.Groups[4].Value,
							match.Groups[5].Value));
				}

			}
			FixAvg(performanceData);
			
			return performanceData;
		}

		#endregion

		#region Helpers
		private PerformanceState GetNewPerformanceState(ProcessedDataPackage state, PerformanceData processorData, string time, string pt, string ut, string ws, string pws)
		{
			PerformanceState ps = new PerformanceState();
			
			ps.TimeStamp = long.Parse(time);
			ps.ProcessorTime = double.Parse(pt);
			ps.UserTime = double.Parse(ut);
			ps.WorkingSet = int.Parse(ws);
			ps.PrivateWorkingSet = int.Parse(pws);

			if (ps.TimeStamp > 0)
				state.CollectionStartTime = Math.Min(state.CollectionStartTime, ps.TimeStamp);
			state.CollectionEndTime = Math.Max(state.CollectionEndTime, ps.TimeStamp);

			processorData.MaxPrivateWorkingSet = Math.Max(processorData.MaxPrivateWorkingSet, ps.PrivateWorkingSet);
			processorData.MaxProcessorTime = Math.Max(processorData.MaxProcessorTime, ps.ProcessorTime);
			processorData.MaxUserTime = Math.Max(processorData.MaxUserTime, ps.UserTime);
			processorData.MaxWorkingSet = Math.Max(processorData.MaxWorkingSet, ps.WorkingSet);
			
			processorData.MinPrivateWorkingSet = Math.Min(processorData.MinPrivateWorkingSet, ps.PrivateWorkingSet);
			processorData.MinProcessorTime = Math.Min(processorData.MinProcessorTime, ps.ProcessorTime);
			processorData.MinUserTime = Math.Min(processorData.MinUserTime, ps.UserTime);
			processorData.MinWorkingSet = Math.Min(processorData.MinWorkingSet, ps.WorkingSet);

			return ps;
		}

		private void FixAvg(PerformanceData processorData)
		{
			double AvgProcessorTime = -1;
			double AvgUserTime = -1;
			double AvgWorkingSet = -1;
			double AvgPrivateWorkingSet = -1;

			foreach (PerformanceState state in processorData)
			{
				AvgProcessorTime += state.ProcessorTime;
				AvgUserTime += state.UserTime;
				AvgWorkingSet += state.WorkingSet;
				AvgPrivateWorkingSet += state.PrivateWorkingSet;
			}

			AvgProcessorTime = (((double)AvgProcessorTime) / ((double)processorData.Count));
			AvgUserTime = (((double)AvgUserTime) / ((double)processorData.Count));
			AvgWorkingSet = (double)(((double)AvgWorkingSet) / ((double)processorData.Count));
			AvgPrivateWorkingSet = (double)(((double)AvgPrivateWorkingSet) / ((double)processorData.Count));

			processorData.AvgProcessorTime = AvgProcessorTime;
			processorData.AvgUserTime = AvgUserTime;
			processorData.AvgWorkingSet = (int)AvgWorkingSet;
			processorData.AvgPrivateWorkingSet = (int)AvgPrivateWorkingSet;
		}

		#endregion

	}
}
