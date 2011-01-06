//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
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
using System.IO;
using System.Reflection;
using MySpace.MSFast.GUI.Engine.Panels.GraphView.Controls;
using MySpace.MSFast.DataProcessors.PageSource;
using System.Text.RegularExpressions;
using MySpace.MSFast.DataProcessors;

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView
{
	public partial class PageGraphPanel : Panel
	{

        private Dictionary<String, HandleCall> callsDelegate;
        private delegate void HandleCall(FlashCallEvent e);

        private ProcessedDataPackage package;

        public PageGraphPanel()
		{
            this.callsDelegate = new Dictionary<string,HandleCall>();
            this.callsDelegate.Add("showSourcePopup", new HandleCall(this.ShowSourcePopup));//s+","+l
            this.callsDelegate.Add("showPairPopup", new HandleCall(this.ShowPairPopup));//txt + "," + urlA + "," + urlB
            this.callsDelegate.Add("hidePairPopup", new HandleCall(this.HidePairPopup));
            this.callsDelegate.Add("showThumbnailPopup", new HandleCall(this.ShowThumbnailPopup));//url
            this.callsDelegate.Add("hideThumbnailPopup", new HandleCall(this.HideThumbnailPopup));

            InitializeComponent();
		}

        internal void SetResults(string graphResults, ProcessedDataPackage package)
		{
            this.package = package;

			if(this.shockwaveFlash == null || graphResults == null || File.Exists(graphResults) == false)
			{
				if (this.shockwaveFlash != null)
				{
					this.shockwaveFlash.LoadMovie(0, "");
					this.shockwaveFlash.Visible = false;
				}
				return;
			}
			
			this.shockwaveFlash.LoadMovie(0, "");

            String location = "MySpace.MSFast.GUI.Engine.RenderGraph.bin";
			location = (Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastMainPanel)).Location).Replace("\\", "/") + "/" + location);

			if (File.Exists(location) == false)
			{
				this.shockwaveFlash.Visible = false;
				return;
			}

			location += "?a=" + (new Random().Next());

			this.shockwaveFlash.Visible = true;
			this.shockwaveFlash.FlashVars = "";
			this.shockwaveFlash.FlashVars = "e=1&u=" + graphResults;
			this.shockwaveFlash.LoadMovie(0, location);
		}

        private Regex requestParserShowSourcePopup = new Regex("[0-9]*,[0-9]*,([0-9]*)", RegexOptions.Compiled);
        private object showSourcePopupLock = new object();
        private void ShowSourcePopup(FlashCallEvent e)//s+","+l
        {
            if (package == null || package.ContainsKey(typeof(BrokenSourceData)) == false)
                return;

            Match m = requestParserShowSourcePopup.Match(e.request);
            if (m.Success == false || m.Groups.Count != 2)
                return;

            lock (showSourcePopupLock)
            {
                PageSourceViewForm form = null;

                foreach (Form opnFrm in Application.OpenForms)
                {
                    if (opnFrm is PageSourceViewForm)
                    {
                        form = (PageSourceViewForm)opnFrm;
                        break;
                    }
                }

                if (form == null)
                {
                    form = new PageSourceViewForm();
                    form.Show();
                }

                form.SetResults(package, int.Parse(m.Groups[1].Value));
                form.Focus();
            }
        }

        private void ShowPairPopup(FlashCallEvent e)//txt + "," + urlA + "," + urlB
        {
        }

        private void HidePairPopup(FlashCallEvent e)
        {
        }

        private void ShowThumbnailPopup(FlashCallEvent e)//url
        {
        }

        private void HideThumbnailPopup(FlashCallEvent e)
        {
        }
    }
}
