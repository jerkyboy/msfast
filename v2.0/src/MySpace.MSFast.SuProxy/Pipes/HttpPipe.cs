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
using MySpace.MSFast.SuProxy.Proxlets;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.SuProxy.Pipes
{
	public class HttpPipe
	{
		private bool isClosed = false;
		public bool IsClosed
		{
			get
			{
				return this.isClosed;
			}
		}

		public SuProxyConfiguration Configuration = null;
		public HttpPipesChain PipesChain;
		public HttpPipesChainsFactory ChainsFactory = null;

		public virtual void Init(Dictionary<object, object> dictionary){}
		public virtual void Close()
		{
			if (isClosed) return;
			isClosed = true;
			this.PipesChain = null;
			this.ChainsFactory = null;
		}

		public virtual void StartReceive()
		{
			if (isClosed) return;
			HttpPipe nextPipe = this.PipesChain.GetNextPipe(this);
			
			if(nextPipe != null)
			{
				nextPipe.StartReceive();
			}
		}

		public virtual void SendData(byte[] buffer, int offset, int length)
		{
			if (IsClosed) return;
			HttpPipe nextPipe = this.PipesChain.GetNextPipe(this);

			if (nextPipe != null)
			{
				nextPipe.SendData(buffer, offset, length);
			}
		}

		public virtual void Flush()
		{
			if (IsClosed) return;
			HttpPipe nextPipe = this.PipesChain.GetNextPipe(this);

			if (nextPipe != null)
			{
				nextPipe.Flush();
			}
		}
		
		public virtual void HandleError(Exception e)
		{
			if (IsClosed) return;
			HttpPipe nextPipe = this.PipesChain.GetNextPipe(this);

			if (nextPipe != null)
			{
				nextPipe.HandleError(e);
			}
		}

	}
}
