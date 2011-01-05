//=======================================================================
/* Project: MSFast (MySpace.MSFast.Core)
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
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MySpace.MSFast.Core.UserExperience
{
    public static class ExceptionsHandler
    {
        public static void HandleException(Exception e)
        {
            String err = BuildErrorString(e);

            try
            {
                EventLog errLog = new EventLog();
                errLog.Source = "MSFast";
                errLog.WriteEntry(err, EventLogEntryType.Error);
            }
            catch
            {
            }
            
            try
            {
                String filename = GetErrorLogginFilename();
                StreamWriter sw = File.AppendText(filename);
                sw.WriteLine(err);
                sw.Flush();
                sw.Close();
            }
            catch 
            {
            }
        }

        private static string GetErrorLogginFilename()
        {
            return (Path.GetDirectoryName(Assembly.GetAssembly(typeof(ExceptionsHandler)).Location).Replace("\\", "/") + "/err.log");
        }

        private static string BuildErrorString(Exception e)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n===========================================================");
            sb.Append("\r\nERROR DETECTED ON ");
            sb.Append(DateTime.Now);
            sb.Append("\r\n===========================================================");
            if (e != null)
            {
                sb.Append("\r\nException Message:\r\n");
                sb.Append(e.Message);
                sb.Append("\r\n-----------------------------------------------------------");
                sb.Append("\r\nException Source:\r\n");
                sb.Append(e.Source);
                sb.Append("\r\n-----------------------------------------------------------");
                sb.Append("\r\nException Stack:\r\n");
                sb.Append(e.StackTrace);
                sb.Append("\r\n-----------------------------------------------------------");
                sb.Append("\r\nInner Exception:\r\n");
                sb.Append(e.InnerException);
                sb.Append("\r\n-----------------------------------------------------------");
            }
            return sb.ToString();
        }

        public static void FlushExceptions()
        {
           
        }
    }
}
