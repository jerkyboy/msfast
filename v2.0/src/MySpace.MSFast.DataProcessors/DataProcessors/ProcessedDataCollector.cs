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
using MySpace.MSFast.DataProcessors.Render;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Screenshots;
using MySpace.MSFast.DataProcessors.Performance;
using MySpace.MSFast.DataProcessors.PageSource;

namespace MySpace.MSFast.DataProcessors
{
	public static class ProcessedDataCollector
	{

		private static Dictionary<Type, DataProcessor> processors;

		static ProcessedDataCollector() 
		{
			processors = new Dictionary<Type, DataProcessor>();

			processors.Add(typeof(DownloadData), new DownloadDataProcessor());
			processors.Add(typeof(RenderData), new RenderDataProcessor());
			processors.Add(typeof(ScreenshotsData), new ScreenshotsDataProcessor());
			processors.Add(typeof(PerformanceData), new PerformanceDataProcessor());
			processors.Add(typeof(PageSourceData), new PageSourceDataProcessor());
			processors.Add(typeof(BrokenSourceData), new BrokenSourceDataProcessor());
		}

		public static ProcessedDataPackage CollectFor(Type type, String folder, int collectionId)
		{
            ProcessedDataPackage cmd = new ProcessedDataPackage(collectionId, folder);
			return CollectFor(type, cmd);
		}



        public static ProcessedDataPackage CollectAll(String folder, int collectionId)
		{
            ProcessedDataPackage cmd = new ProcessedDataPackage(collectionId, folder);

			foreach (Type t in processors.Keys)
			{
				cmd = CollectFor(t, cmd);
			}
						
			return cmd;
		}



		private static ProcessedDataPackage CollectFor(Type type, ProcessedDataPackage cmd)
		{
			if (processors.ContainsKey(type) == false)
				return cmd;

			DataProcessor dp =  processors[type];

			ProcessedData t = null;

			if (dp.IsDataExists(cmd))
			{
				t = dp.ProcessData(cmd);
				if (t != null)
				{
					if (cmd.ContainsKey(t.GetType()) == false)
					{
						cmd.Add(t.GetType(), t);
					}
				}
			}

			return cmd;
		}

	}
}
