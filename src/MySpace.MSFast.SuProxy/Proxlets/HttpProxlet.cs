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
using System.Net;
using System.Net.Sockets;
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.SuProxy.Pipes.Sockets;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.SuProxy.Proxlets
{
	public class HttpProxlet : Proxlet
	{
		private static HttpPipesChainsFactory httpPipesChainsFactory = null;
		private HttpPipesChain currentChain = null;
		private object initiLock = new object();

		public HttpProxlet(ProxletsPool pool, SuProxyConfiguration config)
			: base(pool) 
		{
			lock (initiLock)
			{
				if (httpPipesChainsFactory == null && config.ConfigurationFiles != null)
					httpPipesChainsFactory = new HttpPipesChainsFactory(config); 
			}
			
		}

		public override void Release()
		{
			if (currentChain == null)
			{
				base.Release();
				return;
			}

            HttpPipe pipe = currentChain.GetFirstPipe();
			HttpPipe pipet = null;

			while (pipe != null)
			{
				pipet = pipe;
				pipe = currentChain.GetNextPipe(pipe);
				pipet.Close();
			}

			currentChain.ChainState.Clear();

			if (currentChain.ChainState.ServerSocket != null && currentChain.ChainState.ServerSocket.Connected)
				currentChain.ChainState.ServerSocket.Close();

			if (currentChain.ChainState.ClientSocket != null && currentChain.ChainState.ClientSocket.Connected)
				currentChain.ChainState.ClientSocket.Close();

			currentChain.ChainState.ServerSocket = null;
			currentChain.ChainState.ClientSocket = null;
			currentChain.Clear();

			currentChain = null;

			base.Release();
		}

		public override void ProcessConnection(Socket m_clientSocket)
		{
			/// Get from factory
			HttpPipe[] pipes = httpPipesChainsFactory.GetPipesInChain("BaseChain");
			currentChain = new HttpPipesChain();
			currentChain.AddFirst(pipes);
			currentChain.ChainState.ClientSocket = m_clientSocket;
			currentChain.ChainState.HttpProxlet = this;
			currentChain.GetFirstPipe().StartReceive();
		}

		public override void KillProcess()
		{
			Release();
		}
	}
}
