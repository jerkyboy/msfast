//=======================================================================
/* Project: MSFast (MySpace.MSFast.Core)
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

namespace MySpace.MSFast.Core.Http
{
    public enum HttpMode
	{
		InitiatingConnection,
		SendingRequest,
		WaitingForResponse,
		ReceivingResponse,
		Completed
	}

    public class HttpTransaction : IComparable<HttpTransaction>
	{
		public bool IsTrackable = true;
        public HttpMode Mode;
		public Uri URL;
        public Uri OriginalURL;
		public String TransactionCollectionGroup;

		public char SourcePort = (char)0x0000;
		public uint DestinationIP = 0x00000000;

        public String FileGUID = null;

		public String TransactionId {
			get
			{
                return HttpState.GetHttpStateId(SourcePort, DestinationIP) + "" + URL;
			}
		}

		public int TotalSent = 0;
		public int TotalReceived = 0;

		public DateTime SendingRequestStartTime = new DateTime(1, 1, 1);
		public DateTime SendingRequestEndTime = new DateTime(1, 1, 1);

		public DateTime ReceivingResponseStartTime = new DateTime(1, 1, 1);
		public DateTime ReceivingResponseEndTime = new DateTime(1, 1, 1);

		public DateTime ConnectionStartTime = new DateTime(1, 1, 1);
		public DateTime ConnectionEndTime = new DateTime(1, 1, 1);

        #region IComparable<HttpTransaction> Members

        public int CompareTo(HttpTransaction other)
        {
            return ((this.FileGUID != null && other.FileGUID == this.FileGUID) ? 0 : -1);
        }

        #endregion
    }

	public class HttpState
	{
		public HttpTransaction HttpTransaction = new HttpTransaction();

		#region Http State ID
		public static long GetHttpStateId(PacketMetaData packetMetaData)
		{
			if (packetMetaData.Sender == PacketMetaData.ClientServer.Server)
			{
				return GetHttpStateId(packetMetaData.DestinationPort, packetMetaData.SourceIP);
			}
			else
			{
				return GetHttpStateId(packetMetaData.SourcePort, packetMetaData.DestinationIP);
			}
		}
		public long GetHttpStateId()
		{
            return GetHttpStateId(HttpTransaction.SourcePort, HttpTransaction.DestinationIP);
		}
		public static long GetHttpStateId(char port, uint ip) 
		{
			long l = 0;
			l ^= ip;
			l <<= 16;
			l ^= port;
			return l;			
		}
		#endregion

		public int TotalCapturedData = 0;
        public byte[] HeaderBuffer = null;
		private const int Max_Header_Size = 10240;

		public HttpObjectParser Parser;

        public HttpState(HttpMode mode, char sourcePort, uint destinationIP)
		{
            this.HttpTransaction.ConnectionStartTime = DateTime.Now;
            this.HttpTransaction.SourcePort = sourcePort;
            this.HttpTransaction.DestinationIP = destinationIP;
            this.HttpTransaction.Mode = mode;
		}

		public void Reset(HttpObjectParser parser)
		{
            this.HeaderBuffer = new byte[Max_Header_Size];
            this.TotalCapturedData = 0;
			this.Parser = parser;
		}

		internal void Dispose()
		{
            this.HttpTransaction = null;
			this.Parser = null;
		}
	}
	
}
