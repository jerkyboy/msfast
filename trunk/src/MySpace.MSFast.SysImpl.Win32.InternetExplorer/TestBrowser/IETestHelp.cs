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
using System.Drawing;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using MySpace.MSFast.SysImpl.Win32.ComIterop;
using MySpace.MSFast.SysImpl.Win32.InternetExplorer.TestBrowser;
using MySpace.MSFast.SysImpl.Win32.Utils;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.TestBrowser
{
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface JavascriptExecutioner { void ExecuteJs(string cmdId, string args);}

	[ComVisible(true), GuidAttribute("04C70682-9BD8-443a-9AFC-17849B8A7AAA")] // MY CLASS GUID
	[ProgId("MSFast.Engine")]
	public class IETestHelp : JavascriptExecutioner, IDocHostUIHandler
	{
		private IDocHostUIHandler m_defaultUIHandler;
		private BrowserIEImpl browser = null;
		private BrowserWrapperIEImpl browserWrapperIEImpl = null;
		
        private PerformanceTracker performanceTracker = null;
        private object performanceTrackerLock = new object();

		public PageDataCollectorStartInfo StartInfo = null;

		private delegate void ExecuteJavascriptDelegate(String cmdId, String args);
		private Dictionary<String, ExecuteJavascriptDelegate> JavascriptDelegates;

		public IETestHelp(IDocHostUIHandler defaultUIHandler, BrowserIEImpl browser, BrowserWrapperIEImpl browserWrapperIEImpl)
		{
			this.JavascriptDelegates = new Dictionary<string, ExecuteJavascriptDelegate>();

			this.JavascriptDelegates.Add("getSnapshot", new ExecuteJavascriptDelegate(this.TakeScreenshot));
			this.JavascriptDelegates.Add("startperftest", new ExecuteJavascriptDelegate(this.StartPerformanceTracking));
			this.JavascriptDelegates.Add("endperftest", new ExecuteJavascriptDelegate(this.StopPerformanceTracking));
            this.JavascriptDelegates.Add("saveData", new ExecuteJavascriptDelegate(this.SaveDump));
            this.JavascriptDelegates.Add("onProgress", new ExecuteJavascriptDelegate(this.OnProgress));
			this.JavascriptDelegates.Add("signal_test_started", new ExecuteJavascriptDelegate(this.SignalTestStarted));
			this.JavascriptDelegates.Add("signal_test_ended", new ExecuteJavascriptDelegate(this.SignalTestEnded));
            this.JavascriptDelegates.Add("redirect", new ExecuteJavascriptDelegate(this.RedirectPage));
            this.JavascriptDelegates.Add("clearcache", new ExecuteJavascriptDelegate(this.ClearCache));

			this.browserWrapperIEImpl = browserWrapperIEImpl; 
			this.browser = browser;
			this.m_defaultUIHandler = defaultUIHandler;
		}

		public void ExecuteJs(string cmdId, string args)
		{
			if (this.browser != null)
				this.browser.RefreshView();

			try
			{
				this.JavascriptDelegates[cmdId](cmdId, args);
			}
			catch 
			{
			}

		}

		#region IDocHostUIHandler Members
		public void ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object pdispReserved)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.ShowContextMenu(dwID, ref ppt, pcmdtReserved, pdispReserved);
		}
		public void ShowUI(uint dwID, IntPtr pActiveObject, IntPtr pCommandTarget, IntPtr pFrame, IntPtr pDoc)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.ShowUI(dwID, pActiveObject, pCommandTarget, pFrame, pDoc);
		}
		public void GetHostInfo(ref DOCHOSTUIINFO pInfo)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.GetHostInfo(ref pInfo);
		}
		public void HideUI()
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.HideUI();
		}
		public void UpdateUI()
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.UpdateUI();
		}
		public void EnableModeless(int fEnable)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.EnableModeless(fEnable);
		}
		public void OnDocWindowActivate(int fActivate)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.OnDocWindowActivate(fActivate);
		}
		public void OnFrameWindowActivate(int fActivate)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.OnFrameWindowActivate(fActivate);
		}
		public void ResizeBorder(ref tagRECT prcBorder, IntPtr pUIWindow, int fRameWindow)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.ResizeBorder(ref prcBorder, pUIWindow, fRameWindow);
		}
		public void TranslateUrl(uint dwTranslate, string pchURLIn, ref string ppchURLOut)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.TranslateUrl(dwTranslate, pchURLIn, ref ppchURLOut);
		}
		public uint TranslateAccelerator(ref tagMSG lpMsg, ref Guid pguidCmdGroup, uint nCmdID)
		{
			if (this.m_defaultUIHandler != null)
				return this.m_defaultUIHandler.TranslateAccelerator(ref lpMsg, ref pguidCmdGroup, nCmdID);

			return 0;
		}
		public void GetOptionKeyPath(ref string pchKey, uint dw)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.GetOptionKeyPath(ref pchKey, dw);
		}
		public void GetDropTarget(IntPtr pDropTarget, IntPtr ppDropTarget)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.GetDropTarget(pDropTarget, ppDropTarget);
		}
		public virtual void GetExternal(out object ppDispatch)
		{
			ppDispatch = this;
		}
		public void FilterDataObject(System.Runtime.InteropServices.ComTypes.IDataObject pDO, ref System.Runtime.InteropServices.ComTypes.IDataObject ppDORet)
		{
			if (this.m_defaultUIHandler != null)
				this.m_defaultUIHandler.FilterDataObject(pDO, ref ppDORet);
		}
		#endregion

		public void Dispose()
		{
			if (m_defaultUIHandler != null)
				Marshal.ReleaseComObject(m_defaultUIHandler);
            
            lock (performanceTrackerLock)
            {
                if (performanceTracker != null)
                {
                    try{
                        performanceTracker.StopTracking(null);
                    }catch{
                    }
                    try{
                        performanceTracker.Dispose();
                    }
                    catch { }
                }
                performanceTracker = null;
            }

			browserWrapperIEImpl = null;
			
			browser = null;
			m_defaultUIHandler = null;
		}

		#region Javascript Delegates

		private void RedirectPage(string cmdId, string args)
		{
			if (this.browser != null)
				this.browser.URL = args;
		}

		private void ClearCache(string cmdId, string args)
		{
            try
            {
                WatiN.Core.Native.InternetExplorer.WinInet.ClearCache();
            }
            catch
            {
            }
		}
        
        private void TakeScreenshot(string cmdId, string args)
        {

            if (this.browser == null || this.browser.MainHWND == IntPtr.Zero || this.browser.CanvasHWND == IntPtr.Zero)
                return;

            if (this.StartInfo == null || args == null)
                return;


            String[] argsarr = Regex.Split(args, "~");

            int width = -1;
            int height = -1;

            if (argsarr == null || argsarr.Length == 0)
                return;

            String key = argsarr[0];

            if (argsarr.Length >= 3)
            {
                try { width = int.Parse(argsarr[1]); }
                catch { }
                try { height = int.Parse(argsarr[2]); }
                catch { }
            }

            bool maximize = true;

            if (argsarr.Length == 4)
                try
                {
                    maximize = (int.Parse(argsarr[3]) == 1);
                }
                catch { }

            Stream outs = new ScreenshotsDumpFilesInfo(this.StartInfo).Open(FileAccess.Write, key);
            
            if (outs != null)
            {
                Bitmap bm = ScreenGrabber.GrabScreen(browser.MainHWND, browser.CanvasHWND, maximize);

                if (bm != null)
                {
                    if (width != -1 && height != -1)
                    {
                        bm = ResizeBitmap(bm, width, height);
                    }
                    bm.Save(outs, ImageFormat.Jpeg);

                    try
                    {
                        bm.Dispose();
                    }
                    catch
                    {

                    }
                }
            }

        }
		private static Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
		{
			Bitmap result = new Bitmap(nWidth, nHeight);
			using (Graphics g = Graphics.FromImage((Image)result))
				g.DrawImage(b, 0, 0, nWidth, nHeight);

			try { b.Dispose(); }
			catch { }

			b = null;

			return result;
		}

		private void StartPerformanceTracking(string cmdId, string args)
		{
            lock (performanceTrackerLock)
            {
                try
                {
                    if (performanceTracker != null)
                        return;

                    performanceTracker = new PerformanceTracker(Process.GetCurrentProcess().Id);
                    performanceTracker.StartTracking();
                }
                catch 
                {
                }
            }
		}
		private void StopPerformanceTracking(string cmdId, string args)
		{
            lock (performanceTrackerLock)
            {
                if (performanceTracker == null)
                    return;

                if (this.StartInfo == null || args == null)
                    return;

                Stream outs = null;

                try
                {
                    outs = new PerformanceDumpFilesInfo(this.StartInfo).Open(FileAccess.Write);
                }
                catch
                {
                }

                try{
                    performanceTracker.StopTracking(outs);
                }catch{
                
                }

                try
                {
                    performanceTracker.Dispose();
                    performanceTracker = null;
                }
                catch 
                {
                }
            }
		}

        private void OnProgress(string cmdId, string args)
		{
			if(String.IsNullOrEmpty(args))
                return;

            String[] vals = Regex.Split(args,";");

            if(vals == null || vals.Length != 3)
                return;
            
            try
            {

                CollectPageInformation collectType = (CollectPageInformation)Enum.ToObject(typeof(CollectPageInformation), int.Parse(vals[0]));
                
                int prg = int.Parse(vals[1]);
                int tot = int.Parse(vals[2]);

                if ((collectType & CollectPageInformation.Screenshots_Small) == CollectPageInformation.Screenshots_Small ||
                    (collectType & CollectPageInformation.Screenshots_Full) == CollectPageInformation.Screenshots_Full)
                {
                    TestEvents.FireProgressEvent(TestEventType.CapturingSegment, prg, tot, null);
                }
                else
                {
                    TestEvents.FireProgressEvent(TestEventType.RenderingSegment, prg, tot, null);
                }
            }
            catch 
            {
            
            }
		}

		private void SaveDump(string cmdId, string args)
		{			
            if (this.StartInfo == null || args == null)
				return;


            Stream outs = new RenderDumpFilesInfo(this.StartInfo).Open(FileAccess.Write);
            if (outs != null)
            {                
                StreamWriter sw = new StreamWriter(outs, Encoding.UTF8);
                sw.Write(args);
                sw.Flush();
                sw.Close();
            }
		}

		private void SignalTestEnded(string cmdId, string args)
		{
			if(this.browserWrapperIEImpl != null)
				this.browserWrapperIEImpl.TestEnded(true, PageDataCollectorErrors.NoError);
		}
		private void SignalTestStarted(string cmdId, string args)
		{
			if (this.browserWrapperIEImpl != null)
				this.browserWrapperIEImpl.TestStarted();
		}

		#endregion

	}
}
