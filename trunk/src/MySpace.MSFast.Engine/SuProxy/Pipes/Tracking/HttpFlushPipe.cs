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
using MySpace.MSFast.Core.Configuration.Common;
using System.IO;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Tracking
{
	public class HttpFlushPipe : HttpPipe
	{
        private static Dictionary<DownloadDumpFilesInfo, LinkedList<HttpTransaction>> que = new Dictionary<DownloadDumpFilesInfo, LinkedList<HttpTransaction>>();
		private static object queLock = new object();

        public static void AddFlushQue(DownloadDumpFilesInfo fileInfo, LinkedList<HttpTransaction> httpTransactions)
		{
			lock (queLock)
			{
                if (que.ContainsKey(fileInfo) == false)
                    que.Add(fileInfo, httpTransactions);
			}
		}

		public override void Close()
		{
			lock (queLock)
			{
                Dictionary<DownloadDumpFilesInfo, LinkedList<HttpTransaction>> tmpque = new Dictionary<DownloadDumpFilesInfo, LinkedList<HttpTransaction>>(que);
                foreach (DownloadDumpFilesInfo fileInfo in tmpque.Keys)
				{
					bool cap = true;

                    foreach (HttpTransaction t in tmpque[fileInfo])
					{
                        if (t.Mode != HttpMode.Completed)
						{
							cap = false;
							break;
						}
					}

					if (cap)
					{
                        Stream s = fileInfo.Open(FileAccess.Write);
                        if (s != null)
                        {
                            DataSerializer.SaveHttpTransactions(s, tmpque[fileInfo]);
                        }
                        que.Remove(fileInfo);
					}

				}
			}
			base.Close();
		}

	}
}
