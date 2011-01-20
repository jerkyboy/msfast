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

using System.IO;
using MySpace.MSFast.Core.Http;

namespace MySpace.MSFast.SuProxy.Pipes.Sockets
{
	public class HttpInboundSocketPipe : HttpPipe
	{
		private static AsyncCallback ascDataReceived = new AsyncCallback(OnDataReceivedCallback);
		bool isClient = false;

		public override void Init(Dictionary<object, object> dictionary)
		{
			isClient = (dictionary.ContainsKey("client"));
		}

		public override void StartReceive()
		{
			if (IsClosed) return;
			Socket readSocket = null;
			HttpObjectParser parser = null;

			if (isClient)
			{
				readSocket = this.PipesChain.ChainState.ClientSocket;
				parser = new HttpRequestParser();
			}
			else
			{
				readSocket = this.PipesChain.ChainState.ServerSocket;
				parser = new HttpResponseParser();
			}

			HttpReaderState httpReaderState = new HttpReaderState(this, readSocket, parser);
            
            BeginReceive(httpReaderState);
		}

		private static void BeginReceive(HttpReaderState httpReaderState)
		{
			try
			{
                if (httpReaderState != null && httpReaderState.Socket == null)
                {
                    httpReaderState.Dispose();
                    return;
                }
				httpReaderState.Socket.BeginReceive(httpReaderState.ReceiveBuffer,
																					0,
																					httpReaderState.ReceiveBuffer.Length,
																					SocketFlags.None,
																					ascDataReceived,
																					httpReaderState);
			}
			catch (Exception e)
			{
				HandleError(e, httpReaderState);
			}
		}

        private static void OnDataReceivedCallback(IAsyncResult asyn)
		{
			HttpReaderState httpReaderState = asyn.AsyncState as HttpReaderState;
			
            try
			{   
                if (httpReaderState.Socket != null && httpReaderState.Socket.Connected &&
                    OnData(httpReaderState, httpReaderState.ReceiveBuffer, 0, httpReaderState.Socket.EndReceive(asyn), (httpReaderState.Socket!=null) ? httpReaderState.Socket.Available : 0))
				{
					BeginReceive(httpReaderState);
				}
				else
				{
					httpReaderState.HttpInboundSocketPipe.Flush();
					httpReaderState.Dispose();
				}
			}
			catch (Exception e)
			{
				HandleError(e, httpReaderState);
			}
		}

		private static bool OnData(HttpReaderState httpReaderState, byte[] b_buffer, int index, int length,int available)
		{            
			lock (httpReaderState)
			{
				if (httpReaderState.HttpObjectParser == null)
				{
					throw new Exception("Unknown HTTP Object Parser");
				}

				httpReaderState.TotalReceived += length;
                
				if (!httpReaderState.HttpObjectParser.IsInitiated())
				{
					if (httpReaderState.TotalReceived > httpReaderState.ReceiveHeaderBuffer.Length)
					{
						throw new Exception("Invalid HTTP Header");
					}

					System.Array.Copy(b_buffer, index, httpReaderState.ReceiveHeaderBuffer, httpReaderState.TotalReceived - length, length);

					String header = GetHeader(httpReaderState.ReceiveHeaderBuffer, httpReaderState.TotalReceived);
					
					if (header != null)
					{
						httpReaderState.HttpObjectParser.Initiate(header);
						httpReaderState.HttpInboundSocketPipe.PipesChain.ChainState["RAW_HEADER_" + ((httpReaderState.HttpInboundSocketPipe.isClient) ? "REQUEST" : "RESPONSE")] = header;
						
						if (httpReaderState.HttpInboundSocketPipe.isClient)
						{
							httpReaderState.HttpInboundSocketPipe.PipesChain.ChainState["REQUEST_URI"] = ((HttpRequestParser)httpReaderState.HttpObjectParser).URL;
						}
						
						if (httpReaderState.TotalReceived <= header.Length)
						{
							length = 0;
						}

						byte[] h_buffer = Encoding.ASCII.GetBytes(header);

						if (h_buffer.Length != 0)
						{
                            httpReaderState.HttpObjectParser.OnData(h_buffer, 0, h_buffer.Length, available);
							httpReaderState.HttpInboundSocketPipe.SendData(h_buffer, 0, h_buffer.Length);
						}

						if (httpReaderState.TotalReceived - length < h_buffer.Length)
						{
							int skip = h_buffer.Length - (httpReaderState.TotalReceived - length);
							index += skip;
							length -= skip;
						}
					}
				}

				if (!httpReaderState.HttpObjectParser.IsInitiated())
					return true;

                httpReaderState.HttpObjectParser.OnData(b_buffer, index, length, available);
                
                if (length > 0)
				{
					httpReaderState.HttpInboundSocketPipe.SendData(b_buffer, index, length);
				}
                
				int expectedLength = httpReaderState.HttpObjectParser.GetExpectedLength();
				
				if (expectedLength == -2)
				{   //"Continue", Reset reader
					httpReaderState.HttpInboundSocketPipe.StartReceive();
					return false;
				}
                
				return (expectedLength > httpReaderState.TotalReceived || expectedLength == -1);
			}
		}

		private static void HandleError(Exception e, HttpReaderState httpReaderState)
		{
            if (httpReaderState != null)
            {
                if(httpReaderState.HttpInboundSocketPipe != null)
                    httpReaderState.HttpInboundSocketPipe.HandleError(e);

                httpReaderState.Dispose();
            }
		}

		private static String GetHeader(byte[] buffer, int length)
		{
			String bufStr = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
			int index = bufStr.IndexOf("\r\n\r\n");

			if (index != -1)
				return bufStr.Substring(0, index + 4);

			return null;
		}

		private class HttpReaderState
		{
			private static int Max_Header_Size = 10240;
			private static int Buffer_Size = 2048;

			public Socket Socket = null;
			public HttpObjectParser HttpObjectParser = null;
			public HttpInboundSocketPipe HttpInboundSocketPipe;

			public byte[] ReceiveHeaderBuffer = null;
			public byte[] ReceiveBuffer = null;
			public int TotalReceived = 0;

			public HttpReaderState(HttpInboundSocketPipe httpInboundSocketPipe, Socket socket, HttpObjectParser httpObjectParser)
			{
				this.HttpInboundSocketPipe = httpInboundSocketPipe;
				this.Socket = socket;
				this.HttpObjectParser = httpObjectParser;
				this.ReceiveHeaderBuffer = new byte[Max_Header_Size];
				this.ReceiveBuffer = new byte[Buffer_Size];
				this.TotalReceived = 0;
			}

			public void Dispose()
			{
				this.Socket = null;
				this.HttpObjectParser = null;
				this.HttpInboundSocketPipe = null;
				this.ReceiveHeaderBuffer = null;
				this.ReceiveBuffer = null;
			}
		}
	}
}
