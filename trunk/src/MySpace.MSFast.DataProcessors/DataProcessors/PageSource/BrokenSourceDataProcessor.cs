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

namespace MySpace.MSFast.DataProcessors.PageSource
{
	public class BrokenSourceDataProcessor : DataProcessor<BrokenSourceData>
	{
		private static String filenameFormat = "source_{0}.src";
		private static String filenameFormat_Broken = "source_{0}.src_b";

		#region DataProcessor<BrokenSourceData> Members

		public override bool IsDataExists(ProcessedDataPackage state)
		{
			FileInfo fi = new FileInfo(state.DumpFolder + "\\" + String.Format(filenameFormat, state.CollectionID));
			FileInfo fib = new FileInfo(state.DumpFolder + "\\" + String.Format(filenameFormat_Broken, state.CollectionID));
			return (fi.Exists && fi.Length > 0 && fib.Exists && fib.Length > 0);
		}

		public override BrokenSourceData GetProcessedData(ProcessedDataPackage state)
		{
			if (IsDataExists(state) == false)
				return null;

			String filename = state.DumpFolder + "\\" + String.Format(filenameFormat, state.CollectionID);
			String filename_b = state.DumpFolder + "\\" + String.Format(filenameFormat_Broken, state.CollectionID);

			BrokenSourceData sourceData = new BrokenSourceData();

			StreamReader source = new StreamReader(filename);
			sourceData.PageSource = source.ReadToEnd();
			sourceData.SourceFilename = String.Format(filenameFormat, state.CollectionID);
			sourceData.SourceBreaksFilename = String.Format(filenameFormat_Broken, state.CollectionID);
			source.Close();
			source.Dispose();

			BinaryReader sourceBreaks = new BinaryReader(File.Open(filename_b,FileMode.Open,FileAccess.Read));
			
			MemoryStream ms = new MemoryStream();

			byte[] buffer = new byte[10];

			int startIndex = 0;
			int length = 0;

			while(sourceBreaks.BaseStream.Position < sourceBreaks.BaseStream.Length){
				for (int i = 0; i < buffer.Length; i++)
				{
					buffer[i] = sourceBreaks.ReadByte();
					ms.WriteByte(buffer[i]);
				}
				startIndex = buffer[2];
				startIndex = (startIndex << 8) | buffer[3];
				startIndex = (startIndex << 8) | buffer[4];
				startIndex = (startIndex << 8) | buffer[5];

				length = buffer[6];
				length = (length << 8) | buffer[7];
				length = (length << 8) | buffer[8];
				length = (length << 8) | buffer[9];

				sourceData.InjectionBreaks.AddLast(new SourcePiece(sourceData, startIndex, length));
			}

			sourceData.RawSourceBreaks = ms.ToArray();
			ms.Close();
			ms.Dispose();
			sourceBreaks.Close();

			return sourceData;
		}

		#endregion
	}
}
