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

	public partial class MSFastGlobalConfigPanel : Panel
	{
		public MSFastGlobalConfigPanel()
		{
			InitializeComponent();
			Reset();
		}

		private void Save()
		{
			IConfigSetter setter = ConfigProvider.Instance.GetConfigSetter("MSFast.Global");

			if (setter != null)
			{
                setter.SetString(MSFastGlobalConfigKeys.TEMP_FOLDER, (String)this.txtCPDumpFolder.Text);
				setter.SetString(MSFastGlobalConfigKeys.DUMP_FOLDER, (String)this.txtCPDumpFolder.Text);
                setter.SetString(MSFastGlobalConfigKeys.DEFAULT_PROXY_PORT, this.txtCPProxyPort.Text);
				
                setter.SetBoolean(MSFastGlobalConfigKeys.CLEAR_CACHE_BEFORE_TEST, this.chkCPClearCache.Checked);
                setter.SetBoolean(MSFastGlobalConfigKeys.PAGE_VALIDATION, this.chkCPIPageValidation.Checked);
                setter.SetBoolean(MSFastGlobalConfigKeys.PAGE_GRAPH, this.chkCPIPageGraph.Checked);

                setter.SetInt(MSFastGlobalConfigKeys.DUMP_MAX_SIZE, (int)this.numDumpSize.Value);
                
			}

			FormSet();
		}

		private void Reset()
		{
			IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");

			if (getter != null)
			{

				//String tempFolder = getter.GetString(MSFastGlobalConfigKeys.TEMP_FOLDER);
				String dumpFolder = getter.GetString(MSFastGlobalConfigKeys.DUMP_FOLDER); 

				this.txtCPDumpFolder.Text = dumpFolder;
                this.txtCPProxyPort.Text = getter.GetString(MSFastGlobalConfigKeys.DEFAULT_PROXY_PORT);

				this.chkCPClearCache.Checked = getter.GetBoolean(MSFastGlobalConfigKeys.CLEAR_CACHE_BEFORE_TEST);
                this.chkCPIPageValidation.Checked = getter.GetBoolean(MSFastGlobalConfigKeys.PAGE_VALIDATION);
                this.chkCPIPageGraph.Checked = getter.GetBoolean(MSFastGlobalConfigKeys.PAGE_GRAPH);

                this.numDumpSize.Value = Math.Min(1024,(Math.Max(16,getter.GetInt(MSFastGlobalConfigKeys.DUMP_MAX_SIZE))));
			}

			FormSet();

		}

		private void ResetDefault()
		{
			
		}

		private void FormChanged()
		{
			this.btnSave.Enabled = true;
			this.btnRes.Enabled = true;
			this.btnResDef.Enabled = true;
		}

		private void FormSet()
		{
			this.btnSave.Enabled = false;
			this.btnRes.Enabled = false;
			this.btnResDef.Enabled = true;
		}

	}
}
