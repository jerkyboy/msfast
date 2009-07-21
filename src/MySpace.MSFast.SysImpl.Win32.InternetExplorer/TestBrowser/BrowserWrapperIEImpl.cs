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
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySpace.MSFast.SysImpl.Win32.Utils;
using MySpace.MSFast.SysImpl.Win32.ComIterop;
using MySpace.MSFast.Engine.BrowserWrapper;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.SysImpl.Win32;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.TestBrowser
{
    public class BrowserWrapperIEImpl : MySpace.MSFast.Engine.BrowserWrapper.TestBrowser
	{
		private static _BrowserWrapperIEImpl browserWrapperIEImpl = null;
		private static object _lock = new object();

		private static SynchronizationContext _context;

		private int returnCode = -1;
		private Object returnLock = new Object();
	
		public BrowserWrapperIEImpl()
		{
			lock (_lock)
			{
				if (browserWrapperIEImpl == null)
				{
					_context = AsyncOperationManager.SynchronizationContext;
					using (ManualResetEvent mre = new ManualResetEvent(false))
					{
						Thread t = new Thread((ThreadStart)delegate
						{
							browserWrapperIEImpl = new _BrowserWrapperIEImpl(this);
							mre.Set();
							Application.Run();
						});
						t.Name = "IEWrapperMessageLoop";
						t.IsBackground = true;
						t.SetApartmentState(ApartmentState.STA);
						t.Start();
						mre.WaitOne();
					}
				}
			}
		}

		public int StartTest(PageDataCollectorStartInfo startInfo)
		{
			if (startInfo.ClearCache)
			{
				try
				{
                    WatiN.Core.Native.InternetExplorer.WinInet.ClearCache();
				}
				catch
				{
				}
			}

			if (ProxyHelper.SetProxy(startInfo.ProxyAddress) == false)
			{
				TestEnded(false, PageDataCollectorErrors.CantSetProxy);
			}
			else
			{

				browserWrapperIEImpl.StartTest(startInfo);

				lock (returnLock)
				{
					if (returnCode == -1)
					{
						Monitor.Wait(returnLock, startInfo.Timeout * 1000 * 60);
					}
					if (returnCode == -1)
					{
						returnCode = (int)PageDataCollectorErrors.TestTimeout;
					}
				}

			}

			Dispose();

			return returnCode;
		}

		public void TestEnded(bool ok, PageDataCollectorErrors errcode)
		{
			lock (returnLock)
			{
				returnCode = (int)errcode;
				Monitor.PulseAll(returnLock);
			}
		}

		public void Dispose()
		{
			ProxyHelper.SetProxy(null);

			if (browserWrapperIEImpl != null)
			{
				try { browserWrapperIEImpl.Close(); }
				catch { }
				try { browserWrapperIEImpl.Dispose(); }
				catch { }
				try { browserWrapperIEImpl.Destroy(); }
				catch { }
			}

			browserWrapperIEImpl = null;
		}


		internal void TestStarted()
		{
		}
	}

	class _BrowserWrapperIEImpl : Form
	{
		private bool isUIHostRegistered = false;
		private object _uiHostRegisteredLock = new object();
		private BrowserWrapperIEImpl host;

		private SHDocVw.IWebBrowser2 ieInstance;
		private IntPtr _mainHWND = IntPtr.Zero;
		private IntPtr _canvasHWND = IntPtr.Zero;

		private BrowserIEImpl _browser = null;
		private IETestHelp testHelp = null;

		public PageDataCollectorStartInfo StartInfo = null;

		private static int WM_TEST_ENDED = -1;

		static _BrowserWrapperIEImpl()
		{
			if (WM_TEST_ENDED == -1)
			{
				WM_TEST_ENDED = Win32API.RegisterWindowMessage("WM_TEST_ENDED");
			}
		}

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        } 

		public _BrowserWrapperIEImpl(BrowserWrapperIEImpl host)
		{
			this.host = host;

			this.SuspendLayout();

			WebBrowser webBrowserControl = new WebBrowser();
			webBrowserControl.Dock = System.Windows.Forms.DockStyle.Fill;
			webBrowserControl.Name = "webBrowser1";
			webBrowserControl.TabIndex = 0;
			webBrowserControl.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);

			webBrowserControl.Navigate("about:blank");
	
			ieInstance = webBrowserControl.ActiveXInstance as SHDocVw.IWebBrowser2;
			
			_canvasHWND = webBrowserControl.Handle;
			_mainHWND = this.Handle;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserWrapperIEImpl));

			this.Controls.Add(webBrowserControl);

			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Name = "PerfTrackingBackground";
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Text = "Performance Tracking Background Process";
			this.ResumeLayout(false);
            this.FormBorderStyle = FormBorderStyle.None;
			CheckUIHostRegistration();
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_TEST_ENDED && host != null)
			{
				try
				{
					host.TestEnded(false, PageDataCollectorErrors.TestAborted);
				}
				catch { }
				return;
			}
			base.WndProc(ref m);
		}

		void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			CheckUIHostRegistration();
		}
		
		private void CheckUIHostRegistration()
		{
			lock (_uiHostRegisteredLock)
			{
				if (testHelp != null)
					testHelp.StartInfo = this.StartInfo;

				if (isUIHostRegistered || ieInstance == null)
					return;

				ICustomDoc cDoc = ieInstance.Document as ICustomDoc;
				IOleObject oleObj = ieInstance.Document as IOleObject;

				if (oleObj != null && cDoc != null)
				{
					IOleClientSite clientSite = null;
					oleObj.GetClientSite(ref clientSite);

					if (cDoc != null)
					{
						_browser = new BrowserIEImpl(ieInstance, _mainHWND, _canvasHWND);
						testHelp = new IETestHelp(clientSite as IDocHostUIHandler, _browser, host);
						cDoc.SetUIHandler(testHelp);
						isUIHostRegistered = true;
					}
				}
			}
		}

		public void StartTest(PageDataCollectorStartInfo startInfo)
		{
			this.StartInfo = startInfo;
			
			CheckUIHostRegistration();

			ieInstance.Silent = true;
			ieInstance.AddressBar = false;
			ieInstance.MenuBar = false;
			ieInstance.ToolBar = 0;
			ieInstance.StatusBar = false;

			if (startInfo.IsDebug == false)
			{
				long style = Win32API.GetWindowLong(_mainHWND, -20);
				style |= (long)0x80;
				style = Win32API.SetWindowLong(_mainHWND, -20, style);
                Win32API.MoveWindow(_mainHWND, 10000, 0, 1000, 738, false);
			}
			else
			{
				Win32API.MoveWindow(_mainHWND, 0, 0, 1000, 738, false);
			}

			Win32API.ShowWindow(_mainHWND, Win32API.WindowShowStyle.ShowDefault);

			object oEmpty = String.Empty;
            object oURL = startInfo.LaunchWithURL;

			ieInstance.Navigate2(ref oURL, ref oEmpty, ref oEmpty, ref oEmpty, ref oEmpty);
		}

		private delegate void noparams();

		public new void Close() 
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new noparams(this.Close));
				return;
			}
			base.Close();
		}

		public new void Dispose()
		{
			if (this.InvokeRequired)
			{
				this.Invoke(new noparams(this.Dispose));
				return;
			}

			base.Dispose();
		}

		public void Destroy(){
			
			if (ieInstance != null)
			{
				try
				{
					Marshal.ReleaseComObject(ieInstance);
				}
				catch { }
			}
			if(_browser != null)
			{
				_browser.Dispose();
			}

			if (testHelp != null)
			{
				testHelp.Dispose();
			}
			testHelp = null;
			_browser = null;
			host = null;
			ieInstance = null;
			_mainHWND = IntPtr.Zero;
		}
	}

	public class BrowserIEImpl : Browser
	{
		private SHDocVw.IWebBrowser2 ieInstance;
		private IntPtr _mainHWND = IntPtr.Zero;
		private IntPtr _canvasHWND = IntPtr.Zero;

		public BrowserIEImpl(SHDocVw.IWebBrowser2 v, IntPtr mainHWND, IntPtr canvasHWND)
		{
			this._mainHWND = mainHWND;
			this._canvasHWND = canvasHWND;
			this.ieInstance = v;
		}

		public override IntPtr CanvasHWND
		{
			get {
				return _canvasHWND;
			}
		}

		public void RefreshView() 
		{
			ICustomDoc cDoc = (ICustomDoc)this.ieInstance.Document;
			if (cDoc != null)
			{
				IOleClientSite oleObj = cDoc as IOleClientSite;
				if (oleObj != null)
					oleObj.ShowObject();
			}
		}

		public override IntPtr MainHWND 
		{
			get { return _mainHWND; }
		}

		public override string URL
		{
			get
			{
				if(this.ieInstance != null)
					return this.ieInstance.LocationURL;
				return null;
			}
			set
			{
				if (this.ieInstance != null)
				{
					object oEmpty = String.Empty;
					object oURL = value;
					this.ieInstance.Navigate2(ref oURL, ref oEmpty, ref oEmpty, ref oEmpty, ref oEmpty);
				}
			}
		}

		public override bool GetBuffer(Browser.GetBufferCallback callBack)
		{
			return false;
		}


		public void Dispose()
		{
			if (this.ieInstance != null)
			{
				Marshal.ReleaseComObject(this.ieInstance);
			}
			this.ieInstance = null;
			this._mainHWND = IntPtr.Zero;
		}
	}
}
