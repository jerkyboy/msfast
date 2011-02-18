//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.MSFast)
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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace MySpace.MSFast.Automation.Client.MSFast.Utils
{
    public class ProcessHelper
    {
        public ProcessHelper()
        {
            this.Timeout = -1;
        }
        StringBuilder _StandardError;
        StringBuilder _StandardOutput;

        private string _FileName;
        private string _Arguments;
        private string _WorkingDirectory;

        /// <summary>
        /// Name of the file to launch. This should be a full path of the exe should be used.
        /// </summary>
        public string FileName { get { return _FileName; } set { _FileName = value; } }
        /// <summary>
        /// Arguments that will be passed to the executable. 
        /// </summary>
        public string Arguments { get { return _Arguments; } set { _Arguments = value; } }
        /// <summary>
        /// Working Directory to start the process from. The current working directory of the 
        /// process will be used if this is not specified. 
        /// </summary>
        public string WorkingDirectory { get { return _WorkingDirectory; } set { _WorkingDirectory = value; } }
        /// <summary>
        /// Results from the Standard Output stream.
        /// </summary>
        public string StandardOutput { get { return _StandardOutput.ToString(); } }
        /// <summary>
        /// Results from the Standard Output stream.
        /// </summary>
        public string StandardError { get { return _StandardError.ToString(); } }
        public int Timeout { get; set; }

        public StringDictionary EnvironmentVariables = new StringDictionary();

        /// <summary>
        /// Method is used to execute a process and return the 
        /// result code.
        /// </summary>
        /// <returns>Returns the exit code from the process.</returns>
        public int Execute()
        {
            Process m_proc = null;

            try
            {
                _StandardError = new StringBuilder();
                _StandardOutput = new StringBuilder();

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = _FileName;
                psi.Arguments = _Arguments;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;

                if (this.EnvironmentVariables != null)
                    foreach (String k in this.EnvironmentVariables.Keys)
                        psi.EnvironmentVariables.Add(k, this.EnvironmentVariables[k]);

                if (!string.IsNullOrEmpty(_WorkingDirectory))
                    psi.WorkingDirectory = _WorkingDirectory;

                m_proc = new Process();
                m_proc.StartInfo = psi;
                m_proc.Start();
                m_proc.PriorityClass = ProcessPriorityClass.AboveNormal;

                Thread stdOutThread = new Thread(processStdOut);
                stdOutThread.IsBackground = true;
                stdOutThread.Start(m_proc.StandardOutput);

                Thread stdErrThread = new Thread(processStdErr);
                stdErrThread.IsBackground = true;
                stdErrThread.Start(m_proc.StandardError);

                if (this.Timeout > 0)
                    m_proc.WaitForExit(this.Timeout);
                else
                    m_proc.WaitForExit();

                stdOutThread.Join();
                stdErrThread.Join();

                return m_proc.ExitCode;
            }
            finally
            {
                if (null != m_proc)
                    m_proc.Dispose();
            }
        }

        void processStdErr(object state)
        {
            processOutput(state, _StandardError);
        }

        void processStdOut(object state)
        {
            processOutput(state, _StandardOutput);
        }

        void processOutput(object state, StringBuilder builder)
        {
            StreamReader sr = (StreamReader)state;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                builder.AppendLine(line);
            }
        }
    }
}
