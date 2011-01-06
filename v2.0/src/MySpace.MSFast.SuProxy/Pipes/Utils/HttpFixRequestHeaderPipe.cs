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

namespace MySpace.MSFast.SuProxy.Pipes.Utils
{
	public class HttpFixRequestHeaderPipe : HttpBreakerPipe
	{
        private static Regex RemoveProxyHeaders = new Regex("^Proxy-", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        private static Regex ChangeKeepAlive = new Regex("Connection: Keep-Alive", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        
		private static Regex RemoveHostname = new Regex("^(GET|POST)\\s(http[s]?://.*?)/", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

		private Regex cleanRequestRegex = null;

		public override void Init(Dictionary<object, object> dictionary)
		{
			base.Init(dictionary);
		
		  if (dictionary.ContainsKey("cleanreq"))
		  {
				cleanRequestRegex = new Regex((String)dictionary["cleanreq"]);
		  }
		}

		public override void SendHeader(string header)
		{
			
			header = RemoveProxyHeaders.Replace(header, "");
            header = ChangeKeepAlive.Replace(header, "Connection: close");

			if (cleanRequestRegex != null)
			{
				header = cleanRequestRegex.Replace(header, "");
			}
			
			Match hostnameMatch = RemoveHostname.Match(header);
			if (hostnameMatch != null && hostnameMatch.Success)
			{
				header = header.Replace(hostnameMatch.Groups[2].ToString(), "");
			}

			base.SendHeader(header);
		}

	}
}
