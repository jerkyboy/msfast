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
using System.Reflection;
using System.IO;

namespace MySpace.MSFast.SuProxy.Proxy
{					 
	public class SuProxyConfiguration : Dictionary<String,Object>
	{
		public int ProxyPort;
		public int MaxConnectionsQue;
		public int ProxletsPoolSize;
		public int ProxletsWaitTimeout;
		public int ProxletsWaitTimeoutRetries;

        public String[] ConfigurationFiles= null;
		
		public SuProxyConfiguration(int ProxyPort)
		{
            this.Add("DumpFolder", "C:\\temp");
			this.ProxyPort = ProxyPort;
		}
		public SuProxyConfiguration(int ProxyPort, int MaxConnectionsQue) : this(ProxyPort)
		{
			this.MaxConnectionsQue = MaxConnectionsQue;
		}
		public SuProxyConfiguration(int ProxyPort, int MaxConnectionsQue, int ProxletsPoolSize)
			: this(ProxyPort, MaxConnectionsQue)
		{
			this.ProxletsPoolSize = ProxletsPoolSize;
		}
		public SuProxyConfiguration(int ProxyPort, int MaxConnectionsQue, int ProxletsPoolSize, int ProxletsWaitTimeout)
			: this(ProxyPort, MaxConnectionsQue, ProxletsPoolSize)
		{
			this.ProxletsWaitTimeout = ProxletsWaitTimeout;
		}
		public SuProxyConfiguration(int ProxyPort, int MaxConnectionsQue, int ProxletsPoolSize, int ProxletsWaitTimeout, int ProxletsWaitTimeoutRetries)
			: this(ProxyPort, MaxConnectionsQue, ProxletsPoolSize, ProxletsWaitTimeout)
		{
			this.ProxletsWaitTimeoutRetries = ProxletsWaitTimeoutRetries;
		}

		public static SuProxyConfiguration Default
		{
			get
			{
				return new SuProxyConfiguration(8080, 100, 20,2*60*1000,2);
			}
		}
		
	}
}
