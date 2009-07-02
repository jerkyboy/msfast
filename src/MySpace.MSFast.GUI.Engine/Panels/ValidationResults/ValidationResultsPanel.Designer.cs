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

namespace MySpace.MSFast.GUI.Engine.Panels.ValidationResults
{
	public partial class ValidationResultsPanel
	{
		private ResultsList ls_content = null;

		private ViewSourceValidationOccurancePanel viewSourceDataPanel;
        private ViewDownloadStateOccurancePanel viewDownloadDataPanel;
        private ViewStringOccurancePanel viewStringPanel;
        private Panel viewResultsEmptyPanel;
        private Label validatorDescritionText = null;
        private LeftPanel leftPanel;
        private Control topGroupsButtonsPanel;
        private ComboBox selectGroup;

		private void InitializeComponent()
		{
            topGroupsButtonsPanel = new Panel();
            selectGroup = new ComboBox();

			ls_content = new ResultsList();
            validatorDescritionText = new Label();
            SplitContainer splitContainer1 = new SplitContainer();

            leftPanel = new LeftPanel(ls_content, validatorDescritionText, topGroupsButtonsPanel);

			viewSourceDataPanel = new ViewSourceValidationOccurancePanel ();
            viewDownloadDataPanel = new ViewDownloadStateOccurancePanel();
            viewStringPanel = new ViewStringOccurancePanel();
			viewResultsEmptyPanel = new Panel();
            
            selectGroup.SuspendLayout();
			this.SuspendLayout();
			ls_content.SuspendLayout();
            topGroupsButtonsPanel.SuspendLayout();
            leftPanel.SuspendLayout();
            splitContainer1.SuspendLayout();

			viewSourceDataPanel.SuspendLayout();
			viewDownloadDataPanel.SuspendLayout();
            viewStringPanel.SuspendLayout();
			viewResultsEmptyPanel.SuspendLayout();

			#region viewSourceDataPanel
			this.viewSourceDataPanel.Dock = DockStyle.Fill;
			this.viewSourceDataPanel.Location = new System.Drawing.Point(0, 0);
			this.viewSourceDataPanel.Size = new System.Drawing.Size(196, 183);
			this.viewSourceDataPanel.TabIndex = 0;
			this.viewSourceDataPanel.Visible = false;
			#endregion
			
			#region viewDownloadDataPanel
			this.viewDownloadDataPanel.Dock = DockStyle.Fill;
			this.viewDownloadDataPanel.Location = new System.Drawing.Point(0, 0);
			this.viewDownloadDataPanel.Size = new System.Drawing.Size(196, 183);
			this.viewDownloadDataPanel.TabIndex = 0;
			this.viewSourceDataPanel.Visible = false;
			#endregion

            #region viewStringPanel
            this.viewStringPanel.Dock = DockStyle.Fill;
            this.viewStringPanel.Location = new System.Drawing.Point(0, 0);
            this.viewStringPanel.Size = new System.Drawing.Size(196, 183);
            this.viewStringPanel.TabIndex = 0;
            this.viewStringPanel.Visible = false;
            #endregion

            #region viewResultsEmptyPanel
            this.viewResultsEmptyPanel.Dock = DockStyle.Fill;
			this.viewResultsEmptyPanel.Location = new System.Drawing.Point(0, 0);
			this.viewResultsEmptyPanel.Size = new System.Drawing.Size(196, 183);
			this.viewResultsEmptyPanel.TabIndex = 0;
            this.viewResultsEmptyPanel.BackColor = Color.White;
			this.viewResultsEmptyPanel.Visible = true;
            this.viewResultsEmptyPanel.BorderStyle = BorderStyle.Fixed3D;

            Label label = new Label();
            //label.AutoSize = true;
            label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(231)))), ((int)(((byte)(240)))));
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Name = "label1";
            label.TabIndex = 0;
            label.Text = "(No Information)";
            this.viewResultsEmptyPanel.Controls.Add(label);
			#endregion
			
			#region Content Panel
            this.ls_content.Dock = DockStyle.None;
			this.ls_content.ResultsSelected += new ResultsList.OnResultsSelected(ResultsSelected);
			this.ls_content.Location = new System.Drawing.Point(0, 0);
			this.ls_content.TabIndex = 0;
			this.ls_content.Initialize();
			#endregion

			


            this.validatorDescritionText.Dock = DockStyle.None;
            this.validatorDescritionText.Padding = new Padding(5, 5, 5, 5);
            this.validatorDescritionText.BackColor = System.Drawing.SystemColors.Info;
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.Controls.Add(ls_content);
            leftPanel.Controls.Add(validatorDescritionText);
            leftPanel.Controls.Add(topGroupsButtonsPanel);

            Label t = new Label();
            t.Text = "Select Validation Ruleset :";
            t.Top = 10;
            t.BackColor = Color.White;
            t.Left = 5;
            t.AutoSize = true;

            topGroupsButtonsPanel.BackColor = Color.White;
            topGroupsButtonsPanel.Dock = DockStyle.None;
            topGroupsButtonsPanel.Controls.Add(t);
            topGroupsButtonsPanel.Controls.Add(selectGroup);
            topGroupsButtonsPanel.Height = selectGroup.Height+10;
            topGroupsButtonsPanel.Width = 300;

            selectGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            selectGroup.Width = 120;
            selectGroup.Location = new Point(t.Width + 10, 5);
            selectGroup.SelectedIndexChanged += new EventHandler(selectGroup_SelectedIndexChanged);
            selectGroup.DropDownStyle = ComboBoxStyle.DropDownList;
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Orientation = Orientation.Vertical;
            splitContainer1.Panel1.Controls.Add(leftPanel);
			splitContainer1.Panel2.Controls.Add(viewResultsEmptyPanel);
			splitContainer1.Panel2.Controls.Add(viewSourceDataPanel);
			splitContainer1.Panel2.Controls.Add(viewDownloadDataPanel);
            splitContainer1.Panel2.Controls.Add(viewStringPanel);
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Size = new Size(600,200);
            splitContainer1.SplitterDistance = 300;

            this.Controls.Add(splitContainer1);

			this.viewResultsEmptyPanel.ResumeLayout(false);
			this.viewResultsEmptyPanel.PerformLayout();

			this.viewSourceDataPanel.ResumeLayout(false);
			this.viewSourceDataPanel.PerformLayout();

            this.viewDownloadDataPanel.ResumeLayout(false);
			this.viewDownloadDataPanel.PerformLayout();

            this.viewStringPanel.ResumeLayout(false);
            this.viewStringPanel.PerformLayout();

			this.ls_content.ResumeLayout(false);
			this.ls_content.PerformLayout();

			splitContainer1.ResumeLayout();
			splitContainer1.PerformLayout();

            leftPanel.ResumeLayout();
            leftPanel.PerformLayout();
            
            topGroupsButtonsPanel.ResumeLayout();
            topGroupsButtonsPanel.PerformLayout();

            selectGroup.ResumeLayout();
            selectGroup.PerformLayout();
            
            this.ResumeLayout();

		}

        void selectGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowResults();
        }

        private void SetValidatorDescription(string p)
        {
            this.validatorDescritionText.Text = p;
            this.leftPanel.RevalidateSize();
        }


        private class LeftPanel : Panel
        {
            private ResultsList ls_content = null;
            private Label validatorDescritionText = null;
            private Control topGroupsButtonsPanel = null;

            public LeftPanel(ResultsList ls_content, Label validatorDescritionText, Control topGroupsButtonsPanel)
            {
                this.ls_content = ls_content;
                this.topGroupsButtonsPanel = topGroupsButtonsPanel;
                this.validatorDescritionText = validatorDescritionText;
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                RevalidateSize();
            }

            protected override void OnResize(EventArgs eventargs)
            {
                base.OnResize(eventargs);
                RevalidateSize();
            }

            public void RevalidateSize()
            {
                if (String.IsNullOrEmpty(validatorDescritionText.Text))
                {
                    validatorDescritionText.Height = 0;
                    validatorDescritionText.Visible = false;
                }
                else
                {
                    validatorDescritionText.Visible = true;
                    using (System.Drawing.Graphics g = validatorDescritionText.CreateGraphics())
                    {
                        int w = (int)g.MeasureString(validatorDescritionText.Text, validatorDescritionText.Font).Width + 10;
                        validatorDescritionText.Height = ((int)Math.Max(1, Math.Ceiling(((double)w) / ((double)this.Width))) * 15) + 10;
                    }
                }

                topGroupsButtonsPanel.Top = 0;
                topGroupsButtonsPanel.Width = this.Width;
                topGroupsButtonsPanel.Left = 0;

                ls_content.Top = topGroupsButtonsPanel.Height;
                ls_content.Height = this.Height - validatorDescritionText.Height - topGroupsButtonsPanel.Height;
                ls_content.Width = this.Width;
                ls_content.Left = 0;

                validatorDescritionText.Left = 0;
                validatorDescritionText.Width = this.Width;
                validatorDescritionText.Top = ls_content.Height + ls_content.Top;
            }
        }

	}
}
