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
using mshtml;
using System.Runtime.InteropServices.ComTypes;
using SHDocVw;
using System.IO;
using MySpace.MSFast.SysImpl.Win32.ComIterop;
using MySpace.MSFast.SysImpl.Win32.Utils;
using MySpace.MSFast.Engine.BrowserWrapper;
using System.Windows.Forms;
using MySpace.MSFast.SysImpl.Win32;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.Common
{
	public abstract class BrowserLinkObject : Browser, IObserver, IObjectWithSite, HTMLWindowEvents2
	{

		[DllImport("OLE32.DLL", EntryPoint = "CreateStreamOnHGlobal")]
		private static extern int CreateStreamOnHGlobal(IntPtr hGlobalMemHandle, bool fDeleteOnRelease, out IStream pOutStm);

		#region Consts / App IDS

		const int E_FAIL = unchecked((int)0x80004005);
		const int E_NOINTERFACE = unchecked((int)0x80004002);

		protected Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
		protected Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
		protected Guid IID_IServiceProvider = new Guid("6d5140c1-7436-11ce-8034-00aa006009fa");
		protected Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");
		protected Guid IID_IOleCommandTarget = new Guid("b722bccb-4e68-101b-a2bc-00aa00404770");

		protected Guid DIID_HTMLDocumentEvents2 = new Guid("3050f613-98b5-11cf-bb82-00aa00bdce0b");
		protected Guid DIID_HTMLWindowEvents2 = new Guid("3050f625-98b5-11cf-bb82-00aa00bdce0b");

		protected Guid CATID_CommBand = new Guid("00021494-0000-0000-C000-000000000046");
		protected Guid CGID_DeskBand =    new Guid("EB0FE172-1A3A-11D0-89B3-00A0C90A90AC");
																						  
		protected const int DBID_BANDINFOCHANGED = 0;
		protected const int DBID_SHOWONLY = 1;
		protected const int DBID_MAXIMIZEBAND = 2;
		protected const int DBID_PUSHCHEVRON = 3;
		
		public enum SetSiteResults
		{
			NoSite,
			SiteIsNotServiceProvider,
			SiteIsNotAWebBrowser,
			SiteHasNoName,
			SiteIsNotIExplorer,
			CantRegisterEventHandlers,
			UnknownException,
			Success
		}
		#endregion

		#region Private Objects
		protected SHDocVw.IWebBrowser2 m_pIWebBrowser2; // the browser class object
		protected SHDocVw.DWebBrowserEvents2_Event m_pDWebBrowserEvents2; // browser events 
		
		private IntPtr canvasHWND = IntPtr.Zero;
		private IntPtr mainHWND = IntPtr.Zero;
		#endregion

		#region Public Attributes
		public override IntPtr CanvasHWND { get { return canvasHWND; } }
		public override IntPtr MainHWND { get { return mainHWND; } }
		public override string URL
		{
			get
			{
				if (this.m_pIWebBrowser2 == null)
					return null;

				return this.m_pIWebBrowser2.LocationURL;
			}
			set
			{
				if (this.m_pIWebBrowser2 == null)
					return;

				object uri = value;
				object flags = null;
				object target = null;
				object postdata = null;
				object headers = null;
				this.m_pIWebBrowser2.Navigate2(ref uri, ref flags, ref target, ref postdata, ref headers);
			}
		}
		#endregion

		#region Set/Get Site and Release
		public virtual void SetSite(object pUnkSite)
		{
			SetSiteWithStatus(pUnkSite);
		}
		public SetSiteResults SetSiteWithStatus(object pUnkSite)
		{
			if (m_pIWebBrowser2 != null)
				Release();

			if (pUnkSite == null)
				return SetSiteResults.NoSite;

			try
			{
                MySpace.MSFast.SysImpl.Win32.ComIterop.IServiceProvider pSp = pUnkSite as MySpace.MSFast.SysImpl.Win32.ComIterop.IServiceProvider;

				if (pSp == null)
				{
					Release();
					return SetSiteResults.SiteIsNotServiceProvider;
				}

				IntPtr ipWb2 = IntPtr.Zero;

				pSp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out ipWb2);

				if (ipWb2 == IntPtr.Zero)
				{
					Marshal.Release(ipWb2);
					Release();
					return SetSiteResults.SiteIsNotAWebBrowser;
				}

				m_pIWebBrowser2 = Marshal.GetObjectForIUnknown(ipWb2) as IWebBrowser2;
				Marshal.Release(ipWb2);

				if (m_pIWebBrowser2 == null)
				{
					Release();
					return SetSiteResults.SiteIsNotAWebBrowser;
				}

				string sHostName = m_pIWebBrowser2.FullName;

				if (sHostName == null)
				{
					Release();
					return SetSiteResults.SiteHasNoName;
				}

				if (!(sHostName.ToUpper().EndsWith("IEXPLORE.EXE")))
				{
					Release();
					return SetSiteResults.SiteIsNotIExplorer;
				}

				m_pDWebBrowserEvents2 = m_pIWebBrowser2 as SHDocVw.DWebBrowserEvents2_Event;

				this.mainHWND = (IntPtr)m_pIWebBrowser2.HWND;

				if (m_pDWebBrowserEvents2 == null)
				{
					Release();
					return SetSiteResults.CantRegisterEventHandlers;
				}
				
				m_pDWebBrowserEvents2.BeforeNavigate2 += new SHDocVw.DWebBrowserEvents2_BeforeNavigate2EventHandler(this.OnBeforeNavigate);
				m_pDWebBrowserEvents2.DocumentComplete += new SHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
				m_pDWebBrowserEvents2.OnQuit += new SHDocVw.DWebBrowserEvents2_OnQuitEventHandler(this.OnQuit);
                m_pDWebBrowserEvents2.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(this.NavigateComplete2);
			}
			catch
			{
				Release();
				return SetSiteResults.UnknownException;
			}

			CheckPageReady();

			return SetSiteResults.Success;
		}

		public void GetSite(ref System.Guid riid, out object ppvSite)
		{

			ppvSite = null;

			if (m_pIWebBrowser2 != null)
			{

				IntPtr pSite = IntPtr.Zero;
				IntPtr pUnk = Marshal.GetIUnknownForObject(m_pIWebBrowser2);
				Marshal.QueryInterface(pUnk, ref riid, out pSite);
				Marshal.Release(pUnk);
				Marshal.Release(pUnk);

				if (!pSite.Equals(IntPtr.Zero))
				{
					ppvSite = pSite;
				}
				else
				{
					Release();
					Marshal.ThrowExceptionForHR(E_NOINTERFACE);
				}
			}
			else
			{
				Release();
				Marshal.ThrowExceptionForHR(E_FAIL);
			}
		}
		public virtual void Release()
		{
			canvasHWND = IntPtr.Zero;
			mainHWND = IntPtr.Zero;

			if (m_pDWebBrowserEvents2 != null)
			{
				Marshal.ReleaseComObject(m_pDWebBrowserEvents2);
				m_pDWebBrowserEvents2 = null;
			}

			if (m_pIWebBrowser2 != null)
			{
				Marshal.ReleaseComObject(m_pIWebBrowser2);
				m_pIWebBrowser2 = null;
			}
		}

		private bool registeredOnDoc = false;
		public virtual void RegisterDoc()
		{
			if (registeredOnDoc)
				return;

			registeredOnDoc = true;

			if (this.canvasHWND == IntPtr.Zero)
			{
				IntPtr tabWindowClass = Win32API.FindWindowEx(this.mainHWND, IntPtr.Zero, "TabWindowClass", null);
				if (tabWindowClass != IntPtr.Zero)
				{
					IntPtr shellDocObjectView = Win32API.FindWindowEx(tabWindowClass, IntPtr.Zero, "Shell DocObject View", null);
					if (shellDocObjectView != IntPtr.Zero)
					{
						this.canvasHWND = Win32API.FindWindowEx(shellDocObjectView, IntPtr.Zero, "Internet Explorer_Server", null);
						if (this.canvasHWND == IntPtr.Zero)
						{
							this.canvasHWND = this.mainHWND;
						}
					}
				}
			}
		}
		#endregion

		#region Events
		private void OnBeforeNavigate(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
		{
			if (pDisp == this.m_pIWebBrowser2) // Getting event from top window
				SetState(BrowserStatus.Loading);
		}
		private void OnDocumentComplete(object pDisp, ref object URL)
		{
            CheckIsRefreshingForBuffer();
            CheckPageReady();
        }
        private void NavigateComplete2(object pDisp, ref object URL)
        {
            CheckIsRefreshingForBuffer();
            CheckPageReady();
        }
        private void OnQuit()
		{
			SetState(BrowserStatus.Disposed);
			Release();
		}
		#endregion

        private void CheckIsRefreshingForBuffer()
        {
            if (isRefreshingForBuffer
                && m_pIWebBrowser2 != null
                && m_pIWebBrowser2.ReadyState == tagREADYSTATE.READYSTATE_COMPLETE)
            {
                GetBuffer(this.getBufferCallback);
            }
        }
        private void CheckPageReady()
		{
            if (this.State != BrowserStatus.Ready && 
                m_pIWebBrowser2 != null &&
                (m_pIWebBrowser2.ReadyState == tagREADYSTATE.READYSTATE_COMPLETE || 
                m_pIWebBrowser2.ReadyState == tagREADYSTATE.READYSTATE_INTERACTIVE))
			{
				RegisterDoc();
				SetState(BrowserStatus.Ready);
			}
        }

		private void SetState(BrowserStatus status)
		{
			this.buffer = null;

			if ( isRefreshingForBuffer == false || 
                (status != BrowserStatus.Loading && status != BrowserStatus.Processing))
			{
				this.State = status;
			}
		}

		#region Buffer Stuff

		private bool isRefreshingForBuffer = false;
		private String buffer = "";
        private GetBufferCallback getBufferCallback;

		public override bool GetBuffer(GetBufferCallback callBack)
		{

            this.getBufferCallback = callBack;

			if (this.buffer != null && callBack != null)
			{
				callBack(this.buffer);
				return true;
			}

			if (this.m_pIWebBrowser2 == null)
			{
				SetState(BrowserStatus.Error);
				isRefreshingForBuffer = false;
				return false;
			}

			IHTMLDocument2 document = this.m_pIWebBrowser2.Document as IHTMLDocument2;

			if (document == null)
			{
				SetState(BrowserStatus.Error);
				isRefreshingForBuffer = false;
				return false;
			}

			IPersistStreamInit ips = document as IPersistStreamInit;

			if (ips == null)
			{
				SetState(BrowserStatus.Error);
				isRefreshingForBuffer = false;
				return false;
			}

			try
			{
				IStream istream;

				CreateStreamOnHGlobal(IntPtr.Zero, true, out istream);

				if (istream == null)
				{
					SetState(BrowserStatus.Error);
					isRefreshingForBuffer = false;
					return false;
				}

				int cb;
				unsafe
				{
					
					int hr = ips.Save(istream, false);

					if (hr != HRESULT.S_OK)
					{
						Marshal.ThrowExceptionForHR(hr);
						SetState(BrowserStatus.Error);
						return false;
					}

					istream.Seek(0, 0, IntPtr.Zero);

					byte[] buffer = new byte[1024];
					StringBuilder sb = new StringBuilder();

                    int* pcb = &cb;
                    IntPtr readOffset = new IntPtr(pcb);

					do
					{
						istream.Read(buffer, 1024, readOffset);
						sb.Append(System.Text.ASCIIEncoding.UTF8.GetString(buffer, 0, cb));
					} while (cb > 0);

					istream = null;
					ips = null;

					if (sb.Length <= 0)
					{
						SetState(BrowserStatus.Error);
						isRefreshingForBuffer = false;
						return false;
					}

					isRefreshingForBuffer = false;
					this.buffer = sb.ToString();

				}
			}
			catch (Exception e)
			{
				// workaround a bug on IE
				// when there's a "no-cache" http header
				// the HTML Source is invisible, so to fix it,
				// we need to refresh the page..
				if (e is FileNotFoundException)
				{
					// Call refresh unless called before...
					if (isRefreshingForBuffer || m_pIWebBrowser2 == null)
					{
						SetState(BrowserStatus.Error);
						return false;
					}

					isRefreshingForBuffer = true;
					SetState(BrowserStatus.Processing);
					m_pIWebBrowser2.Refresh();

					return true;
				}
				else
				{
					SetState(BrowserStatus.Error);
					return false;
				}
			}

			if (callBack != null)
				callBack(this.buffer);

			return true;
		}

		#endregion

		

		#region HTMLDocumentEvents2 Members

		public void onactivate(IHTMLEventObj pEvtObj) { }
		public void onafterupdate(IHTMLEventObj pEvtObj) { }
        public bool onbeforeactivate(IHTMLEventObj pEvtObj) { return true; }
        public bool onbeforedeactivate(IHTMLEventObj pEvtObj) { return true; }
		public void onbeforeeditfocus(IHTMLEventObj pEvtObj) { }
        public bool onbeforeupdate(IHTMLEventObj pEvtObj) { return true; }
		public void oncellchange(IHTMLEventObj pEvtObj) { }
        public bool onclick(IHTMLEventObj pEvtObj) { return true; }
        public bool oncontextmenu(IHTMLEventObj pEvtObj) { return true; }
        public bool oncontrolselect(IHTMLEventObj pEvtObj) { return true; }
		public void ondataavailable(IHTMLEventObj pEvtObj) { }
		public void ondatasetchanged(IHTMLEventObj pEvtObj) { }
		public void ondatasetcomplete(IHTMLEventObj pEvtObj) { }
        public bool ondblclick(IHTMLEventObj pEvtObj) { return true; }
		public void ondeactivate(IHTMLEventObj pEvtObj) { }
        public bool ondragstart(IHTMLEventObj pEvtObj) { return true; }
        public bool onerrorupdate(IHTMLEventObj pEvtObj) { return true; }
		public void onfocusin(IHTMLEventObj pEvtObj) { }
		public void onfocusout(IHTMLEventObj pEvtObj) { }
        public bool onhelp(IHTMLEventObj pEvtObj) { return true; }
		public void onkeydown(IHTMLEventObj pEvtObj) { }
        public bool onkeypress(IHTMLEventObj pEvtObj) { return true; }
		public void onkeyup(IHTMLEventObj pEvtObj) { }
		public void onmousedown(IHTMLEventObj pEvtObj) { }
		public void onmousemove(IHTMLEventObj pEvtObj) { }
		public void onmouseout(IHTMLEventObj pEvtObj) { }
		public void onmouseover(IHTMLEventObj pEvtObj) { }
		public void onmouseup(IHTMLEventObj pEvtObj) { }
        public bool onmousewheel(IHTMLEventObj pEvtObj) { return true; }
		public void onpropertychange(IHTMLEventObj pEvtObj) { }
		public void onrowenter(IHTMLEventObj pEvtObj) { }
        public bool onrowexit(IHTMLEventObj pEvtObj) { return true; }
		public void onrowsdelete(IHTMLEventObj pEvtObj) { }
		public void onrowsinserted(IHTMLEventObj pEvtObj) { }
		public void onselectionchange(IHTMLEventObj pEvtObj) { }
        public bool onselectstart(IHTMLEventObj pEvtObj) { return true; }
        public bool onstop(IHTMLEventObj pEvtObj) { return true; }
		public void onreadystatechange(IHTMLEventObj pEvtObj) { CheckPageReady(); }
		#endregion

		#region HTMLWindowEvents2 Members
		public void onafterprint(IHTMLEventObj pEvtObj) { }
		public void onbeforeprint(IHTMLEventObj pEvtObj) { }
		public void onbeforeunload(IHTMLEventObj pEvtObj) { }
		public void onblur(IHTMLEventObj pEvtObj) { }
		public void onerror(string description, string url, int line) { }
		public void onfocus(IHTMLEventObj pEvtObj) { }
		public void onload(IHTMLEventObj pEvtObj) { }
		public void onresize(IHTMLEventObj pEvtObj) { }
		public void onscroll(IHTMLEventObj pEvtObj) { }
		public void onunload(IHTMLEventObj pEvtObj) { }
		#endregion

	}
}
