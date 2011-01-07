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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace MySpace.MSFast.SuProxy.Pipes.Utils
{
	public class HttpCustomRequestPipe : HttpPipe
	{
		MemoryStream ms = new MemoryStream();
		static Regex PostDataRegex = new Regex("(?:Content-Type: multipart/form-data; boundary=[-]*?)([0-9A-Za-z]*)\r\n(?:\\s|.)*?(?:[-]*?\\1)\r\nContent-Disposition: form-data; name=\"customRequestBody\"\r\n\r\n((\\s|.)*?)(?:\r\n[-]*?\\1)", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public override void SendData(byte[] buffer, int offset, int length)
		{
			ms.Write(buffer, offset, length);
		}

		public override void Flush() 
		{
			byte[] buffer = ParseDataCreateSocketAndGetRequest(ms.ToArray());
			ms.Close();

			base.SendData(buffer, 0, buffer.Length);
			buffer = null;

			base.Flush();
		}

		private byte[] ParseDataCreateSocketAndGetRequest(byte[] data)
		{
			String bufStr = Encoding.ASCII.GetString(data, 0, data.Length);
			Match m = PostDataRegex.Match(bufStr);
			
			if (m.Success && this.PipesChain.ChainState.ServerSocket == null)
			{
				String s = m.Groups[2].ToString();
				TcpClient tc = new TcpClient("localhost", this.Configuration.ProxyPort);
				this.PipesChain.ChainState.ServerSocket = tc.Client;
				return Encoding.ASCII.GetBytes(s);
			}

			return data;
		}
	}
}