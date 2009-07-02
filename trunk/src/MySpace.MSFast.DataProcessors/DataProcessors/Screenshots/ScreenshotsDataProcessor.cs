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
using System.IO;

namespace MySpace.MSFast.DataProcessors.Screenshots
{
	public class ScreenshotsDataProcessor : DataProcessor<ScreenshotsData>
	{
		private static String screenShotsFilePattern = "TC_{0}_*.jpg";
	
		#region DataProcessor<ScreenshotsData> Members

		public override ScreenshotsData GetProcessedData(ProcessedDataPackage state)
		{
			if (IsDataExists(state) == false)
				return null;

			DirectoryInfo di = new DirectoryInfo(state.DumpFolder);
			FileInfo[] fi = di.GetFiles(String.Format(screenShotsFilePattern, state.CollectionID));

			ScreenshotsData sd = new ScreenshotsData();

			foreach (FileInfo f in fi)
			{
				sd.Add(new Screenshot(f.FullName));
			}

			sd.FilenameStructure = screenShotsFilePattern;

			return sd;
		}

		#endregion

		#region DataProcessor Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
			DirectoryInfo di = new DirectoryInfo(state.DumpFolder);
			FileInfo[] fi = di.GetFiles(String.Format(screenShotsFilePattern, state.CollectionID));

			return fi.Length > 0;
		}

		#endregion
	}
}
