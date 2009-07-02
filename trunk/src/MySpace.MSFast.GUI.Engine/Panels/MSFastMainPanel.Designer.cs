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
using MySpace.MSFast.GUI.Engine.Panels.GraphView;
using MySpace.MSFast.GUI.Engine.Panels.ValidationResults;
using MySpace.MSFast.GUI.Engine.Panels.Status;

namespace MySpace.MSFast.GUI.Engine.Panels
{
	public partial class MSFastMainPanel
	{


		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.startCollectingDataBtn = new System.Windows.Forms.ToolStripButton();
			this.abortCollectingDataBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.toolStripSeparator5 = new ToolStripSeparator();
            this.pageGraphBtn = new System.Windows.Forms.ToolStripButton();
			this.validationResultsBtn = new System.Windows.Forms.ToolStripButton();
			this.configCollectingDataBtn = new System.Windows.Forms.ToolStripButton();
            this.aboutUsDataBtn = new System.Windows.Forms.ToolStripButton();
            this.updatesReadyBtn = new System.Windows.Forms.ToolStripButton();


			this.toolStrip1.SuspendLayout();

			this.testStatusPanel = new TestStatutsPanel();
			this.graphViewPanel = new PageGraphPanel();
			this.validationResultsViewPanel = new ValidationResultsPanel();

			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startCollectingDataBtn,
            this.abortCollectingDataBtn,
            this.toolStripSeparator1,
            this.pageGraphBtn,
            this.toolStripSeparator2,
            this.validationResultsBtn,
            this.toolStripSeparator3,
            this.aboutUsDataBtn,
            this.toolStripSeparator4,
            this.configCollectingDataBtn,
            this.toolStripSeparator5,
            this.updatesReadyBtn});

            this.toolStrip1.Dock = DockStyle.Top;
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(695, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// testStatusPanel
			// 
			this.testStatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testStatusPanel.Location = new System.Drawing.Point(12, 44);
			this.testStatusPanel.Name = "testStatusPanel";
			this.testStatusPanel.Size = new System.Drawing.Size(248, 189);
			this.testStatusPanel.TabIndex = 2;
			this.testStatusPanel.Visible = false;
			// 
			// graphViewPanel
			// 
			this.graphViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.graphViewPanel.Location = new System.Drawing.Point(12, 44);
			this.graphViewPanel.Name = "graphViewPanel";
			this.graphViewPanel.Size = new System.Drawing.Size(248, 189);
			this.graphViewPanel.TabIndex = 2;
			this.graphViewPanel.Visible = false;
			// 
			// validationResultsViewPanel
			// 
			this.validationResultsViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.validationResultsViewPanel.Location = new System.Drawing.Point(12, 44);
			this.validationResultsViewPanel.Name = "validationResultsViewPanel";
			this.validationResultsViewPanel.Size = new System.Drawing.Size(248, 189);
			this.validationResultsViewPanel.TabIndex = 2;
			this.validationResultsViewPanel.Visible = false;
			// 
			// startCollectingDataBtn
			// 
			this.startCollectingDataBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.startCollectingDataBtn.Image = MSFast.GUI.Engine.Resources.Resources.start;
			this.startCollectingDataBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.startCollectingDataBtn.Name = "startCollectingDataBtn";
			this.startCollectingDataBtn.Size = new System.Drawing.Size(23, 22);
			this.startCollectingDataBtn.Text = "Start Test";
			// 
			// abortCollectingDataBtn
			// 
			this.abortCollectingDataBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.abortCollectingDataBtn.Image = MSFast.GUI.Engine.Resources.Resources.stop;
			this.abortCollectingDataBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.abortCollectingDataBtn.Name = "abortCollectingDataBtn";
			this.abortCollectingDataBtn.Size = new System.Drawing.Size(23, 22);
			this.abortCollectingDataBtn.Text = "Stop Test";
            // 
			// pageGraphBtn
			// 
			this.pageGraphBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.pageGraphBtn.Name = "pageGraphBtn";
			this.pageGraphBtn.Size = new System.Drawing.Size(72, 22);
			this.pageGraphBtn.Text = "Page Graph";
			// 
			// validationResultsBtn
			// 
			this.validationResultsBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.validationResultsBtn.Name = "validationResultsBtn";
			this.validationResultsBtn.Size = new System.Drawing.Size(104, 22);
			this.validationResultsBtn.Text = "Validation Results";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator3";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Alignment = ToolStripItemAlignment.Right;
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator3";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator5.Alignment = ToolStripItemAlignment.Right;
            // 
			// configCollectingDataBtn
			// 
			this.configCollectingDataBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.configCollectingDataBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.configCollectingDataBtn.Image = MSFast.GUI.Engine.Resources.Resources.settings;
			this.configCollectingDataBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.configCollectingDataBtn.Name = "configCollectingDataBtn";
			this.configCollectingDataBtn.Size = new System.Drawing.Size(23, 22);
			this.configCollectingDataBtn.Text = "Properties";
            //
            // aboutUsDataBtn
            //
            this.aboutUsDataBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutUsDataBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutUsDataBtn.Image = MSFast.GUI.Engine.Resources.Resources.help;
            this.aboutUsDataBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutUsDataBtn.Name = "aboutUsDataBtn";
            this.aboutUsDataBtn.Size = new System.Drawing.Size(23, 22);
            this.aboutUsDataBtn.Text = "About Us";
            //
            // updatesReadyBtn
            //
            this.updatesReadyBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.updatesReadyBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.ImageAndText;
            this.updatesReadyBtn.Image = MSFast.GUI.Engine.Resources.Resources.newVersion;
            this.updatesReadyBtn.BackgroundImage = MSFast.GUI.Engine.Resources.Resources.newVersionBG;
            this.updatesReadyBtn.BackgroundImageLayout = ImageLayout.Stretch;
            this.updatesReadyBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.updatesReadyBtn.Name = "updatesReadyBtn";
            this.updatesReadyBtn.Text = "A New Version is Available!";
			

            this.ClientSize = new System.Drawing.Size(284, 264);

            Panel wrapper = new Panel();
            wrapper.Dock = DockStyle.Fill;

            wrapper.Controls.Add(this.testStatusPanel);
            wrapper.Controls.Add(this.graphViewPanel);
            wrapper.Controls.Add(this.validationResultsViewPanel);
            this.Controls.Add(wrapper);
            this.Controls.Add(this.toolStrip1);
 
			this.Name = "MSFastMainPanel";
			this.Text = "MSFastMainPanel";

			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

			this.configCollectingDataBtn.Click += new EventHandler(configCollectingDataBtn_Click);
            this.aboutUsDataBtn.Click += new EventHandler(aboutUsDataBtn_Click);
            this.updatesReadyBtn.Click += new EventHandler(updatesReadyBtn_Click);
			this.abortCollectingDataBtn.Click += new EventHandler(abortCollectingDataBtn_Click);
			this.startCollectingDataBtn.Click += new EventHandler(startCollectingDataBtn_Click);
			this.pageGraphBtn.Click += new EventHandler(pageGraphBtn_Click);
			this.validationResultsBtn.Click += new EventHandler(validationResultsBtn_Click);
		}

        void updatesReadyBtn_Click(object sender, EventArgs e)
        {
            OpenNewVersionAvailable();
        }

        void aboutUsDataBtn_Click(object sender, EventArgs e)
        {
            OpenAboutUs();   
        }

		void validationResultsBtn_Click(object sender, EventArgs e)
		{
			ShowValidationResultsPanel();
		}

		void pageGraphBtn_Click(object sender, EventArgs e)
		{
			ShowGraphPanel();
		}

		void startCollectingDataBtn_Click(object sender, EventArgs e)
		{
			StartTest();
		}

		void configCollectingDataBtn_Click(object sender, EventArgs e)
		{
			OpenConfigWindow();
		}

		void abortCollectingDataBtn_Click(object sender, EventArgs e)
		{
			AbortTest();
		}


		#endregion


		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton startCollectingDataBtn;
		private System.Windows.Forms.ToolStripButton abortCollectingDataBtn;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton pageGraphBtn;
        private System.Windows.Forms.ToolStripButton validationResultsBtn;
        private System.Windows.Forms.ToolStripButton configCollectingDataBtn;
        private System.Windows.Forms.ToolStripButton aboutUsDataBtn;
        private System.Windows.Forms.ToolStripButton updatesReadyBtn;

		private TestStatutsPanel testStatusPanel;
		private PageGraphPanel graphViewPanel;
		private ValidationResultsPanel validationResultsViewPanel;

	}
}
