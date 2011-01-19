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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySpace.MSFast.Core.Logger;
using MySpace.MSFast.SuProxy.Proxy;
using System.IO;
using System.Reflection;

namespace MySpace.MSFast.GUI.SuProxy
{

    public class SuProxyControlPanelForm : Form
    {
        private static readonly MSFastLogger log = MSFastLogger.GetLogger(typeof(SuProxyControlPanelForm));
        private GroupBox groupBox1;
        private Button btnStartProxy;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStartProxy = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStartProxy);
            this.groupBox1.Location = new System.Drawing.Point(132, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(371, 106);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SuProxy";
            // 
            // btnStartProxy
            // 
            this.btnStartProxy.Location = new System.Drawing.Point(88, 40);
            this.btnStartProxy.Name = "btnStartProxy";
            this.btnStartProxy.Size = new System.Drawing.Size(93, 33);
            this.btnStartProxy.TabIndex = 0;
            this.btnStartProxy.Text = "Start Proxy";
            this.btnStartProxy.UseVisualStyleBackColor = true;
            this.btnStartProxy.Click += new System.EventHandler(this.btnStartProxy_Click);
            // 
            // SuProxyControlPanelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 576);
            this.Controls.Add(this.groupBox1);
            this.Name = "SuProxyControlPanelForm";
            this.Text = "SuProxyControlPanel";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SuProxyServer proxyServer = null;

        public SuProxyControlPanelForm()
        {
            InitializeComponent();
        }

        private void btnStartProxy_Click(object sender, EventArgs e)
        {
            SuProxyConfiguration spc = new SuProxyConfiguration(8080, 100, 20, 2 * 60 * 1000, 2);
            spc.ConfigurationFiles = new String[]{
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(SuProxyControlPanelForm)).Location) + "\\conf\\SuProxy.default.config", 
                Path.GetDirectoryName(Assembly.GetAssembly(typeof(SuProxyControlPanelForm)).Location) + "\\conf\\SuProxy.Standalone.config" 
            };

            proxyServer = new SuProxyServer(spc);
            proxyServer.Start();
            btnStartProxy.Enabled = false;
        }        
    }
}
