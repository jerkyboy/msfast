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
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.SuProxy.Pipes.Utils;
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using System.IO;
using System.Reflection;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
	public class PreCollectionPagePipe : HttpBreakerPipe
	{
		private Regex collectQueryParsers = new Regex("PRE_COLLECTION=(.*)&__r=([0-9]*)&__c=([0-9]*)$");
		private static String responseHeader = "HTTP/1.1 200 OK\r\n" +
																			  "Server: SuProxy\r\n" +
																			  "Accept-Ranges: bytes\r\n" +
																			  "Vary: Accept-Encoding\r\n" +
																			  "Content-Length: {0}\r\n\r\n";

		private int CollectFlags = 0;
		private int CollectId = 0;
		private String URLEncoded = "";
		private String URL = "";

		public override void SendHeader(string header)
		{
			if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI"))
			{
				String uriStr = (String)this.PipesChain.ChainState["REQUEST_URI"];
				Match m = collectQueryParsers.Match(uriStr);

				try { 
					this.URLEncoded = m.Groups[1].Value;
					this.URL = Encoding.UTF8.GetString(Convert.FromBase64String(this.URLEncoded));
				}
				catch { }
				try { this.CollectId = int.Parse(m.Groups[2].Value); }
				catch { }
				try { this.CollectFlags = int.Parse(m.Groups[3].Value); }
				catch { }
			}
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
		}

		public override void Flush()
		{
            CollectorsConfig cc = null;
            Stream configStream = null;
            try
            {
                configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MySpace.MSFast.Engine.DataCollectors.config");
                cc = new CollectorsConfig(configStream);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (configStream != null)
                        configStream.Close();
                }
                catch
                {
                }
                configStream = null;
            }

			if (  CollectFlags == 0 ||
				   cc == null ||
                    cc.Collection == null ||
					String.IsNullOrEmpty(cc.Collection.JSMain) ||
					String.IsNullOrEmpty(cc.Collection.Html) ||
					String.IsNullOrEmpty(cc.Collection.JSSetTestID) ||
					String.IsNullOrEmpty(cc.Collection.JSStartCollecting))
				return;

			CollectPageInformation cpi = CollectPageInformation.Render;
			
			try{
				cpi = (CollectPageInformation)Enum.ToObject(typeof(CollectPageInformation), CollectFlags);
			}catch
			{
			}

			StringBuilder scripts = new StringBuilder();

			foreach (CollectorsConfig.Collector cs in cc)
			{
				if ((cpi & cs.CollectType) == cs.CollectType)
				{
					scripts.Append(cs.Collection);
				}
			}

			scripts.Insert(0, String.Format(cc.Collection.JSSetTestID, 0,0,CollectId, this.URL, this.URLEncoded));
			scripts.Insert(0, cc.Collection.JSMain);

			String page = String.Format(cc.Collection.Html, scripts.ToString(), cc.Collection.JSStartCollecting);

			byte[] b = Encoding.UTF8.GetBytes(page);

			base.SendHeader(String.Format(responseHeader, b.Length));
			base.SendBodyData(b,0,b.Length);
			base.Flush();
		}
	}
}
