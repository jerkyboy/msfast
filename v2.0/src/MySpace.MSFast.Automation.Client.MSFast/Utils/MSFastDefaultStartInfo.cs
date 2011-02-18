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
using System.Linq;
using System.Text;
using MySpace.MSFast.Engine.CollectorStartInfo;
using System.Reflection;
using log4net;
using EYF.Core.Configuration;
using System.IO;

namespace MySpace.MSFast.Automation.Client.MSFast.Utils
{
    public class MSFastDefaultStartInfo
    {
        public static String TempFolder = @"C:\temp";
        public static String[] ConfigFiles;
        public static String EngineExecutable;
        public static String CollectorScripts;
        public static int Timeout = 5;//min

        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();

        public static void Init()
        {
            TempFolder = System.Environment.GetEnvironmentVariable("TestTempFolder");

            if (String.IsNullOrEmpty(TempFolder))
                TempFolder = AppConfig.Instance["TestTempFolder"];

            if (String.IsNullOrEmpty(TempFolder))
                TempFolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastDefaultStartInfo)).Location);

            TempFolder = TempFolder.Replace('/', '\\').Trim();

            if (TempFolder.EndsWith("\\") == false)
                TempFolder += "\\";


            EngineExecutable = System.Environment.GetEnvironmentVariable("EngineExecutable");

            if (String.IsNullOrEmpty(EngineExecutable))
                EngineExecutable = AppConfig.Instance["EngineExecutable"];

            if (String.IsNullOrEmpty(EngineExecutable))
            {
                EngineExecutable = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastDefaultStartInfo)).Location).Replace('/', '\\').Trim();
                if (EngineExecutable.EndsWith("\\") == false)
                    EngineExecutable += "\\";

                EngineExecutable = EngineExecutable + "engine.exe";
            }

            String cnfFiles = System.Environment.GetEnvironmentVariable("MSFastConfigFiles");

            if (String.IsNullOrEmpty(cnfFiles))
                cnfFiles = AppConfig.Instance["MSFastConfigFiles"];

            if (String.IsNullOrEmpty(cnfFiles))
            {
                cnfFiles = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastDefaultStartInfo)).Location).Replace('/', '\\').Trim();
                if (cnfFiles.EndsWith("\\") == false)
                    cnfFiles += "\\";

                cnfFiles = cnfFiles + "conf\\SuProxy.default.config|" + cnfFiles + "conf\\SuProxy.msfast.config";
            }
            ConfigFiles = cnfFiles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);


            CollectorScripts = System.Environment.GetEnvironmentVariable("MSFastCollectorScripts");

            if (String.IsNullOrEmpty(CollectorScripts) || File.Exists(CollectorScripts) == false)
                CollectorScripts = AppConfig.Instance["MSFastCollectorScripts"];

            if (String.IsNullOrEmpty(CollectorScripts) || File.Exists(CollectorScripts) == false)
            {
                CollectorScripts = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastDefaultStartInfo)).Location).Replace('/', '\\').Trim();
                if (CollectorScripts.EndsWith("\\") == false)
                    CollectorScripts += "\\";

                CollectorScripts = CollectorScripts + "TestsScript.config";
            }
        }

        public static bool SetDefaultStartupInfo(PageDataCollectorStartInfo chr, Uri testUri, int resultId)
        {
            chr.DumpFolder = TempFolder;
            chr.TempFolder = TempFolder;
            chr.ClearCache = true;
            chr.CollectionID = resultId;
            chr.ProxyPort = 8081;
            chr.EngineExecutable = EngineExecutable;
            chr.IsDebug = !false;
            chr.URL = testUri.ToString();
            chr.ConfigFiles = ConfigFiles;
            chr.CollectorScripts = CollectorScripts;
            chr.Timeout = Timeout;

            if (log.IsDebugEnabled && chr != null)
            {
                log.Debug("Setting Default Startup Info :");
                log.Debug("DumpFolder: " + chr.DumpFolder);
                log.Debug("TempFolder: " + chr.TempFolder);
                log.Debug("ClearCache: " + chr.ClearCache);
                log.Debug("CollectionID: " + chr.CollectionID);
                log.Debug("ProxyPort: " + chr.ProxyPort);
                log.Debug("EngineExecutable: " + chr.EngineExecutable);
                log.Debug("IsDebug: " + chr.IsDebug);
                log.Debug("URL: " + chr.URL);
                log.Debug("ConfigFiles: " + String.Join(", ", chr.ConfigFiles));
                log.Debug("CollectorScripts: " + CollectorScripts);
                
            }

            return true;
        }

    }
}
