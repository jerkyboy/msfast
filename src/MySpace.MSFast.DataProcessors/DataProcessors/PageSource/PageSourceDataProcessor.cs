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
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.DataProcessors.PageSource
{
	public class PageSourceDataProcessor : DataProcessor<PageSourceData>
	{
		#region DataProcessor<PageSourceData> Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
            SourceDumpFilesInfo sdfi = new SourceDumpFilesInfo(state);

            if (sdfi.Exist() == false)
                return false;

            FileInfo fi = new FileInfo(sdfi.GetFullPath());

			return (fi.Exists && fi.Length > 0);
		}

		public override PageSourceData GetProcessedData(ProcessedDataPackage state)
		{

            SourceDumpFilesInfo sdfi = new SourceDumpFilesInfo(state);
            Stream sourceStream = sdfi.Open(FileAccess.Read);

            PageSourceData sourceData = new PageSourceData();

            if (sourceStream == null)
                return sourceData;

            StreamReader source = new StreamReader(sourceStream);

            sourceData.SourceFilename = sdfi.GetFilename();
			sourceData.PageSource = source.ReadToEnd();
			source.Close();
			source.Dispose();

			return sourceData;
		}


		#endregion
	}
}
