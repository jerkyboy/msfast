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
using System.Windows.Forms;
using MySpace.MSFast.ImportExportsMgrs;
using System.IO;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.Engine;
using MySpace.MSFast.Engine.CollectorStartInfo;
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.GUI.Engine
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
            PageDataCollectorStartInfo csr = new PageDataCollectorStartInfo();
            csr.URL = "http://www.google.com/";
            PageDataCollector pdc = new PageDataCollector();
            int a = pdc.StartTest(csr);

            string[] args = Environment.GetCommandLineArgs();

            String openFile = null;

            if (args != null && args.Length == 2)
                openFile = args[1];

            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(openFile));

		}
	}
}
