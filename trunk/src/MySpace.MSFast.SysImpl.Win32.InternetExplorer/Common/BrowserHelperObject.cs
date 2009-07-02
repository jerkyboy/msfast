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
using System.Windows.Forms;
using SHDocVw;
using mshtml;
using System.Runtime.InteropServices.ComTypes;
using System.IO;
using MySpace.MSFast.SysImpl.Win32.ComIterop;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.Common
{
    /// <summary>
    /// This class is a simple implementation of Internet Explorer BHO
    /// </summary>
	public abstract class BrowserHelperObject : BrowserLinkObject, IDocHostUIHandler
	{
		protected IDocHostUIHandler m_defaultUIHandler;

		public override void SetSite(object pUnkSite)
		{
			if (m_defaultUIHandler != null){
				Release();
			}

			base.SetSiteWithStatus(pUnkSite);
		}

		public override void Release()
		{
			base.Release();

			if (m_defaultUIHandler != null)
			{
				Marshal.ReleaseComObject(m_defaultUIHandler);
				m_defaultUIHandler = null;
			}
		}

		private bool registeredUIHandler = false;
		public override void RegisterDoc() 
		{
			base.RegisterDoc();
			
			if (registeredUIHandler == false)
			{
				registeredUIHandler = true;

				ICustomDoc cDoc = this.m_pIWebBrowser2.Document as ICustomDoc;
				IOleObject oleObj = this.m_pIWebBrowser2.Document as IOleObject;

				if (oleObj != null && cDoc != null)
				{
					IOleClientSite clientSite = null;
					oleObj.GetClientSite(ref clientSite);

					if (m_defaultUIHandler == null && clientSite != null)
					{
						m_defaultUIHandler = clientSite as IDocHostUIHandler;
					}
					if (cDoc != null)
					{
						cDoc.SetUIHandler(this);
					}
				}
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


	}
}
