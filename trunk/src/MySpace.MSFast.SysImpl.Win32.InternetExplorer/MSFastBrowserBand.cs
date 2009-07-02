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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using mshtml;
using System.Runtime.InteropServices.ComTypes;
using SHDocVw;
using Microsoft.Win32;
using System.Reflection;
using MySpace.MSFast.SysImpl.Win32.Utils;
using MySpace.MSFast.Engine.BrowserWrapper;
using MySpace.MSFast.GUI.Engine.Panels;
using MySpace.MSFast.SysImpl.Win32;
using MySpace.MSFast.SysImpl.Win32.InternetExplorer.Common;

namespace MySpace.MSFast.SysImpl.Win32.InternetExplorer
{

    [ClassInterfaceAttribute(ClassInterfaceType.None)]
	[ComVisible(true), GuidAttribute("AAE91B90-296A-471e-9926-2D4505F8EF5B")] // MY CLASS GUID
    [ProgIdAttribute("MySpace.MSFast")]
	 public class MSFastBrowserBand : BrowserBand
    {
        
        private Panel toolbarControl = null;

        public override void InitializeComponent(){
            toolbarControl = new TBWrapper(this);
            toolbarControl.Dock = DockStyle.Left;
			toolbarControl.Name = "MySpace's Performance Tracker";
            toolbarControl.MinimumSize = new System.Drawing.Size(150, 230);
            toolbarControl.MaximumSize = new System.Drawing.Size(0, 850);
        }
        
		  public override Control ToolbarControl { get {return toolbarControl;} }

          private class TBWrapper : Panel 
          {
              private Panel tb;
              private IntPtr horizHwnd = IntPtr.Zero;

              public TBWrapper(Browser br)
              {
                  tb = new MSFastMainPanel(br);
                  this.Controls.Add(tb);
              }
              protected override void OnSizeChanged(EventArgs e)
              {
                  if (horizHwnd == IntPtr.Zero)
                      horizHwnd = GetHorizHwnd(this.Handle);

                  int w = this.Width;

                  if (horizHwnd != IntPtr.Zero)
                  {
                      RECT baseBarRect;
                      RECT thisRect;

                      Win32API.GetWindowRect(horizHwnd, out baseBarRect);
                      Win32API.GetWindowRect(this.Handle, out thisRect);
                      w = (baseBarRect.Right - thisRect.Left);
                  }

                  this.tb.Left = 0;
                  this.tb.Top = 0;
                  this.tb.Width = w;
                  this.tb.Height = this.Height;

              }

              private IntPtr GetHorizHwnd(IntPtr res)
              {
                  StringBuilder className = null;
                  do
                  {
                      res = Win32API.GetParent(res);
                      if (res != IntPtr.Zero)
                      {
                          className = new StringBuilder(100);

                          if (Win32API.GetClassName(res, className, className.Capacity) != 0)
                          {
                              if (className.ToString().Trim().StartsWith("BaseBar"))
                              {
                                  return res;
                              }
                          }
                      }

                  } while (res != IntPtr.Zero);

                  return res;
              }
          }
	 }
}