//=======================================================================
/* Project: MSFast (MySpace.MSFast.SysImpl.Win32.InternetExplorer)
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
using System.ComponentModel;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.TestBrowser
{
	public static class ProxyHelper
	{
		public static bool UnsetProxy()
		{
			return SetProxy(null, null);
		}
		public static bool SetProxy(string strProxy)
		{
			return SetProxy(strProxy, "local;modules.corp.myspace.com");
		}

		[DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern IntPtr InternetOpen(string lpszAgent, int dwAccessType, string lpszProxyName,string lpszProxyBypass, int dwFlags);

		[DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern IntPtr InternetOpenUrl(IntPtr hInternet, String lpszUrl, String lpszHeaders, int dwHeadersLength, int dwFlags, int dwContext);

		[DllImport("wininet.dll", SetLastError = true)]
		static extern bool InternetQueryOption(IntPtr hInternet, InternetOption dwOption, IntPtr lpBuffer, int dwBufferLength);

		const int INTERNET_OPEN_TYPE_PRECONFIG = 0; // use registry configuration
		const int INTERNET_OPEN_TYPE_DIRECT = 1; // direct to net
		const int INTERNET_OPEN_TYPE_PROXY = 3;  // via named proxy
		const int INTERNET_OPEN_TYPE_PRECONFIG_WITH_NO_AUTOPROXY = 4; // prevent using java/script/INS
		

		[DllImport("wininet.dll", EntryPoint = "InternetCloseHandle")]
		public static extern long InternetCloseHandle(IntPtr hInet);


		


		public static bool SetProxy(string strProxy, string exceptions)
		{

			IntPtr hInternet = InternetOpen("tempconn", INTERNET_OPEN_TYPE_DIRECT, null, null, 0);

			InternetPerConnOptionList list = new InternetPerConnOptionList();

			int optionCount = string.IsNullOrEmpty(strProxy) ? 1 : (string.IsNullOrEmpty(exceptions) ? 2 : 3);

			InternetConnectionOption[] options = new InternetConnectionOption[optionCount];

			// USE a proxy server ...
			options[0].m_Option = PerConnOption.INTERNET_PER_CONN_FLAGS;
			options[0].m_Value.m_Int = (int)((optionCount < 2) ? PerConnFlags.PROXY_TYPE_DIRECT : (PerConnFlags.PROXY_TYPE_PROXY));

			// use THIS proxy server
			if (optionCount > 1)
			{
				options[1].m_Option = PerConnOption.INTERNET_PER_CONN_PROXY_SERVER;
				options[1].m_Value.m_StringPtr = Marshal.StringToHGlobalAuto(strProxy);

				// except for these addresses ...
				if (optionCount > 2)
				{
					options[2].m_Option = PerConnOption.INTERNET_PER_CONN_PROXY_BYPASS;
					options[2].m_Value.m_StringPtr = Marshal.StringToHGlobalAuto(exceptions);
				}
			}

			// default stuff
			list.dwSize = Marshal.SizeOf(list);
			list.szConnection = IntPtr.Zero;
			list.dwOptionCount = options.Length;
			list.dwOptionError = 0;

			int optSize = Marshal.SizeOf(typeof(InternetConnectionOption));
			
			// make a pointer out of all that ...
			IntPtr optionsPtr = Marshal.AllocCoTaskMem(optSize * options.Length);
			
			// copy the array over into that spot in memory ...
			for (int i = 0; i < options.Length; ++i)
			{
				IntPtr opt = new IntPtr(optionsPtr.ToInt32() + (i * optSize));
				Marshal.StructureToPtr(options[i], opt, false);
			}

			list.options = optionsPtr;

			// and then make a pointer out of the whole list
			IntPtr ipcoListPtr = Marshal.AllocCoTaskMem((Int32)list.dwSize);
			Marshal.StructureToPtr(list, ipcoListPtr, false);

			
			// and finally, call the API method!
			
			int returnvalue = NativeMethods.InternetSetOption(hInternet, InternetOption.INTERNET_OPTION_PER_CONNECTION_OPTION, ipcoListPtr, list.dwSize) ? -1 : 0;
			
			int returnvalue2 = NativeMethods.InternetSetOption(IntPtr.Zero, InternetOption.INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0) ? -1 : 0;

			int returnvalue3 = NativeMethods.InternetSetOption(IntPtr.Zero, InternetOption.INTERNET_OPTION_REFRESH, IntPtr.Zero, 0) ? -1 : 0;

			//if (hInternet != IntPtr.Zero)
				//InternetCloseHandle(hInternet);

			if (returnvalue == 0)
			{  // get the error codes, they might be helpful
				returnvalue = Marshal.GetLastWin32Error();
			}

			// FREE the data ASAP
			Marshal.FreeCoTaskMem(optionsPtr);
			Marshal.FreeCoTaskMem(ipcoListPtr);
			if (returnvalue > 0)
			{  // throw the error codes, they might be helpful
				throw new Win32Exception(Marshal.GetLastWin32Error());
			}
			
			return (returnvalue < 0);
		}
	}

	#region WinInet structures
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct InternetPerConnOptionList
	{
		public int dwSize;               // size of the INTERNET_PER_CONN_OPTION_LIST struct
		public IntPtr szConnection;         // connection name to set/query options
		public int dwOptionCount;        // number of options to set/query
		public int dwOptionError;           // on error, which option failed
		//[MarshalAs(UnmanagedType.)]
		public IntPtr options;
	};

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct InternetConnectionOption
	{
		static readonly int Size;
		public PerConnOption m_Option;
		public InternetConnectionOptionValue m_Value;
		static InternetConnectionOption()
		{
			InternetConnectionOption.Size = Marshal.SizeOf(typeof(InternetConnectionOption));
		}

		// Nested Types
		[StructLayout(LayoutKind.Explicit)]
		public struct InternetConnectionOptionValue
		{
			// Fields
			[FieldOffset(0)]
			public System.Runtime.InteropServices.ComTypes.FILETIME m_FileTime;
			[FieldOffset(0)]
			public int m_Int;
			[FieldOffset(0)]
			public IntPtr m_StringPtr;
		}
	}
	#endregion

	#region WinInet enums
	//
	// options manifests for Internet{Query|Set}Option
	//
	public enum InternetOption : uint
	{
		INTERNET_OPTION_PER_CONNECTION_OPTION = 75,
		INTERNET_OPTION_REFRESH = 37,
		INTERNET_OPTION_SETTINGS_CHANGED = 39
	}

	//
	// Options used in INTERNET_PER_CONN_OPTON struct
	//
	public enum PerConnOption
	{
		INTERNET_PER_CONN_FLAGS = 1, // Sets or retrieves the connection type. The Value member will contain one or more of the values from PerConnFlags
		INTERNET_PER_CONN_PROXY_SERVER = 2, // Sets or retrieves a string containing the proxy servers.  
		INTERNET_PER_CONN_PROXY_BYPASS = 3, // Sets or retrieves a string containing the URLs that do not use the proxy server.  
		INTERNET_PER_CONN_AUTOCONFIG_URL = 4,//, // Sets or retrieves a string containing the URL to the automatic configuration script.  
		INTERNET_PER_CONN_AUTODISCOVERY_FLAGS = 5,
		INTERNET_OPTION_PER_CONNECTION_OPTION = 75

	}

	//
	// PER_CONN_FLAGS
	//
	[Flags]
	public enum PerConnFlags
	{
		PROXY_TYPE_DIRECT = 0x00000001,  // direct to net
		PROXY_TYPE_PROXY = 0x00000002,  // via named proxy
		PROXY_TYPE_AUTO_PROXY_URL = 0x00000004,  // autoproxy URL
		PROXY_TYPE_AUTO_DETECT = 0x00000008   // use autoproxy detection
	}
	#endregion

	internal static class NativeMethods
	{
		[DllImport("WinInet.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool InternetSetOption(IntPtr hInternet, InternetOption dwOption, IntPtr lpBuffer, int dwBufferLength);
	}

}
