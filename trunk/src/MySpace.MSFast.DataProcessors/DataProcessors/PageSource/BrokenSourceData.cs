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

namespace MySpace.MSFast.DataProcessors.PageSource
{
	public class SourcePiece
	{
		public PageSourceData SourceData = null;


		public SourcePiece(PageSourceData sourceData, int startIndex, int length)
		{
			this.StartIndex = startIndex;
			this.Length = length;
			this.SourceData = sourceData;
		}

		public int StartIndex = -1;
		public int Length = -1;

		public String PeiceSource
		{
			get
			{
				if (this.SourceData == null ||
					String.IsNullOrEmpty(this.SourceData.PageSource) ||
					this.SourceData.PageSource.Length < this.StartIndex + this.Length)
					return null;

				return this.SourceData.PageSource.Substring(this.StartIndex, this.Length);
			}
		}
	}

	public class BrokenSourceData : PageSourceData
	{
		public byte[] RawSourceBreaks;
		public String SourceBreaksFilename;
		public LinkedList<SourcePiece> InjectionBreaks = new LinkedList<SourcePiece>();
	}
}
