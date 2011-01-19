using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Engine.CollectorStartInfo;
using EYF.Core.Configuration;
using System.Reflection;
using System.IO;

namespace BDika.Tasks.TestsExecuter.Utils
{
    public class MSFastDefaultStartInfo
    {
        public static String TempFolder;
        public static String[] ConfigFiles;
        public static String EngineExecutable;

        static MSFastDefaultStartInfo()
        {
            TempFolder = @"C:\temp";
                       
            TempFolder = System.Environment.GetEnvironmentVariable("TestTempFolder");
    
            if(String.IsNullOrEmpty(TempFolder))
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
            
            if(String.IsNullOrEmpty(cnfFiles))
                cnfFiles = AppConfig.Instance["MSFastConfigFiles"];
            
            ConfigFiles = cnfFiles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool SetDefaultStartupInfo(PageDataCollectorStartInfo chr, Uri testUri, int resultId)
        {
            
            chr.DumpFolder = TempFolder;
            chr.TempFolder = TempFolder;
            chr.ClearCache = true;
            chr.CollectionID = resultId;
            chr.ProxyPort = 8081;
            chr.EngineExecutable = EngineExecutable;
            chr.IsDebug = true;

            chr.URL = testUri.ToString();
            chr.ConfigFiles = ConfigFiles;

            return true;
        }

    }
}
