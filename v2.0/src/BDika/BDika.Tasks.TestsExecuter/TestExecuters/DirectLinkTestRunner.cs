using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using log4net;
using System.Reflection;
using System.IO;
using BDika.Client.API.Tests;
using MySpace.MSFast.DataProcessors;
using EYF.Core.Configuration;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine;

namespace BDika.Tasks.TestsExecuter.TestExecuters
{
    public class DirectLinkTestRunner : ITestRunner
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        public String TempFolder;
        public String[] ConfigFiles;

        public DirectLinkTestRunner()
        {
            TempFolder = AppConfig.Instance["TestTempFolder"];
            
            if (String.IsNullOrEmpty(this.TempFolder))
                TempFolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DirectLinkTestRunner)).Location);

            TempFolder = TempFolder.Replace('/','\\').Trim();
            
            if (TempFolder.EndsWith("\\") == false)
                TempFolder += "\\";

            this.ConfigFiles = AppConfig.Instance["MSFastConfigFiles"].Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries);
        }

        public bool RunTest(TestIteration testIteration)

        {
            if (testIteration == null || testIteration.CollectorsConfig == null || testIteration.ResultsID == 0)
            {
                if (log.IsDebugEnabled)
                    log.Debug("Invalid TestIteration ");
                
                return false;
            }

            if (ValidateCollectorConfig(testIteration.CollectorsConfig) == false)
            {
                if(log.IsErrorEnabled)
                    log.Error("Invalid config, aborting test...");

                return false;
            }
            
            CollectorsConfig cc = testIteration.CollectorsConfig;

            String testProtocol = cc.GetArgumentValue("test_protocol");
            String testDomain = cc.GetArgumentValue("test_domain");
            String testPath = cc.GetArgumentValue("test_path");
            String testQueryString = cc.GetArgumentValue("test_query_string");

            Uri testUri = new Uri(String.Concat(testProtocol, "://", testDomain, testPath, testQueryString));

            if(log.IsDebugEnabled)
                log.Debug("Test URL(" + testUri + "), Results ID " + testIteration.ResultsID);

            String isDevLogin = cc.GetArgumentValue("dev_login");
            String isRequireLogin = cc.GetArgumentValue("require_login");
            String loginUsername = cc.GetArgumentValue("login_username");
            String loginPassword = cc.GetArgumentValue("login_password");

            ProcessedDataPackage results = null;

            if (String.IsNullOrEmpty(isRequireLogin) || isRequireLogin.ToLower().Trim().Equals("false"))
            {
                results = RunDirectTest(testUri, testIteration.ResultsID);
            }
            else if(String.IsNullOrEmpty(isDevLogin) || isDevLogin.ToLower().Trim().Equals("false"))
            {
                results = RunTestWithProductionLogin(testUri, testIteration.ResultsID, loginUsername, loginPassword);
            }else
            {
                results = RunTestWithDevLogin(testUri, testIteration.ResultsID, loginUsername, loginPassword);
            }

            testIteration.ProcessedDataPackage = results;

            return results != null;
        }

        private ProcessedDataPackage RunTestWithDevLogin(Uri testUri, uint resultsID, string loginUsername, string loginPassword)
        {
            return RunWebTest(@"\WebTests\Generic\Development\DevelopmentLogin.webtest", testUri, resultsID, loginUsername, loginPassword);
        }

        private ProcessedDataPackage RunTestWithProductionLogin(Uri testUri, uint resultsID, string loginUsername, string loginPassword)
        {
            return RunWebTest(@"\WebTests\Generic\Production\ProductionLogin.webtest", testUri, resultsID, loginUsername, loginPassword);
        }

        private ProcessedDataPackage RunWebTest(String webtest, Uri testUri, uint resultsID, string loginUsername, string loginPassword)
        {
            String webTestBase = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DirectLinkTestRunner)).Location).Replace('/','\\').Trim();
            
            if(webTestBase.EndsWith("\\") == false)
                webTestBase += "\\";

            webTestBase += webTestBase;

            return ProcessData(resultsID);
        }        

        private ProcessedDataPackage RunDirectTest(Uri testUri, uint resultsID)
        {
            PageDataCollectorStartInfo chr = new PageDataCollectorStartInfo();

            chr.DumpFolder = this.TempFolder;
            chr.TempFolder = this.TempFolder;
            chr.ClearCache = true;
            chr.CollectionID = (int)resultsID;
            chr.ProxyPort = 8081;

            chr.IsDebug = true;

            chr.URL = testUri.ToString();

            chr.ConfigFiles = ConfigFiles;

            int results = new PageDataCollector().StartTest(chr);

            if(results == 0)
                return ProcessData(resultsID);
            
            return null;

        }

        private ProcessedDataPackage ProcessData(uint resultsID)
        {
            return ProcessedDataCollector.CollectAll(this.TempFolder, (int)resultsID);
        }

        private bool ValidateCollectorConfig(CollectorsConfig cc)
        {
            if (log.IsDebugEnabled)
                log.Debug("Validating Config...");


            String testProtocol = cc.GetArgumentValue("test_protocol");
            String testDomain = cc.GetArgumentValue("test_domain");
            String testPath = cc.GetArgumentValue("test_path");
            String testQueryString = cc.GetArgumentValue("test_query_string");

            String isRequireLogin = cc.GetArgumentValue("require_login");
            String loginUsername = cc.GetArgumentValue("login_username");
            String loginPassword = cc.GetArgumentValue("login_password");

            if(String.IsNullOrEmpty(testProtocol))
            {
                if (log.IsDebugEnabled)
                    log.Debug("Protocol is missing, setting as 'http' ");

                cc.SetArgument("test_protocol","http");
            }

            if (String.IsNullOrEmpty(testDomain))
            {
                if (log.IsErrorEnabled)
                    log.Error("Invalid Test Domain");

                return false;
            }

            if (String.IsNullOrEmpty(testPath))
            {
                if (log.IsErrorEnabled)
                    log.Error("Invalid Test Path");

                return false;
            }
            
            Uri uri = null;

            try
            {
                uri = new Uri(String.Concat(testProtocol, "://", testDomain, testPath, testQueryString));
            }
            catch(Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("Invalid test uri ", e);
                
                return false;
            }
            if (uri == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("Invalid test uri!");

                return false;
            }

            if (String.IsNullOrEmpty(isRequireLogin) || isRequireLogin.ToLower().Trim().Equals("false"))
                return true;

            if (String.IsNullOrEmpty(loginUsername) || String.IsNullOrEmpty(loginPassword))
            {
                if (log.IsErrorEnabled)
                    log.Error("Invalid login credentials");
                
                return false;
            }

            return true;
        }
    }
}
