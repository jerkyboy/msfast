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
using System.Runtime.InteropServices;

namespace MySpace.MSFast.Core.Http
{
	
	[StructLayout(LayoutKind.Sequential), Serializable]
	public struct NetworkDevice
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public String Name;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		public String Description;
	};

	[StructLayout(LayoutKind.Sequential), Serializable]
	public struct NetworkPacket
	{
		public PacketTimestamp ts;
		public UInt32 caplen;
		public UInt32 len;
		unsafe public byte* data;
	};

	[StructLayout(LayoutKind.Sequential), Serializable]
	public struct PacketTimestamp
	{
		public long tv_sec;			  /* seconds */
		public long tv_usec;        /* and microseconds */
	};

	[Flags]
	public enum TCPFlags
	{
			CWR = 128,
			ECE = 64,
			URG = 32,
			ACK = 16,
			PSH = 8,
			RST = 4,
			SYN = 2,
			FIN = 1
	}

	public class PacketMetaData 
	{
		public enum ClientServer
		{
			Client,Server
		}

		public ClientServer Sender;

		public uint SourceIP = 0x00000000;
		public char SourcePort = (char)0x0000;

		public uint DestinationIP = 0x00000000;
		public char DestinationPort = (char)0x0000;

		public uint Sequence = 0;
		public uint Acknowledgment = 0;

		public byte Flags = 0x00;
	}
}
