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
using System.Drawing;

namespace MySpace.MSFast.GUI.Engine.Panels.ValidationResults
{
	public partial class ViewSourceValidationOccurancePanel
	{

     
        private ToolStripButton helpBtn;
        private ToolStripButton nextBtn;
        private ToolStripButton prevBtn;
        private ToolStrip toolStrip;
        private RichTextBox sourceText;
        private Label commentText;

        private Panel topPanel;


		private void InitializeComponent()
		{
			this.nextBtn = new ToolStripButton();
			this.prevBtn = new ToolStripButton();
            this.helpBtn = new ToolStripButton();
			
            this.sourceText = new RichTextBox();
            this.commentText = new Label();
			
            this.topPanel = new Panel();


            toolStrip = new ToolStrip();
            toolStrip.SuspendLayout();

			this.SuspendLayout();

            toolStrip.Items.AddRange(new ToolStripItem[] { this.helpBtn, this.prevBtn, this.nextBtn });
            toolStrip.Anchor = System.Windows.Forms.AnchorStyles.Right;
            toolStrip.BackColor = System.Drawing.SystemColors.Info;
            toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            toolStrip.Location = new System.Drawing.Point(501, 9);
            toolStrip.Size = new System.Drawing.Size(70, 23);
            toolStrip.Location = new System.Drawing.Point(0, 0);
            toolStrip.Name = "toolStrip1";
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            
			// 
			// startCollectingDataBtn
			// 
			this.helpBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.helpBtn.Image = MSFast.GUI.Engine.Resources.Resources.help;
			this.helpBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpBtn.Name = "helpBtn";
			this.helpBtn.Size = new System.Drawing.Size(23, 22);
			this.helpBtn.Text = "More Details";
			this.helpBtn.Alignment = ToolStripItemAlignment.Left;
			// 
			// startCollectingDataBtn
			// 
			this.nextBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.nextBtn.Image = MSFast.GUI.Engine.Resources.Resources.next;
			this.nextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.nextBtn.Name = "nextBtn";
			this.nextBtn.Size = new System.Drawing.Size(23, 22);
			this.nextBtn.Text = "Next Occurance";
            this.nextBtn.Alignment = ToolStripItemAlignment.Left;
			// 
			// startCollectingDataBtn
			// 
			this.prevBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.prevBtn.Image = MSFast.GUI.Engine.Resources.Resources.prev;
			this.prevBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.prevBtn.Name = "prevBtn";
			this.prevBtn.Size = new System.Drawing.Size(23, 22);
			this.prevBtn.Text = "Previous Occurance";
            this.prevBtn.Alignment = ToolStripItemAlignment.Left;
			
			// 
			// startCollectingDataBtn
			// 
            this.commentText.Name = "commentText";
            this.commentText.Text = "";
            this.commentText.Visible = false;
            this.commentText.Height = 58;
            this.commentText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.commentText.BackColor = System.Drawing.SystemColors.Info;
            this.commentText.Padding = new Padding(5, 5, 5, 5);
			// 
            // sourceText
			// 
            this.sourceText.Location = new System.Drawing.Point(66, 92);
            this.sourceText.Name = "sourceText";
            this.sourceText.Size = new System.Drawing.Size(100, 96);
            this.sourceText.TabIndex = 1;
            this.sourceText.Text = "";
            this.sourceText.ReadOnly = true;
            this.sourceText.BackColor = Color.White;
            this.sourceText.WordWrap = false;

            this.topPanel.BackColor = System.Drawing.SystemColors.Info;
            this.topPanel.Visible = true;
            this.topPanel.SuspendLayout();
            this.topPanel.Size = new System.Drawing.Size(479, 50);
            this.topPanel.Controls.Add(this.commentText);
            this.topPanel.Controls.Add(this.toolStrip);

            this.Controls.Add(this.sourceText);
            this.Controls.Add(this.topPanel);

            topPanel.ResumeLayout(false);
            topPanel.PerformLayout();

            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
			
            this.ResumeLayout(false);
			this.PerformLayout();

			this.nextBtn.Click += new EventHandler(nextBtn_Click);
			this.prevBtn.Click += new EventHandler(prevBtn_Click);
			this.helpBtn.Click += new EventHandler(helpBtn_Click);
		}

		void nextBtn_Click(object sender, EventArgs e)
		{
			NextOccurance();
		}

		void prevBtn_Click(object sender, EventArgs e)
		{
			PreviousOccurance();
		}
        
        void helpBtn_Click(object sender, EventArgs e)
        {
            OpenHelp();
        }
	}
}
