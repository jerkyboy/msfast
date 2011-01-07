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
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Core.Http;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.Core.Logger;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Tracking
{
	public class HttpTracerPipe : HttpPipe
	{
		private static bool isTracking = false;

        private static Dictionary<Uri, Uri> URLMasks = new Dictionary<Uri, Uri>();
        private static LinkedList<HttpTransaction> httpTransactions = new LinkedList<HttpTransaction>();
		private int MaxBufferSizeWhenIdle = 5;
		private int MaxBufferSizeWhenTracking = 300;

        public static String STATE_KEY = "HttpTracerPipe_HttpTransaction";

		private bool isRequest = false;

		public override void Init(Dictionary<object, object> dictionary) 
		{
			base.Init(dictionary);
			this.isRequest = dictionary.ContainsKey("request");
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
           	if (isRequest && this.PipesChain.ChainState.ContainsKey(STATE_KEY) == false && this.PipesChain.ChainState.ContainsKey("REQUEST_URI"))
			{
                HttpTransaction httpTranc = new HttpTransaction();

				httpTranc.URL = new Uri((String)this.PipesChain.ChainState["REQUEST_URI"]);
                httpTranc.Mode = HttpMode.SendingRequest;

				httpTranc.ConnectionStartTime = DateTime.Now;
				httpTranc.SendingRequestStartTime = DateTime.Now;
				httpTranc.TotalSent = length;

                TestEvents.FireProgressEvent(TestEventType.RequestingFile, httpTranc.URL);
                TestEvents.FireProgressEvent(TestEventType.SendingData, length, 0, httpTranc.URL);

				this.PipesChain.ChainState.Add(STATE_KEY, httpTranc);
				
				AddRequestToTracker(httpTranc);
			}
			else if(isRequest)
			{
                HttpTransaction httpTranc = (HttpTransaction)this.PipesChain.ChainState[STATE_KEY];
				httpTranc.TotalSent += length;
                TestEvents.FireProgressEvent(TestEventType.SendingData, length, 0, httpTranc.URL);
            }
			else if(isRequest == false && this.PipesChain.ChainState.ContainsKey(STATE_KEY))
			{
                HttpTransaction httpTranc = (HttpTransaction)this.PipesChain.ChainState[STATE_KEY];

                if (httpTranc.Mode == HttpMode.WaitingForResponse)
                {
                    httpTranc.Mode = HttpMode.ReceivingResponse;
					httpTranc.ReceivingResponseStartTime = DateTime.Now;
				}
				httpTranc.TotalReceived += length;
                
                TestEvents.FireProgressEvent(TestEventType.ReceivingData, length, 0, httpTranc.URL);
            }
			
			base.SendData(buffer, offset, length);
		}

		public override void Flush()
		{
			if (this.PipesChain.ChainState.ContainsKey(STATE_KEY))
			{
                HttpTransaction httpTranc = (HttpTransaction)this.PipesChain.ChainState[STATE_KEY];
				
				if (isRequest)
				{
                    httpTranc.Mode = HttpMode.WaitingForResponse;
					httpTranc.SendingRequestEndTime = DateTime.Now;
				}
				else
				{
                    TestEvents.FireProgressEvent(TestEventType.ResponseEnded, httpTranc.URL);
                    httpTranc.Mode = HttpMode.Completed;
					httpTranc.ReceivingResponseEndTime = DateTime.Now;
					httpTranc.ConnectionEndTime = DateTime.Now;
				}
			}

			base.Flush();
		}

		public override void Close()
		{
			try
			{
				if(this.PipesChain != null && this.PipesChain.ChainState != null && this.PipesChain.ChainState.ContainsKey(STATE_KEY))
                    ((HttpTransaction)this.PipesChain.ChainState[STATE_KEY]).Mode = HttpMode.Completed;
			}
			catch 
			{
			}
			base.Close();
		}

        private void AddRequestToTracker(HttpTransaction httpTranc)
		{
			lock(httpTransactions){
				while (httpTransactions.Count > (isTracking ? MaxBufferSizeWhenTracking : MaxBufferSizeWhenIdle))
				{
					httpTransactions.RemoveFirst();
				}
				httpTransactions.AddLast(httpTranc);
			}
		}
        private static readonly MSFastLogger log = MSFastLogger.GetLogger("IETestHelp");
        public static void FlushTracker(DownloadDumpFilesInfo fileInfo, Uri fromUrl)
		{
			lock (httpTransactions)
			{
				if (isTracking == false)
					return;

				isTracking = false;

				if(httpTransactions.Count <= 0){
					return;
				}

                HttpTransaction t = null;

                foreach (HttpTransaction tt in httpTransactions)
                {
                    if (tt.IsTrackable && URLMasks.ContainsKey(tt.URL))
                    {
                        tt.OriginalURL = tt.URL;
                        tt.URL = URLMasks[tt.URL];
                    }
                }

				while (httpTransactions.Count > 0)
				{
					t = httpTransactions.First.Value;

                    if (t.URL != null) log.Info(t.URL);
                    if (t.OriginalURL != null) log.Info(t.OriginalURL);

					if (t.IsTrackable && t.URL != null && t.URL.Equals(fromUrl))
                    {
						break;
					}

					httpTransactions.RemoveFirst();
				}                

				if (httpTransactions.Count > 0)
				{
                    HttpFlushPipe.AddFlushQue(fileInfo, new LinkedList<HttpTransaction>(httpTransactions));
				}

				httpTransactions.Clear();
			}
		}


		public static void StartTracking()
		{
			lock (httpTransactions)
			{
				if (isTracking)
					return;
				
				isTracking = true;
			}
		}
        
        public static void AddURLMask(Uri key, Uri maskTo)
        {
            if (URLMasks.ContainsKey(key))
            {
                URLMasks[key] = maskTo;
            }
            else
            {
                URLMasks.Add(key, maskTo);
            }
        }
    }
}
