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
            StringBuilder scripts = new StringBuilder();

            if (this.Configuration is EngineSuProxyConfiguration)
            {
                scripts.Append(CollectorScriptsConfig.Instance.PageDataCollector);
                scripts.AppendFormat(CollectorScriptsConfig.Instance.Constructor, ((EngineSuProxyConfiguration)this.Configuration).CollectionID,
                                                                                  0,
                                                                                  this.CollectionInfoParser.URL,
                                                                                  this.CollectionInfoParser.URLEncoded,
                                                                                  this.CollectionInfoParser.URL);

                foreach (CollectorScript cs in CollectorScriptsConfig.Instance.Values)
			    {
				    scripts.Append(cs.Script);
			    }
                
                scripts.Append(CollectorScriptsConfig.Instance.Event_OnInit);
            }

            String page = String.Format(CollectorScriptsConfig.Instance.PreCollectionHtml, scripts.ToString());

            byte[] b = Encoding.UTF8.GetBytes(page);

            base.SendHeader(String.Format(responseHeader, b.Length));
            base.SendBodyData(b, 0, b.Length);
			base.Flush();
		}
	}
}
