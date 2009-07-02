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

namespace MySpace.MSFast.GUI.Engine.Panels.Status
{
	public partial class TestStatutsPanel
	{
        private Panel startTestPanel;
        private Panel workingPanel;
        
        private Label lblCurrentLabel;

        private TaskProgressLabel lblPreparingTest = new TaskProgressLabel("Preparing Test");
        private TaskProgressLabel lblRenderingSegment = new TaskProgressLabel("Rendering Segments");
        private TaskProgressLabel lblCaptureSegment = new TaskProgressLabel("Capturing Segments");
        private TaskProgressLabel lblProcessResults = new TaskProgressLabel("Processing Results");
        private DownloadingFilesList lstDownloadedFiles = new DownloadingFilesList();

		private void InitializeComponent()
        {
            //Start Test Panel
            
            this.startTestPanel = new StatusPanelBase();
            this.workingPanel = new Panel();

            this.lblCurrentLabel = new Label();

            this.SuspendLayout();
            this.startTestPanel.SuspendLayout();
            this.workingPanel.SuspendLayout();

            #region Start Test Panel
            this.startTestPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startTestPanel.Location = new System.Drawing.Point(12, 44);
            this.startTestPanel.Name = "startTestPanel";
            this.startTestPanel.Size = new System.Drawing.Size(248, 189);
            this.startTestPanel.TabIndex = 2;
            this.startTestPanel.Visible = true;

            PictureBox pb = new PictureBox();
            pb.Image = MSFast.GUI.Engine.Resources.Resources.startTest;
            pb.Size = MSFast.GUI.Engine.Resources.Resources.startTest.Size;
            this.startTestPanel.Controls.Add(pb);
            #endregion

            #region Current State Panel
            this.workingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workingPanel.Location = new System.Drawing.Point(12, 44);
            this.workingPanel.Name = "workingPanel";
            this.workingPanel.TabIndex = 2;
            this.workingPanel.Visible = false;

            this.lstDownloadedFiles.Dock = DockStyle.Fill;
            this.lstDownloadedFiles.BackColor = System.Drawing.Color.Transparent;

            this.lblCurrentLabel.Location = new System.Drawing.Point(35, 15);
            this.lblCurrentLabel.Height = 20;
            this.lblCurrentLabel.Name = "lblCurrentLabel";
            this.lblCurrentLabel.AutoSize = true;
            this.lblCurrentLabel.TabIndex = 2;
            this.lblCurrentLabel.BackColor = Color.White;
            this.lblCurrentLabel.ForeColor = Color.FromArgb(0x6e7088);
            this.lblCurrentLabel.Text = "............................................................................";

            this.lblPreparingTest.Location = new System.Drawing.Point(35, 35);
            this.lblRenderingSegment.Location = new System.Drawing.Point(35, 55);
            this.lblCaptureSegment.Location = new System.Drawing.Point(35, 75);
            this.lblProcessResults.Location = new System.Drawing.Point(35, 95);

            Panel left = new Panel();
            left.Dock = DockStyle.Top;
            left.BackColor = Color.White;
            left.Height = 115;
            left.Controls.Add(this.lblCurrentLabel);
            left.Controls.Add(this.lblPreparingTest);
            left.Controls.Add(this.lblRenderingSegment);
            left.Controls.Add(this.lblCaptureSegment);
            left.Controls.Add(this.lblProcessResults);

            this.workingPanel.Controls.Add(this.lstDownloadedFiles);
            this.workingPanel.Controls.Add(left);
            

            #endregion

            this.Controls.Add(this.startTestPanel);
            this.Controls.Add(this.workingPanel);

            this.Dock = DockStyle.Fill;
            
            this.workingPanel.ResumeLayout();
			this.ResumeLayout();
		}
	}
}
