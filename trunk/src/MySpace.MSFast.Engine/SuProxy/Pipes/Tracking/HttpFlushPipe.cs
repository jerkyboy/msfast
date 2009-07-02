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
using MySpace.MSFast.Core.Http;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Tracking
{
	public class HttpFlushPipe : HttpPipe
	{
        private static Dictionary<String, LinkedList<HttpTransaction>> que = new Dictionary<string, LinkedList<HttpTransaction>>();
		private static object queLock = new object();

        public static void AddFlushQue(String filename, LinkedList<HttpTransaction> httpTransactions)
		{
			lock (queLock)
			{
				if(que.ContainsKey(filename) == false)
					que.Add(filename, httpTransactions);
			}
		}

		public override void Close()
		{
			lock (queLock)
			{
                Dictionary<String, LinkedList<HttpTransaction>> tmpque = new Dictionary<string, LinkedList<HttpTransaction>>(que);
				foreach (String filename in tmpque.Keys)
				{
					bool cap = true;

                    foreach (HttpTransaction t in tmpque[filename])
					{
                        if (t.Mode != HttpMode.Completed)
						{
							cap = false;
							break;
						}
					}

					if (cap)
					{
                        DataSerializer.SaveHttpTransactions(filename, tmpque[filename]);
						que.Remove(filename);
					}

				}
			}
			base.Close();
		}

	}
}
