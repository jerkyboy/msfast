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
using System.Drawing;
using System.Windows.Forms;
using MySpace.MSFast.GUI.Engine.Panels.GraphView.Controls;

namespace MySpace.MSFast.GUI.Engine.Panels.GraphView
{
	public partial class PageGraphPanel
	{
		private FlashPlayer shockwaveFlash = null;

		private void InitializeComponent()
		{
			this.shockwaveFlash = new FlashPlayer();
			this.SuspendLayout();
			
			this.shockwaveFlash.Enabled = true;
			this.shockwaveFlash.Location = new System.Drawing.Point(27, 43);
			this.shockwaveFlash.Name = "shockwaveFlash";
			this.shockwaveFlash.Size = new System.Drawing.Size(192, 192);
			this.shockwaveFlash.Dock = DockStyle.Fill;
			this.shockwaveFlash.TabIndex = 0;
			this.shockwaveFlash.AllowScriptAccess = "always";
			this.shockwaveFlash.FlashCall += new FlashCallEventHandler(shockwaveFlash_FlashCall);

			this.Controls.Add(shockwaveFlash);
			this.ResumeLayout();
		}

		void  shockwaveFlash_FlashCall(object sender, FlashCallEvent e)
		{
            if (this.callsDelegate.ContainsKey(e.name))
            {
                this.callsDelegate[e.name](e);
            }
		}
	}
}
