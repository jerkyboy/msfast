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
using MySpace.MSFast.SuProxy.Exceptions;

namespace MySpace.MSFast.SuProxy.Proxlets
{
	public abstract class Proxlet
	{
		public enum ProxletState 
		{
			Processing,
			Locked,
			Available,
			Disposed
		}

		public ProxletState State = ProxletState.Available;
		private object stateLock = new object();
		private ProxletsPool pool = null;

		public Proxlet(ProxletsPool pool) 
		{
			this.pool = pool;
		}

		public void Process(Socket m_clientSocket)
		{
			lock (stateLock)
			{
				if (State == ProxletState.Disposed)
				{
					throw new ProxletDisposedException();
				}
				else if (State == ProxletState.Processing)
				{
					throw new ProxletBusyException();
				}
				
				State = ProxletState.Processing;

				ProcessConnection(m_clientSocket);
			}
		}

		public virtual void Release() 
		{
			lock (stateLock)
			{
				if (State == ProxletState.Processing)
				{
					State = ProxletState.Available;
					pool.ProxletReleased(this);
				}
				else if (State == ProxletState.Disposed) 
				{
					throw new ProxletDisposedException();
				}
			}
		}

		public virtual void Dispose()
		{
			lock (stateLock)
			{
				this.pool = null;
				this.State = ProxletState.Disposed;
			}
		}

		public abstract void ProcessConnection(Socket m_clientSocket);
		public abstract void KillProcess();

	}
}
