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
using System.Runtime.InteropServices;
using System.Threading;
using MySpace.MSFast.SuProxy.Exceptions;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.SuProxy.Proxlets
{
	public class ProxletsPool
	{
		private Proxlet[] proxlets = null;
		private SuProxyConfiguration config = null;

		public ProxletsPool(SuProxyConfiguration config) 
		{
			this.config = config;

			this.proxlets = new Proxlet[config.ProxletsPoolSize];
			for (int i = 0; i < this.proxlets.Length; i++) 
			{
				this.proxlets[i] = new HttpProxlet(this, config); 
			}
		}
		
		public Proxlet GetProxlet() 
		{
			lock (this.proxlets)
			{
				int countTry = this.config.ProxletsWaitTimeoutRetries;
				while (countTry > 0)
				{
					for (int i = 0; i < this.proxlets.Length; i++)
					{
						if (this.proxlets[i].State == Proxlet.ProxletState.Available)
						{
							this.proxlets[i].State = Proxlet.ProxletState.Locked;
							return this.proxlets[i];
						}
					}
					Monitor.Wait(this.proxlets, this.config.ProxletsWaitTimeout);
					countTry--;
				}
				throw new GetProxletTimedOutException();
			}
		}

		public void ProxletReleased(Proxlet p) 
		{
			lock (this.proxlets)
			{
				Monitor.PulseAll(this.proxlets);
			}
		}

		public void Dispose() 
		{
			if (proxlets == null) 
			{
				return;
			}

			foreach (Proxlet p in proxlets) 
			{
				p.Dispose();
			}

			this.proxlets = null;
		}

		public void KillProxlets()
		{
			lock (this.proxlets)
			{
				for (int i = 0; i < this.proxlets.Length; i++)
				{
					if (this.proxlets[i].State == Proxlet.ProxletState.Processing)
					{
						this.proxlets[i].KillProcess();
					}
				}
			}
		}
	}
}
