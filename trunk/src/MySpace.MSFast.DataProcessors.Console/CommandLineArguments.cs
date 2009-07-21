//=======================================================================
/* Project: MSFast (MySpace.MSFast.DataProcessors.Console)
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
using System.IO;

namespace MySpace.MSFast.DataProcessors.Console
{
    public class CommandLineArguments
    {

        public String Path = null;
        public int CollectionId = 1;

		private static Dictionary<String, ArgumentsParser> ArgsParsers;
        delegate void ArgumentsParser(String name, String value, CommandLineArguments si);

        static CommandLineArguments()
		{
			ArgsParsers = new Dictionary<string, ArgumentsParser>();
            ArgsParsers.Add("/path:", new ArgumentsParser(delegate(String name, String value, CommandLineArguments si) { si.Path = value.Replace("\\", "/"); }));
            ArgsParsers.Add("/id:", new ArgumentsParser(delegate(String name, String value, CommandLineArguments si) { try { si.CollectionId = int.Parse(value); } catch { } }));
		}




        public CommandLineArguments(String[] commandLineArguments)
		{
			this.ParseCommandLineArgs(commandLineArguments);
		}

        public virtual bool IsValid()
        {
            return (String.IsNullOrEmpty(Path) == false && Directory.Exists(Path));
        }


        private void ParseCommandLineArgs(String[] commandLineArguments)
        {
            if (commandLineArguments != null)
            {
                foreach (String s in commandLineArguments)
                {
                    foreach (String ak in ArgsParsers.Keys)
                    {
                        if (CheckAndPlaceValue(s, ak, ArgsParsers[ak]))
                        {
                            break;
                        }
                    }
                }
            }
        }

        private bool CheckAndPlaceValue(string s, string ky, ArgumentsParser ap)
        {
            if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(ky))
                return false;

            if (s.Trim().ToLower().StartsWith(ky))
            {
                s = s.Substring(ky.Length);
                ap(ky, s, this);
                return true;
            }
            return false;
        }

        public static void PrintUsage(TextWriter tw)
        {
            tw.WriteLine("--------------------------------------------------------------------\r\n");
            tw.WriteLine(" MSFast Results Processor, MySpace.com (c) 2009\r\n");
            tw.WriteLine("--------------------------------------------------------------------\r\n");
            tw.Write(" Use:  processor.exe");
            tw.WriteLine(" /path:\"<DUMP FOLDER>\"\r\n");
            tw.WriteLine("--------------------------------------------------------------------\r\n");
            tw.WriteLine(" Optional Parameters:");
            tw.WriteLine("");
            tw.WriteLine("    /id:<Collection ID>           ID of the collection\r\n");
            tw.WriteLine("--------------------------------------------------------------------");
        }
    }
}
