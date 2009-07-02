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

namespace MySpace.MSFast.DataProcessors.Download
{
	public class DownloadState
	{
		public URLType URLType = URLType.Unknown;
		public int TotalSent = 0;
		public int TotalReceived = 0;
		public long SendingRequestStartTime = -1;
		public long SendingRequestEndTime = -1;
		public long ReceivingResponseStartTime = -1;
		public long ReceivingResponseEndTime = -1;
		public long ConnectionStartTime = -1;
		public long ConnectionEndTime = -1;
		public String URL = String.Empty;
        public String FileGUID = null;
	}

	public enum URLType : int
	{
		Image = 1,
		CSS = 2,
		JS = 3,
		Unknown = 4
	}

	public class DownloadData : LinkedList<DownloadState>, ProcessedData
	{
		public String DownloadDataDumpFilename = "";

		public int TotalCSS = 0;
		public int TotalCSSWeight = 0;
		public int TotalImages = 0;
		public int TotalImagesWeight = 0;
		public int TotalJS = 0;
		public int TotalJSWeight = 0;
		public int TotalFiles = 0;
		public int TotalDataReceived = 0;
		public int TotalDataSent = 0;
	}
}
