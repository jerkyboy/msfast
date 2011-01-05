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
using System.Text.RegularExpressions;
using System.IO;

namespace MySpace.MSFast.SuProxy.Pipes.Utils
{
	public class HttpFixResponseHeaderPipe : HttpBreakerPipe
	{

		private static Regex TransferEncodingRegex = new Regex("Transfer-Encoding: chunked\r\n", RegexOptions.Compiled);
		private static Regex RemoveKeepAlive = new Regex("Connection: ([a-zA-Z\\-]*?)\r\n", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
		private static Regex FindContentLength = new Regex("Content-Length: (.*?)\r\n", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public bool IsChunked = false;
		private String header = "";

		private MemoryStream bodybuffer = null;			 

		public override void SendHeader(string header)
		{
			//Fix connection
			if (RemoveKeepAlive.IsMatch(header))
			{
				header = RemoveKeepAlive.Replace(header, "Connection: close\r\n");
			}
			else
			{
				header = header.Substring(0, header.IndexOf("\r\n")) + "\r\nConnection: close" + header.Substring(header.IndexOf("\r\n"), header.Length - header.IndexOf("\r\n"));
			}

			//Fix Chunked/ContentLength
			this.IsChunked = TransferEncodingRegex.IsMatch(header);

			if (this.IsChunked)
			{
				base.SendHeader(header);
				return;
			}

			this.header = header;

		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
			if (this.IsChunked)
			{
				base.SendBodyData(buffer, offset, length);
				return;
			}

			if (bodybuffer == null) 
			{
				bodybuffer = new MemoryStream();
			}

			bodybuffer.Write(buffer,offset,length);
		}

		public override void  Flush()
		{
			if (!this.IsChunked && this.header != null)
			{
				byte[] buffer = null;
				
				if(bodybuffer == null)
					buffer = new byte[0];
				else
					buffer = bodybuffer.ToArray();

				this.header = SetContentLength(this.header, buffer.Length);
				
				base.SendHeader(this.header);
				base.SendBodyData(buffer, 0, buffer.Length);
			}

			base.Flush();
		}

		private string SetContentLength(string header, int length)
		{

			if (FindContentLength.IsMatch(header))
			{
				header = FindContentLength.Replace(header, "Content-Length: " + length + "\r\n");
			}
			else
			{
				header = header.Substring(0, header.IndexOf("\r\n")) + "\r\nContent-Length: " + length + header.Substring(header.IndexOf("\r\n"), header.Length - header.IndexOf("\r\n"));
			}

			
			return header;	
		}


	}
}
