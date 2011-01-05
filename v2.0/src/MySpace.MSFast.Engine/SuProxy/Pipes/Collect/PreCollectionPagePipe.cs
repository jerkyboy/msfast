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
using MySpace.MSFast.Engine.SuProxy.Utils;
using MySpace.MSFast.Engine.SuProxy.Proxy;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
	public class PreCollectionPagePipe : HttpBreakerPipe
	{
		private static String responseHeader = "HTTP/1.1 200 OK\r\n" +
																			  "Server: SuProxy\r\n" +
																			  "Accept-Ranges: bytes\r\n" +
																			  "Vary: Accept-Encoding\r\n" +
																			  "Content-Length: {0}\r\n\r\n";

        private CollectionInfoParser CollectionInfoParser = null;

		public override void SendHeader(string header)
		{
            this.CollectionInfoParser = new CollectionInfoParser(this.PipesChain.ChainState);
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

            if (this.CollectionInfoParser.CollectFlags == 0 ||
				   cc == null ||
                    cc.Collection == null ||
					String.IsNullOrEmpty(cc.Collection.JSMain) ||
					String.IsNullOrEmpty(cc.Collection.Html) ||
					String.IsNullOrEmpty(cc.Collection.JSSetTestID) ||
					String.IsNullOrEmpty(cc.Collection.JSStartCollecting))
				return;

			CollectPageInformation cpi = CollectPageInformation.Render;
			
			try{
                cpi = (CollectPageInformation)Enum.ToObject(typeof(CollectPageInformation), this.CollectionInfoParser.CollectFlags);
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

            if (this.CollectionInfoParser != null && this.Configuration is EngineSuProxyConfiguration)
            {
                scripts.Insert(0, String.Format(cc.Collection.JSSetTestID, 0, 0, ((EngineSuProxyConfiguration)this.Configuration).CollectionID, this.CollectionInfoParser.URL, this.CollectionInfoParser.URLEncoded));
                scripts.Insert(0, cc.Collection.JSMain);

                String page = String.Format(cc.Collection.Html, scripts.ToString(), cc.Collection.JSStartCollecting);

                byte[] b = Encoding.UTF8.GetBytes(page);

                base.SendHeader(String.Format(responseHeader, b.Length));
                base.SendBodyData(b, 0, b.Length);
            }
			base.Flush();
		}
	}
}
