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

        static MSFastDefaultStartInfo()
        {
            TempFolder = @"C:\temp";
            String cnfFiles = @"G:\Development\MSFast\src\bin\Debug\conf\SuProxy.default.config|G:\Development\MSFast\src\bin\Debug\conf\SuProxy.msfast.config";
            /*
            TempFolder = System.Environment.GetEnvironmentVariable("TestTempFolder");
    
            if(String.IsNullOrEmpty(TempFolder))
                TempFolder = AppConfig.Instance["TestTempFolder"];

            if (String.IsNullOrEmpty(TempFolder))
                TempFolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastDefaultStartInfo)).Location);

            TempFolder = TempFolder.Replace('/', '\\').Trim();

            if (TempFolder.EndsWith("\\") == false)
                TempFolder += "\\";


            String cnfFiles = System.Environment.GetEnvironmentVariable("MSFastConfigFiles");
            
            if(String.IsNullOrEmpty(cnfFiles))
                cnfFiles = AppConfig.Instance["MSFastConfigFiles"];
            */
            ConfigFiles = cnfFiles.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool SetDefaultStartupInfo(PageDataCollectorStartInfo chr, Uri testUri, int resultId)
        {
            chr.DumpFolder = TempFolder;
            chr.TempFolder = TempFolder;
            chr.ClearCache = true;
            chr.CollectionID = resultId;
            chr.ProxyPort = 8081;

            chr.IsDebug = true;

            chr.URL = testUri.ToString();
            chr.ConfigFiles = ConfigFiles;

            return true;
        }

    }
}
