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
using System.IO;
using System.Threading;

namespace MySpace.MSFast.SuProxy.Pipes.Throttling
{
	public class HttpThrottlingPipe : HttpPipe
	{
		private bool isSenderRunning = false;
		private bool isFlush = false;
		private Object senderSyncLock = new object();
		private MemoryStream msBuffer = null;
		private long positionOffset = 0;

		private byte[] kbps = null;
		private static int TID = 0;
		private int tid = TID++;

		public override void Init(Dictionary<object, object> dictionary)
		{
			base.Init(dictionary);

			int i_kbps = int.Parse((String)dictionary["kbps"]);
			kbps = new byte[1024 * i_kbps];
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
			if (length == 0)
				return;

			if (kbps == null)
			{
				base.SendData(buffer, offset, length);
				return;
			}

			lock (senderSyncLock) 
			{
				if (isSenderRunning == false) 
				{
					isSenderRunning = true;
					isFlush = false;
					new Thread(this.SendThrottledData).Start();
				}
				if (msBuffer == null) 
				{
					msBuffer = new MemoryStream();
				}
				msBuffer.Write(buffer, offset, length);
				Monitor.PulseAll(senderSyncLock);
			}

		}


		public override void Close()
		{
			lock (senderSyncLock)
			{
				if (isSenderRunning)
				{
					isSenderRunning = false;
					isFlush = true;
				}
				if (msBuffer != null)
				{
					msBuffer.Close();
					msBuffer.Dispose();
				}
				Monitor.PulseAll(senderSyncLock);
			}
			base.Close();
		}
        
        private int sentWithoutSleep = 0;

		private void SendThrottledData()
		{
			int read = 0;
			
			while (isSenderRunning) 
			{
				lock (senderSyncLock)
				{
                    try
                    {
                        if (!isSenderRunning)
                            return;

                        long currentPos = msBuffer.Position;
                        msBuffer.Position = positionOffset;
                        read = msBuffer.Read(kbps, 0, kbps.Length);
                        msBuffer.Position = currentPos;
                        positionOffset += read;

                        if (read != 0)
                        {
                            if (sentWithoutSleep + read >= kbps.Length)
                            {
                                sentWithoutSleep = 0;
                                Thread.Sleep(1000);
                            }
                            sentWithoutSleep += read;
                            base.SendData(kbps, 0, read);
                        }
                        else
                        {
                            if (isFlush == false)
                            {
                                Monitor.Wait(senderSyncLock);
                            }
                            else
                            {
                                msBuffer.Close();
                                base.Flush();
                                return;
                            }
                        }
                    }
                    catch 
                    {
                        return;
                    }
				}
			}
		}

		public override void Flush()
		{
			isFlush = true;
			lock (senderSyncLock)
			{
				Monitor.PulseAll(senderSyncLock);
			}
		}

	}
}
