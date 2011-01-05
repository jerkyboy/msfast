//=======================================================================
/* Project: MSFast (MySpace.MSFast.SuProxy)
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
using MySpace.MSFast.SuProxy.Pipes.Utils;
using System.Text.RegularExpressions;
using System.IO;

namespace MySpace.MSFast.SuProxy.Pipes.Parsing
{
	public class HttpUnchunkPipe : HttpBreakerPipe
	{
		private bool IsChunked = false;
		private static Regex TransferEncodingRegex = new Regex("Transfer-Encoding: chunked\r\n", RegexOptions.Compiled);
		private String header = "";

		public override void SendHeader(string header)
		{
			this.IsChunked = TransferEncodingRegex.IsMatch(header); 
			
			if(!this.IsChunked){
				base.SendHeader(header);
				return;
			}
			
			this.header = header;
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
			if (!this.IsChunked)
			{
				base.SendBodyData(buffer, offset, length);
				return;
			}

			buffer = FixChunked(buffer, offset, length);

			this.header = RemoveChunked(this.header);
			this.header = SetContentLength(this.header, buffer.Length);
			
			base.SendHeader(this.header);
			base.SendBodyData(buffer, 0, buffer.Length);
		}

		private string RemoveChunked(string header)
		{
			return TransferEncodingRegex.Replace(header, String.Empty);
		}

		private string SetContentLength(string header, int newLength)
		{
			return header;
		}

		private byte[] FixChunked(byte[] buffer, int offset, int length)
		{
			int shouldRead = -1;
			String lengthStr = String.Empty;

			MemoryStream memStrm = new MemoryStream();

			for (int i = offset; i < buffer.Length && i < length; i++)
			{
				//First char must be a hex
				if (lengthStr.Length == 0 && !IsHexChar(buffer[i]))
					return null;

				if (shouldRead == -1)
				{
					if (IsHexChar(buffer[i]))
					{
						lengthStr += (char)buffer[i];
					}
					else
					{
						shouldRead = Int32.Parse(lengthStr, System.Globalization.NumberStyles.HexNumber);
					}
				}
				if (shouldRead == 0)
				{
					byte[] r = memStrm.ToArray();
					memStrm.Close();
					return r;
				}
				if (buffer[i] == '\r' && buffer[i + 1] == '\n')
				{
					memStrm.Write(buffer, i + 2, shouldRead);
					i += 3 + shouldRead;
					shouldRead = -1;
					lengthStr = String.Empty;
				}
			}

			byte[] rs = memStrm.ToArray();
			memStrm.Close();
			return rs;
		}
		private bool IsHexChar(byte c)
		{
			return ((c >= 48 && c <= 57) || (c >= 65 && c <= 70) || (c >= 97 && c <= 102));
		}

	}
}
