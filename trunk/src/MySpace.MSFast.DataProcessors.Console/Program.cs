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
using System.Xml;
using System.IO;
using MySpace.MSFast.DataValidators;
using System.Reflection;
using MySpace.MSFast.DataValidators.ValidationResultTypes;
using MySpace.MSFast.DataProcessors.DataValidators.ValidationResultTypes;

namespace MySpace.MSFast.DataProcessors.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.Read();
            CommandLineArguments cla = new CommandLineArguments(args);

            if (cla.IsValid() == false)
            {
                CommandLineArguments.PrintUsage(System.Console.Error);
                return;
            }

            String outfolder = Directory.GetCurrentDirectory().ToString().Replace("\\", "/");
            String confolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location) + "\\conf\\";
            
            XmlDocument xml = null;

            if (String.IsNullOrEmpty(confolder) || 
                String.IsNullOrEmpty(outfolder) || 
                Directory.Exists(confolder) == false ||
                Directory.Exists(outfolder) == false ||
                File.Exists(confolder + "DefaultPageValidation.xml") == false)
            {
                System.Console.Error.Write("Invalid Configuration!");
                return;
            }

            
            ProcessedDataPackage package = ProcessedDataCollector.CollectAll(cla.Path, cla.CollectionId);
            
            if (package == null || package.Count == 0 || (xml = package.Serialize()) == null)
            {
                System.Console.Error.Write("Error while serializing the data!");
                return;
            }

            xml.Save(outfolder + "/collectedData.xml");






            ValidationRunner vr = new ValidationRunner();
            vr.LoadFromFile(confolder + "DefaultPageValidation.xml");

            ValidationResultsPackage rsults = vr.ValidateBlocking(package);

            if (rsults == null || rsults.Count == 0 || (xml = rsults.Serialize()) == null)
            {
                System.Console.Error.Write("Error while serializing validation results!");
                return;
            }

            xml.Save(outfolder + "/validationResults.xml");

            System.Console.WriteLine("Output saved to:");
            System.Console.WriteLine("  " + outfolder + "/collectedData.xml");
            System.Console.WriteLine("  " + outfolder + "/validationResults.xml");
        }
    }
}














