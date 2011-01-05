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
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Sockets;

namespace MySpace.MSFast.SuProxy.Pipes.Utils
{
    public class HttpLocalFileResponsePipe : HttpPipe
	{
		private static String ResponseHeader = "HTTP/1.1 200 OK\r\n" +
											   "Server: SuProxy\r\n" + 
											   "Accept-Ranges: bytes\r\n" + 
											   "Vary: Accept-Encoding\r\n" +
											   "Content-Length: {0}\r\n\r\n";

        private String LocalFile = null;

        public override void Init(Dictionary<object, object> dictionary)
        {
            base.Init(dictionary);

            if (dictionary.ContainsKey("file"))
            {
                LocalFile = (String)dictionary["file"];
            }
        }

		public override void SendData(byte[] buffer, int offset, int length)
		{
            
		}

		public override void Flush()
		{
			try
			{
                if (!String.IsNullOrEmpty(LocalFile) && File.Exists(LocalFile) && this.PipesChain.ChainState.ServerSocket == null)
                {
                    byte[] b = File.ReadAllBytes(LocalFile);
                    String res = String.Format(ResponseHeader, b.Length);
					byte[] h = Encoding.UTF8.GetBytes(res);

					base.SendData(h, 0, h.Length);
					base.SendData(b, 0, b.Length);
				}
			}
			catch 
			{

			}
			
			base.Flush();
		}

	}
}