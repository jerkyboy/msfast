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
using MySpace.MSFast.GUI.Engine.Helpers;

namespace MySpace.MSFast.GUI.Engine.Panels
{
    public partial class UpdateAvailable : MSFastForm
    {
        private String get_url;

        public UpdateAvailable()
        {
            InitializeComponent();
            PopulateVersion();
        }

        private void OpenBrowser()
        {
            this.Close();
            System.Diagnostics.Process.Start("http://" + get_url);
        }

        private void PopulateVersion()
        {
            this.label1.Text = "";
            this.txtDesc.Text = "";

            UpdateHelper.GetLatestVersionDetails(new UpdateHelper.GetLatestVersionDetailsCallback(this.LatestVersionDetailsCallback));
        }

        private void LatestVersionDetailsCallback(String ver, String desc, String get_url, DateTime versionDate)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new UpdateHelper.GetLatestVersionDetailsCallback(this.LatestVersionDetailsCallback), new object[] { ver, desc, get_url, versionDate });
                return;
            }
            this.get_url = get_url;
            this.txtDesc.Text = desc;
            this.label1.Text = String.Format("Version {0} (1) of MySpace.com Performance Tracker is Available!", ver, versionDate);
        }
    }
}
