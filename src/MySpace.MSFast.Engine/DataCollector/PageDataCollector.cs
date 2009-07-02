//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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
using System.Reflection;
using System.Threading;
using MySpace.MSFast.SuProxy.Proxy;
using System.IO;
using MySpace.MSFast.Engine.CollectorStartInfo;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.Engine
{
	public class PageDataCollector
	{
		private bool started = false;
		private object startedLock = new object();

        public event OnTestEventHandler OnTestProgress;

        public int StartTest(PageDataCollectorStartInfo settings)
		{
			if (settings == null || settings.IsValid() == false)
			{
				return (int) PageDataCollectorErrors.InvalidOrMissingArguments;
			}

			lock (startedLock)
			{
				if (started)
				{
					return (int)PageDataCollectorErrors.ObjectDisposed;
				}
				started = true;
			}
			
			PageDataCollectorErrors prepareResults = settings.PrepareStartInfo();

			if (prepareResults != PageDataCollectorErrors.NoError)
			{
				return (int)prepareResults;
			}

			String executable = null;
			
			try
			{
                executable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(PageDataCollector)).Location).Replace("\\", "/") + "/engine.exe";
			}
			catch 
			{
			}

			if (executable == null || File.Exists(executable) == false)
			{
				return (int)PageDataCollectorErrors.Unknown;
			}

			ProcessStartInfo psi = new ProcessStartInfo(executable);
			psi.CreateNoWindow = true;
			psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			psi.UseShellExecute = false;
			psi.Arguments = settings.CreateCommandLineArgs();
            psi.RedirectStandardOutput = true;

			Process listFiles = new Process();
            listFiles.OutputDataReceived += new DataReceivedEventHandler(OutputDataReceived);
            listFiles.StartInfo = (psi);
            listFiles.Start();
            listFiles.BeginOutputReadLine();
            listFiles.WaitForExit((settings.Timeout + 1) * 60 * 1000); //Wait timeout + minute

			settings.CleanUp();
			settings.Dispose();
			settings = null;

			if (listFiles.HasExited)
			{
				return listFiles.ExitCode;
			}
			else
			{
				return (int)PageDataCollectorErrors.TestTimeout;
			}
		}

        private void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OnTestProgress != null && e != null && String.IsNullOrEmpty(e.Data) == false)
            {
                TestEvents.ProcessProgress(e.Data, OnTestProgress);
            }
        }
	}
}
