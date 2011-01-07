//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.Core.Http;
using System.IO;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.Engine.SuProxy.Utils;
using MySpace.MSFast.Engine.SuProxy.Proxy;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Tracking
{
	public class HttpTracerStatePipe : HttpPipe
	{
		private static byte[] responseHeader = null;
        private static Regex parseQueryString = new Regex("[\\?](START|STOP)_TRACKING=[0-9]*~([A-Za-z0-9=/\\+]*)~([A-Za-z0-9=/\\+]*)", RegexOptions.Compiled);

		
		public override void Init(System.Collections.Generic.Dictionary<object, object> dictionary)
		{
			base.Init(dictionary);
			if(responseHeader == null)
				responseHeader = Encoding.UTF8.GetBytes("HTTP/1.1 204 OK\r\n\r\n");
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
			if (this.PipesChain.ChainState.ContainsKey(HttpTracerPipe.STATE_KEY))
			{
                ((HttpTransaction)this.PipesChain.ChainState[HttpTracerPipe.STATE_KEY]).IsTrackable = false;
			}

			if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI"))
			{
				String url = (String)this.PipesChain.ChainState["REQUEST_URI"];
				
				Match m = parseQueryString.Match(url);

				if (m.Success)
				{
					try
                    {
                        Uri originalUrl = new Uri(base64Decode(m.Groups[2].Value));
                        Uri startFrom = new Uri(base64Decode(m.Groups[3].Value));
                        
                        bool isStart = m.Groups[1].Value == "START";

						if (isStart)
						{
                            HttpTracerPipe.AddURLMask(startFrom, originalUrl);
                            HttpTracerPipe.StartTracking();
						}
						else if(this.Configuration is EngineSuProxyConfiguration)
						{
                            DownloadDumpFilesInfo ddfi = new DownloadDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration);
                            HttpTracerPipe.FlushTracker(ddfi, originalUrl);
						}
					}
					catch 
					{
					
					}
				}
			}
		}

		private static string base64Decode(string data)
		{
			try
			{
				System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
				System.Text.Decoder utf8Decode = encoder.GetDecoder();

				byte[] todecode_byte = Convert.FromBase64String(data);
				int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
				char[] decoded_char = new char[charCount];
				utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
				string result = new String(decoded_char);
				return result;
			}
			catch (Exception e)
			{
				throw new Exception("Error in base64Decode" + e.Message);
			}
		}
		
		public override void Flush()
		{
			
			base.SendData(responseHeader, 0, responseHeader.Length);
			base.Flush();
		}
	}
}
