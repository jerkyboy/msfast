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
using MySpace.MSFast.Automation.Client.API.Tests;
using log4net;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.DataProcessors;
using System.Reflection;
using System.IO;
using MySpace.MSFast.Automation.Client.API;
using MySpace.MSFast.Automation.Client.MSFast.Utils;
using EYF.Core.Configuration;
using Microsoft.Build.Utilities;
using MySpace.MSFast.Engine;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.Automation.Client.MSFast
{
    public class DirectLinkTestRunner : ITestRunner
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();

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
                if (log.IsErrorEnabled)
                    log.Error("Invalid config, aborting test...");

                return false;
            }

            CollectorsConfig cc = testIteration.CollectorsConfig;

            String testProtocol = cc.GetArgumentValue("test_protocol");
            String testDomain = cc.GetArgumentValue("test_domain");
            String testPath = cc.GetArgumentValue("test_path");
            String testQueryString = cc.GetArgumentValue("test_query_string");

            Uri testUri = new Uri(String.Concat(testProtocol, "://", testDomain, testPath, testQueryString));

            if (log.IsDebugEnabled)
                log.Debug("Test URL(" + testUri + "), Results ID " + testIteration.ResultsID);

            String isDevLogin = cc.GetArgumentValue("dev_login");
            String isRequireLogin = cc.GetArgumentValue("require_login");
            String loginUsername = cc.GetArgumentValue("login_username");
            String loginPassword = cc.GetArgumentValue("login_password");

            ProcessedDataPackage results = null;

            if (String.IsNullOrEmpty(isRequireLogin) || isRequireLogin.ToLower().Trim().Equals("false"))
            {
                results = RunDirectTest(testIteration, testUri, testIteration.ResultsID);
            }
            else if (String.IsNullOrEmpty(isDevLogin) || isDevLogin.ToLower().Trim().Equals("false"))
            {
                results = RunTestWithProductionLogin(testIteration, testUri, testIteration.ResultsID, loginUsername, loginPassword);
            }
            else
            {
                results = RunTestWithDevLogin(testIteration, testUri, testIteration.ResultsID, loginUsername, loginPassword);
            }

            testIteration.ProcessedDataPackage = results;

            return results != null;
        }

        private ProcessedDataPackage RunTestWithDevLogin(TestIteration testIteration, Uri testUri, uint resultsID, string loginUsername, string loginPassword)
        {
            return RunWebTest(testIteration, testUri, resultsID, @"WebTests\Development\DevelopmentLogin.webtest");
        }

        private ProcessedDataPackage RunTestWithProductionLogin(TestIteration testIteration, Uri testUri, uint resultsID, string loginUsername, string loginPassword)
        {
            return RunWebTest(testIteration, testUri, resultsID, @"WebTests\Production\ProductionLogin.webtest");
        }

        private ProcessedDataPackage RunWebTest(TestIteration testIteration, Uri testUri, uint resultsID, string webtest)
        {
            String webTestBase = Path.GetDirectoryName(Assembly.GetAssembly(typeof(DirectLinkTestRunner)).Location).Replace('/', '\\').Trim();

            if (webTestBase.EndsWith("\\") == false)
                webTestBase += "\\";

            webtest = webTestBase + webtest;

            //Start the webtest
            ProcessHelper helper = new ProcessHelper();
            helper.FileName = AppConfig.Instance["MSTest"]; 
            helper.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

            helper.EnvironmentVariables.Add("collectorsConfig", new JSONCollectorsConfigWriter().SerializeConfig(testIteration.CollectorsConfig));
            helper.EnvironmentVariables.Add("resultsId", resultsID.ToString());
            helper.EnvironmentVariables.Add("TestTempFolder", MSFastDefaultStartInfo.TempFolder);
            helper.EnvironmentVariables.Add("EngineExecutable", MSFastDefaultStartInfo.EngineExecutable);
            helper.EnvironmentVariables.Add("MSFastConfigFiles", String.Join("|",MSFastDefaultStartInfo.ConfigFiles));
            helper.EnvironmentVariables.Add("MSFastCollectorScripts", MSFastDefaultStartInfo.CollectorScripts);
            
            if (String.IsNullOrEmpty(webtest) || File.Exists(webtest) == false)
            {
                if (log.IsFatalEnabled)
                    log.FatalFormat("Can't locate web-test! {0}", webtest);

                return null;
            }

            CommandLineBuilder builder = new CommandLineBuilder();
            builder.AppendSwitch("/noisolation");
            builder.AppendSwitch("/nologo");
            builder.AppendSwitchIfNotNull("/resultsfile:", String.Format(AppConfig.Instance["ResultsFilePattern"], resultsID));
            builder.AppendSwitchIfNotNull("/runconfig:", @"TrackerWebTest.testrunConfig");
            builder.AppendSwitchIfNotNull("/TestContainer:", webtest);

            helper.Arguments = builder.ToString();
            helper.Timeout = int.Parse(AppConfig.Instance["MSTestTimeout"]);

            if (log.IsDebugEnabled)
                log.DebugFormat("Arguments passed to MsTest {0}", helper.Arguments);

            int ExitCode = helper.Execute();

            if (log.IsDebugEnabled)
                log.DebugFormat("Process exit code = {0}\r\nStandard Out: {1}\r\nStandard Error: {2}", ExitCode, helper.StandardOutput, helper.StandardError);

            if (0 != ExitCode)
            {
                if (log.IsFatalEnabled)
                    log.FatalFormat("MsTest.exe exited with an error code of {0} rather than 0.\r\nStandard Out: {1}\r\nStandard Error:{2}", helper.Timeout, helper.StandardOutput, helper.StandardError);

                return null;
            }

            return ProcessData(resultsID);
        }

        private ProcessedDataPackage RunDirectTest(TestIteration testIteration, Uri testUri, uint resultsID)
        {
            PageDataCollectorStartInfo chr = new PageDataCollectorStartInfo();

            if (MSFastDefaultStartInfo.SetDefaultStartupInfo(chr, testUri, (int)resultsID))
            {
                int results = new PageDataCollector().StartTest(chr);

                if (results == 0)
                {
                    return ProcessData(resultsID);
                }
                else if (log.IsDebugEnabled)
                {
                    log.Debug("Results Code : " + (PageDataCollectorErrors)results + " ( " + results + ")");
                }

            }

            return null;

        }

        private ProcessedDataPackage ProcessData(uint resultsID)
        {
            return ProcessedDataCollector.CollectAll(MSFastDefaultStartInfo.TempFolder, (int)resultsID);
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

            if (String.IsNullOrEmpty(testProtocol))
            {
                if (log.IsDebugEnabled)
                    log.Debug("Protocol is missing, setting as 'http' ");

                cc.SetArgument("test_protocol", "http");
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
            catch (Exception e)
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