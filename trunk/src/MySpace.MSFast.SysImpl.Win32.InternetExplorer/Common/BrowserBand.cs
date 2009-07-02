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
using System.Drawing;
using SHDocVw;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using MySpace.MSFast.SysImpl.Win32.ComIterop;


namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer.Common
{
	public abstract class BrowserBand : BrowserLinkObject, IDeskBand, IDockingWindow, IOleWindow, IInputObject
    {
        /// <summary>
        /// Reference to the host.
        /// </summary>
	    protected IInputObjectSite m_pIInputObjectSite;

		#region BrowserBandImpl

        /// <summary>
        /// constructor
        /// </summary>
        public BrowserBand()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the instance is created
        /// </summary>
        public abstract void InitializeComponent();

	    /// <summary>
	    /// Should return the main toolbar control
	    /// </summary>
        public abstract Control ToolbarControl { get; }

        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected void OnGotFocus(System.EventArgs e)
        {
            if (m_pIInputObjectSite != null)
                m_pIInputObjectSite.OnFocusChangeIS(this as IInputObject, 1);
        }
        /// <summary>
        /// Notifies explorer of focus change.
        /// </summary>
        protected void OnLostFocus(System.EventArgs e)
        {
            if (m_pIInputObjectSite != null)
                m_pIInputObjectSite.OnFocusChangeIS(this as IInputObject, 0);
        }
        #endregion

        #region BHOObject
        public override void SetSite(Object pUnkSite)
        {
    		SetSiteResults doOnSetSite = base.SetSiteWithStatus(pUnkSite);

            if (doOnSetSite != SetSiteResults.Success)
            {
                return;
            }

            if (m_pIInputObjectSite != null)
                Release();

            if (pUnkSite == null)
                return;

            m_pIInputObjectSite = pUnkSite as IInputObjectSite;
        }
        public override void Release()
        {
            base.Release();

            if (m_pIInputObjectSite != null)
            {
                Marshal.ReleaseComObject(m_pIInputObjectSite);
                m_pIInputObjectSite = null;
            }
        }
        #endregion

        #region IDeskBand
        public virtual void GetBandInfo(UInt32 dwBandID,UInt32 dwViewMode,ref DESKBANDINFO dbi)
        {
            dwBandID = 1;

            if ((dbi.dwMask & DBIM.MINSIZE) != 0)
            {
                dbi.ptMinSize.X = this.ToolbarControl.MinimumSize.Width;
                dbi.ptMinSize.Y = this.ToolbarControl.MinimumSize.Height;
            }

            if ((dbi.dwMask & DBIM.MAXSIZE) != 0)
            {
                dbi.ptMaxSize.X = this.ToolbarControl.MaximumSize.Width;
                dbi.ptMaxSize.Y = this.ToolbarControl.MaximumSize.Height;
            }

            if ((dbi.dwMask & DBIM.INTEGRAL) != 0)
            {
                dbi.ptIntegral.X = 10;
                dbi.ptIntegral.Y = 10;
            }

            if ((dbi.dwMask & DBIM.ACTUAL) != 0)
            {
                dbi.ptActual.X = this.ToolbarControl.Size.Width;
                dbi.ptActual.Y = this.ToolbarControl.Size.Height;
            }

            if ((dbi.dwMask & DBIM.TITLE) != 0)
            {
                dbi.wszTitle = this.ToolbarControl.Name;
            }

            dbi.dwModeFlags = DBIMF.VARIABLEHEIGHT | DBIMF.NORMAL | DBIMF.ADDTOFRONT | DBIMF.USECHEVRON;
        }
        #endregion

        #region IDockingWindow

        public virtual void GetWindow(out System.IntPtr phwnd)
        {
            if (ToolbarControl == null)
                phwnd = IntPtr.Zero;

            phwnd = this.ToolbarControl.Handle;
        }
        
        public virtual void ContextSensitiveHelp(bool fEnterMode) { }
        
        /// <summary>
        /// Called by explorer when band object needs to be showed or hidden.
        /// </summary>
        /// <param name="fShow"></param>
        public virtual void ShowDW(bool fShow)
        {
            if(ToolbarControl == null)
                return;

            if (fShow)
                ToolbarControl.Show();
            else
                this.ToolbarControl.Hide();
        }

        /// <summary>
        /// Called by explorer when window is about to close.
        /// </summary>
        public virtual void CloseDW(UInt32 dwReserved)
        {
            if (ToolbarControl == null)
                return;

            this.ToolbarControl.Dispose();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public virtual void ResizeBorderDW(IntPtr prcBorder, Object punkToolbarSite, bool fReserved) {}

        #endregion

        #region IInputObject
        /// <summary>
        /// Called explorer when focus has to be chenged.
        /// </summary>
        public virtual void UIActivateIO(Int32 fActivate, ref MSG Msg)
        {
            if (fActivate != 0 && ToolbarControl != null)
            {
                this.ToolbarControl.Focus();
            }
        }

        public virtual Int32 HasFocusIO()
        {
            if (ToolbarControl == null)
                return 0;

            return ((this.ToolbarControl.Focused) ? 0 : 1); //S_OK : S_FALSE;
        }

        /// <summary>
        /// Called by explorer to process keyboard events. Undersatands Tab and F6.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>S_OK if message was processed, S_FALSE otherwise.</returns>
        public virtual Int32 TranslateAcceleratorIO(ref MSG msg)
        {
            return 1;//S_FALSE
        }
        #endregion

    }
}
