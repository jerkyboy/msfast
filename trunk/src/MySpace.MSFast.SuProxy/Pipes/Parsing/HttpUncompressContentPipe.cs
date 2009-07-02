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
using System.IO.Compression;

namespace MySpace.MSFast.SuProxy.Pipes.Parsing
{
	public class HttpUncompressContentPipe : HttpBreakerPipe
	{
		private bool IsCompressed = false;
		private static Regex ContentEncodingRegex = new Regex("Content-Encoding: gzip\r\n", RegexOptions.Compiled);
		private String header = "";

		public override void SendHeader(string header)
		{
			this.IsCompressed = ContentEncodingRegex.IsMatch(header);

			if (!this.IsCompressed)
			{
				base.SendHeader(header);
				return;
			}

			this.header = header;
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
			if (!this.IsCompressed)
			{
				base.SendBodyData(buffer, offset, length);
				return;
			}

			buffer = Unzip(buffer, offset, length);
			this.header = RemoveGzip(this.header);
			this.header = SetContentLength(this.header, buffer.Length);

			base.SendHeader(this.header);
			base.SendBodyData(buffer, 0, buffer.Length);
		}

		private string RemoveGzip(string header)
		{
			return ContentEncodingRegex.Replace(header, String.Empty);
		}
		
		private string SetContentLength(string header, int newLength)
		{
			return header;
		}

		private byte[] Unzip(byte[] buffer, int offset, int length)
		{
			MemoryStream results = new MemoryStream();
			MemoryStream ms = new MemoryStream(buffer, offset, length);

			GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress);

			byte[] cBuff = new byte[2048];
			int l = 0;

			while ((l = zipStream.Read(cBuff, 0, cBuff.Length)) != 0)
			{
				results.Write(cBuff, 0, l);
			}

			byte[] r = results.ToArray();

			zipStream.Close();
			ms.Close();
			results.Close();

			return r;
		}

	}
}
