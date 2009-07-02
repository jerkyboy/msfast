//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Configuration)
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

namespace MySpace.MSFast.GUI.Configuration.MSFast
{
	public partial class MSFastGlobalConfigPanel
	{
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();

            this.btnBrowseDump = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCPIPageValidation = new System.Windows.Forms.CheckBox();
            this.chkCPIPageGraph = new System.Windows.Forms.CheckBox();
			this.chkCPClearCache = new System.Windows.Forms.CheckBox();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
			this.txtCPProxyPort = new System.Windows.Forms.TextBox();
			this.txtCPDumpFolder = new System.Windows.Forms.TextBox();
            this.numDumpSize = new NumericUpDown();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnRes = new System.Windows.Forms.Button();
			this.btnResDef = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 3;
			// 
			// groupBox1
			// 
            this.groupBox1.Controls.Add(this.chkCPIPageValidation);
            this.groupBox1.Controls.Add(this.chkCPIPageGraph);
            this.groupBox1.Controls.Add(this.chkCPClearCache);

			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 90);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Capture Preferences";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnBrowseDump);
            this.groupBox2.Controls.Add(this.numDumpSize);
            this.groupBox2.Controls.Add(this.txtCPDumpFolder);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);

            this.groupBox2.Location = new System.Drawing.Point(12, 112);
            this.groupBox2.Name = "groupBox3";
            this.groupBox2.Size = new System.Drawing.Size(365, 85);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Temp Folder Preferences";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCPProxyPort);
            this.groupBox3.Controls.Add(this.label2);

            this.groupBox3.Location = new System.Drawing.Point(12, 206);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(365, 50);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Internal Proxy Preferences";

            // 
            // chkCPIPageValidation
			// 
			this.chkCPIPageValidation.AutoSize = true;
            this.chkCPIPageValidation.Location = new System.Drawing.Point(6, 22);
            this.chkCPIPageValidation.Name = "chkCPIPageValidation";
            this.chkCPIPageValidation.Size = new System.Drawing.Size(121, 17);
            this.chkCPIPageValidation.TabIndex = 11;
            this.chkCPIPageValidation.Text = "Page Validation";
            this.chkCPIPageValidation.UseVisualStyleBackColor = true;
			// 
            // chkCPIPageGraph
			// 
            this.chkCPIPageGraph.AutoSize = true;
            this.chkCPIPageGraph.Location = new System.Drawing.Point(6, 45);
            this.chkCPIPageGraph.Name = "chkCPIPageGraph";
            this.chkCPIPageGraph.Size = new System.Drawing.Size(100, 17);
            this.chkCPIPageGraph.TabIndex = 9;
            this.chkCPIPageGraph.Text = "Page Graph";
            this.chkCPIPageGraph.UseVisualStyleBackColor = true;
            // 
            // chkCPClearCache
            // 
            this.chkCPClearCache.AutoSize = true;
            this.chkCPClearCache.Location = new System.Drawing.Point(6, 68);
            this.chkCPClearCache.Name = "chkCPClearCache";
            this.chkCPClearCache.Size = new System.Drawing.Size(163, 17);
            this.chkCPClearCache.TabIndex = 0;
            this.chkCPClearCache.Text = "Clear browser cache before each test";
            this.chkCPClearCache.UseVisualStyleBackColor = true;
            // 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 23);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Run proxy on port #";
			// 
			// txtCPProxyPort
			// 
			this.txtCPProxyPort.Location = new System.Drawing.Point(143, 20);
			this.txtCPProxyPort.Name = "txtCPProxyPort";
			this.txtCPProxyPort.Size = new System.Drawing.Size(100, 20);
			this.txtCPProxyPort.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Temp Folder";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Disk space to use (16 - 1024MB)";
            // 
			// txtCPDumpFolder
			// 
			this.txtCPDumpFolder.Location = new System.Drawing.Point(78, 48);
			this.txtCPDumpFolder.Name = "txtCPDumpFolder";
			this.txtCPDumpFolder.Size = new System.Drawing.Size(162, 20);
			this.txtCPDumpFolder.TabIndex = 4;
			// 
            // numDumpSize
			// 
            this.numDumpSize.Location = new System.Drawing.Point(185, 20);
            this.numDumpSize.Name = "numDumpSize";
            this.numDumpSize.Size = new System.Drawing.Size(55, 20);
            this.numDumpSize.TabIndex = 5;
            this.numDumpSize.Minimum = 16;
            this.numDumpSize.Maximum = 1024;
            // 
            // btnBrowseDump
            // 
            this.btnBrowseDump.Location = new System.Drawing.Point(245, 46);
            this.btnBrowseDump.Name = "btnBrowseDump";
            this.btnBrowseDump.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseDump.TabIndex = 10;
            this.btnBrowseDump.Text = "Browse";
            this.btnBrowseDump.UseVisualStyleBackColor = true;

            
			
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(300, 265);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 10;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			
			// 
			// btnRes
			// 
            this.btnRes.Location = new System.Drawing.Point(220, 265);
			this.btnRes.Name = "btnRes";
			this.btnRes.Size = new System.Drawing.Size(75, 23);
			this.btnRes.TabIndex = 11;
			this.btnRes.Text = "Reset";
			this.btnRes.UseVisualStyleBackColor = true;
			
			// 
			// btnResDef
			// 
            this.btnResDef.Location = new System.Drawing.Point(12, 265);
			this.btnResDef.Name = "btnResDef";
			this.btnResDef.Size = new System.Drawing.Size(100, 23);
			this.btnResDef.TabIndex = 12;
			this.btnResDef.Text = "Restore Default";
			this.btnResDef.UseVisualStyleBackColor = true;


			this.btnSave.Click += new EventHandler(btnSave_Click);
			this.btnResDef.Click += new EventHandler(btnResDef_Click);
			this.btnRes.Click += new EventHandler(btnRes_Click);
            this.btnBrowseDump.Click += new EventHandler(btnBrowseDump_Click);
			this.txtCPProxyPort.TextChanged += new EventHandler(_FormChanged);
            this.numDumpSize.ValueChanged += new EventHandler(_FormChanged);
            this.txtCPDumpFolder.TextChanged += new EventHandler(_FormChanged);
			
			this.chkCPClearCache.CheckedChanged += new EventHandler(_FormChanged);
            this.chkCPIPageValidation.CheckedChanged += new EventHandler(_CaptureChkFormChanged);
            this.chkCPIPageGraph.CheckedChanged += new EventHandler(_CaptureChkFormChanged);



			this.ClientSize = new System.Drawing.Size(680, 509);
			//this.Controls.Add(this.btnResDef);
			this.Controls.Add(this.btnRes);
			this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";

            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();

            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();

            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
			this.PerformLayout();

		}

        void btnBrowseDump_Click(object sender, EventArgs e)
        {
            String folder = OpenBrowse();
            if (String.IsNullOrEmpty(folder))
                return;
            this.txtCPDumpFolder.Text = folder;
        }

        private String OpenBrowse()
        {
            if (folderBrowserDialog == null)
            {
                folderBrowserDialog = new FolderBrowserDialog();
            }
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return this.folderBrowserDialog.SelectedPath;
            }
            return null;
        }


        private void _CaptureChkFormChanged(object sender, EventArgs e)
		{
            if (this.chkCPIPageGraph.Checked == false && 
                this.chkCPIPageValidation.Checked == false)
            {
                this.chkCPIPageGraph.Checked = (sender == this.chkCPIPageGraph);
                this.chkCPIPageValidation.Checked = (sender == this.chkCPIPageValidation);
                MessageBox.Show("You must select at least one category to capture.");
            }
            FormChanged();
		}
            
		private void _FormChanged(object sender, EventArgs e)
		{
			FormChanged();
		}

		void btnRes_Click(object sender, EventArgs e)
		{
			Reset();
		}

		void btnResDef_Click(object sender, EventArgs e)
		{
			ResetDefault();
		}


		void btnSave_Click(object sender, EventArgs e)
		{
			Save();
		}


        private FolderBrowserDialog folderBrowserDialog = null;

		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCPIPageValidation;
        private System.Windows.Forms.CheckBox chkCPIPageGraph;
		private System.Windows.Forms.TextBox txtCPProxyPort;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkCPClearCache;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.Windows.Forms.TextBox txtCPDumpFolder;
        private System.Windows.Forms.NumericUpDown numDumpSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Button btnBrowseDump;

		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnRes;
		private System.Windows.Forms.Button btnResDef;


	}
}
