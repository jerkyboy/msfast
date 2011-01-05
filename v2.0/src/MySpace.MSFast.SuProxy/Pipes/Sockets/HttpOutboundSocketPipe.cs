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
using System.Net;
using MySpace.MSFast.SuProxy.Pipes.Utils;
using System.IO;

namespace MySpace.MSFast.SuProxy.Pipes.Sockets
{
	public class HttpOutboundSocketPipe : HttpPipe
	{
		private byte[] sendBuffer = null;
		private bool isSendFlushed = false;
		private LinkedList<byte[]> sendBufferQue = null;
		private Object queLocker = new Object();

		private bool isClient = false;

		public override void Init(Dictionary<object, object> dictionary)
		{
			isClient = (dictionary.ContainsKey("client"));

			this.sendBufferQue = new LinkedList<byte[]>();
			this.isSendFlushed = false;
			this.sendBuffer = null;
		}

		public override void SendData(byte[] buffer, int offset, int length)
		{
			lock (queLocker)
			{
				if (length <= 0) return;

				#region Get Server Socket

				if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI") && !isClient && this.PipesChain.ChainState.ServerSocket == null)
				{
                    Uri uri = null;

                    try
                    {
                        uri = new Uri((String)this.PipesChain.ChainState["REQUEST_URI"]);
                    }
                    catch(ArgumentNullException)
                    {
                        return;
                    }

                    if (uri == null || String.IsNullOrEmpty(uri.Host) || uri.Port <= 0)
                        return;

					this.PipesChain.ChainState.ServerSocket = GetSocket(uri.Host, uri.Port);
				}

				#endregion
				

				byte[] cacheBuff = new byte[length];
				System.Array.Copy(buffer, offset, cacheBuff, 0, length);

				if (this.sendBuffer != null)
				{
					this.sendBufferQue.AddFirst(cacheBuff);
				}
				else
				{
					try
					{
						Socket socket = ((isClient) ? this.PipesChain.ChainState.ClientSocket : this.PipesChain.ChainState.ServerSocket);
						
						this.sendBuffer = cacheBuff;

						socket.BeginSend(this.sendBuffer,
																	0,
																	this.sendBuffer.Length,
																	SocketFlags.None,
																	new AsyncCallback(this.OnDataSent),
																	this);
					}
					catch (Exception e)
					{
						HandleError(e);
					}
				}
			}
		}

		private void OnDataSent(IAsyncResult asyn)
		{
			lock (queLocker)
			{
				try
				{
					if (this.PipesChain == null || this.PipesChain.ChainState == null)
						return;

					Socket socket = ((isClient) ? this.PipesChain.ChainState.ClientSocket : this.PipesChain.ChainState.ServerSocket);
					int iRx = socket.EndSend(asyn);

					if (iRx != this.sendBuffer.Length)
					{
						throw new Exception("Error sending packet!");
					}

					byte[] buff = this.sendBuffer;

					if (this.sendBufferQue.Count != 0)
					{
						this.sendBuffer = this.sendBufferQue.Last.Value;
						this.sendBufferQue.RemoveLast();
						socket.BeginSend(this.sendBuffer, 0, this.sendBuffer.Length, SocketFlags.None, new AsyncCallback(this.OnDataSent), this);
						return;
					}

					this.sendBuffer = null;

					if (this.isSendFlushed)
					{
						StartReceive();
					}
				}
				catch (Exception e)
				{
					HandleError(e);
				}
			}
		}

		public override void Flush() 
		{
			lock (queLocker)
			{
				this.isSendFlushed = true;
				if (this.sendBufferQue.Count == 0 && this.sendBuffer == null)
				{
					StartReceive();
				}
			}
		}

		#region Helpers
		private static Socket GetSocket(string server, int port)
		{
			Socket s = null;
			IPHostEntry hostEntry = null;

			// Get host related information.
			hostEntry = Dns.GetHostEntry(server);

			// Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
			// an exception that occurs when the host IP Address is not compatible with the address family
			// (typical in the IPv6 case).
			foreach (IPAddress address in hostEntry.AddressList)
			{
                /*if (address.AddressFamily == AddressFamily.InterNetworkV6)
                    continue;*/

				IPEndPoint ipe = new IPEndPoint(address, port);
				Socket tempSocket =
					 new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				tempSocket.Connect(ipe);

				if (tempSocket.Connected)
				{
					s = tempSocket;
					break;
				}
				else
				{
					continue;
				}
			}
			return s;
		}
		#endregion

	}
}