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
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using MySpace.MSFast.Core.Configuration.CommonDataTypes;

namespace MySpace.MSFast.GUI.Configuration.MSFast
{
	public class HttpSnifferPluginConfigPanel : Panel
	{
		public HttpSnifferPluginConfigPanel() 
		{
			InitializeComponent();
			Reset();
		}

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();

			this.SuspendLayout();
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label1);

			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Listen To";
			// 
			// comboBox1
			// 
			this.comboBox1.Location = new System.Drawing.Point(101, 11);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(326, 21);
			this.comboBox1.TabIndex = 1;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Ports";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(101, 60);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(326, 20);
			this.textBox1.TabIndex = 3;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(271, 89);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 21);
			this.button1.TabIndex = 4;
			this.button1.Text = "Save";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(352, 89);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 21);
			this.button2.TabIndex = 5;
			this.button2.Text = "Reset";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 39);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Device ID";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(98, 39);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(147, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "XXXXXXXXXXXXXXXXXXXX";

			this.PerformLayout();
			this.ResumeLayout(false);
		}

		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.button2.Enabled = true;
			this.label4.Text = "";

			/*NetworkDevice[] availableNetworkDevice = WinPcapWrapper.GetNetworkDeviceList();
			for (int i = 0; i < availableNetworkDevice.Length; i++)
			{
				if (String.IsNullOrEmpty(availableNetworkDevice[i].Name) == false && availableNetworkDevice[i].Description == this.comboBox1.Text)
				{
					this.label4.Text = availableNetworkDevice[i].Name;
					break;
				}
			}*/

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			this.button1.Enabled = true;
			this.button2.Enabled = true;

		}

		private void button1_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void Save()
		{
			IConfigSetter setter = ConfigProvider.Instance.GetConfigSetter("MSFast.Global");

			if (this.comboBox1.SelectedIndex == 0)
			{
				MessageBox.Show("Please Select a network device");
				return;
			}

			if (setter != null)
			{
				setter.SetString(MSFastGlobalConfigKeys.DEVICE_ID, (String)this.label4.Text);
				setter.SetString(MSFastGlobalConfigKeys.PORTS, this.textBox1.Text);
			}

			this.button1.Enabled = false;
			this.button2.Enabled = false;
		}

		private void Reset()
		{
			IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");

			if (getter != null)
			{

				String ndid = getter.GetString(MSFastGlobalConfigKeys.DEVICE_ID);
				String ports = getter.GetString(MSFastGlobalConfigKeys.PORTS);

				/*NetworkDevice[] availableNetworkDevice = WinPcapWrapper.GetNetworkDeviceList();

				this.comboBox1.Items.Clear();
				this.comboBox1.Items.Add("Select network device");

				int selectedNDID = 0;
				int c = 1;
				for (int i = 0; i < availableNetworkDevice.Length; i++)
				{
					if (String.IsNullOrEmpty(availableNetworkDevice[i].Name) == false)
					{
						this.comboBox1.Items.Add(availableNetworkDevice[i].Description);
						if (ndid == availableNetworkDevice[i].Name)
						{
							selectedNDID = c;
						}
						c++;
					}
				}
				this.label4.Text = "";
				this.comboBox1.SelectedIndex = selectedNDID;
				this.textBox1.Text = ports;*/

			}

			this.button1.Enabled = false;
			this.button2.Enabled = false;

		}



	}
}
