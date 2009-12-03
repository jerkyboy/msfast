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
using MySpace.MSFast.ImportExportsMgrs;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Render;

namespace MySpace.MSFast.DataProcessors.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            MSFImportExportsManager m = new MSFImportExportsManager();

            //m.LoadProcessedDataPackage(File.Open("C:\\new.msf", FileMode.OpenOrCreate), newD);
            ProcessedDataPackage p = m.LoadProcessedDataPackage(File.Open("C:\\temp\\old.msf", FileMode.Open));

            RenderData dd = (RenderData)p[typeof(RenderData)];

            CommandLineArguments cla = new CommandLineArguments(args);

            if (cla.IsValid() == false)
            {
                CommandLineArguments.PrintUsage(System.Console.Error);
                return;
            }

            String outfolder = Directory.GetCurrentDirectory().ToString().Replace("\\", "/");
            String confolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location) + "\\conf\\";

            if (String.IsNullOrEmpty(confolder) || 
                String.IsNullOrEmpty(outfolder) || 
                Directory.Exists(confolder) == false ||
                Directory.Exists(outfolder) == false ||
                File.Exists(confolder + "DefaultPageValidation.xml") == false)
            {
                System.Console.Error.Write("Invalid Configuration!");
                return;
            }

            ProcessedDataPackage package = null;
            
            try
            {
                package = ProcessedDataCollector.CollectAll(cla.Path, cla.CollectionId);
            }
            catch (DirectoryNotFoundException)
            {
                System.Console.Error.Write("Invalid input folder!");
                return;
            }

            if (package == null || package.Count == 0)
            {
                System.Console.Error.Write("Error while processing data!");
                return;
            }

            ValidationRunner vr = new ValidationRunner();
            vr.LoadFromFile(confolder + "DefaultPageValidation.xml");

            ValidationResultsPackage rsults = vr.ValidateBlocking(package);


            if (rsults == null || rsults.Count == 0)
            {
                System.Console.Error.Write("Error while validating results!");
                return;
            } 
            
            if ("xml".Equals(cla.SaveType))
            {
                SavePackage(new XMLImportExportManager(), package, outfolder);
            }
            else if ("msf".Equals(cla.SaveType))
            {
                SavePackage(new MSFImportExportsManager(), package, outfolder);
            }
            else if ("har".Equals(cla.SaveType))
            {
                SavePackage(new HARImportExportsManager(), package, outfolder);
            }

            SaveValidation(rsults, outfolder);
           
        }

        private static void SavePackage(ImportExportManager importExportManager, ProcessedDataPackage package, String outfolder)
        {        
            Stream myStream = null;

            try
            {
                String filename = outfolder + "/collectedData." + importExportManager.DefaultExtension;
                myStream = File.Open(filename, FileMode.Create);
                importExportManager.SaveProcessedDataPackage(myStream, package);
                System.Console.WriteLine("Output saved to:");
                System.Console.WriteLine(filename);
            }
            catch
            {
                System.Console.Error.Write("Error while saving data!");
                return;
            }
            finally
            {
                if (myStream != null)
                {
                    try
                    {
                        myStream.Flush();
                    }
                    catch 
                    {
                    }
                    myStream.Close();
                    myStream.Dispose();
                }
            }
        }

        private static void SaveValidation(ValidationResultsPackage rsults, String outfolder)
        {
            XmlDocument xml = null;
            
            if ((xml = rsults.Serialize()) == null)
            {
                System.Console.Error.Write("Error while serializing validation results!");
                return;
            }

            xml.Save(outfolder + "/validationResults.xml");

            System.Console.WriteLine("Validation saved to:");
            System.Console.WriteLine("  " + outfolder + "/validationResults.xml");
        }
    }
}














