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
using System.Collections;

namespace MySpace.MSFast.DataProcessors.Screenshots
{
	public class Screenshot
	{
		public String Filename = "";
		public bool IsThumbnail = false;

		public Screenshot(String filename) 
		{
			IsThumbnail = filename.IndexOf("_t.") != -1;
			this.Filename = filename;
		}
	}

	public class ScreenshotsData : List<Screenshot>, ProcessedData
	{
		public String FilenameStructure;
		public Screenshot DefaultScreenshot = null;
	}
}
