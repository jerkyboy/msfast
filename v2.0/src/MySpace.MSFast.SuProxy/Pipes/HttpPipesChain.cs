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
using System.Net.Sockets;

namespace MySpace.MSFast.SuProxy.Pipes
{

	public class HttpPipesChainState : Dictionary<String,Object>
	{
		public Socket ClientSocket = null;
		public Socket ServerSocket = null;
		public HttpProxlet HttpProxlet;
	}

	public class HttpPipesChain : LinkedList<HttpPipe>
	{
		public int PipesChainId;
		public HttpPipesChainState ChainState;
		
		public HttpPipesChain()
		{
			this.ChainState = new HttpPipesChainState();
		}

		public HttpPipe GetFirstPipe() 
		{
			if (this.First != null)
			{
				return this.First.Value;
			}

			return null;
		}

		public HttpPipe GetNextPipe(HttpPipe current) 
		{
			LinkedListNode<HttpPipe> s = this.Find(current);
			
			if(s != null){

				LinkedListNode<HttpPipe> next = s.Next;
				
				if (next != null) 
				{
					return next.Value;
				}
			}
			return null;
		}

		public void AddAfter(HttpPipe afterPipe,HttpPipe[] pipes)
		{
			LinkedListNode<HttpPipe> latestNode = null;
			int index = 0 ;

			if (afterPipe == null)
			{
				this.AddFirst(pipes[index]);
				pipes[index].PipesChain = this;

				index++;
				latestNode = this.First;
			}
			else 
			{
				latestNode = this.Find(afterPipe);
			}

			HttpPipe pipe = null;
			for (int i = index; i < pipes.Length; i++)
			{
				pipe = pipes[i];
				pipe.PipesChain = this;
				this.AddAfter(latestNode, pipe);
				latestNode = this.Find(pipe);
			}
		}
		public void AddFirst(HttpPipe[] pipes)
		{
			this.AddAfter(null, pipes);
		}

	}
}
