//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.API)
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
using EYF.Tasks;
using MySpace.MSFast.Automation.Client.API;
using EYF.Core.Configuration;
using MySpace.MSFast.Automation.Client.API.Tests;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using log4net;

namespace MySpace.MSFast.Automation.Client.API
{
    public abstract class AbsClient : BaseLoopTask
    {
        public bool PrintToConsole = false;
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        private Dictionary<String, ITestRunner> runners = null;
        private MSFATestingClient client = null;
        private ITestRunner defaultTestRunner = null;

        public void Init()
        {
            base.Interval = long.Parse(AppConfig.Instance["CheckInterval"]);
            this.client = new MSFATestingClient(AppConfig.Instance["BaseDomain"], AppConfig.Instance["ClientID"], AppConfig.Instance["ClientKey"], 60000);
        }

        public void AddTestRunner(String testType, ITestRunner runner)
        {
            if (runners == null)
                runners = new Dictionary<string, ITestRunner>();

            if (runners.ContainsKey(testType))
                return;

            if (defaultTestRunner == null)
                defaultTestRunner = runner;

            runners.Add(testType, runner);

            if(PrintToConsole){
                Console.Write("\r\nNew Test Runner Added\t[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(testType);
                Console.ResetColor();
                Console.Write("]");
            }
        }

        public override TaskResults ExecuteTask()        
        {
            TestIteration testIteration = null;

            TaskResults results = new TaskResults();

            if (client == null)
                throw new Exception("Not initialized");

            if ((runners == null || runners.Count == 0 ) && defaultTestRunner == null)  
                return results;

            while (true)
            {
                if (PrintToConsole)
                {
                    Console.ResetColor();
                    Console.Write("\r\nGetting Test From Que...");
                }
                try
                {
                    if (log.IsDebugEnabled) log.Debug("Creating client and calling GetNextTestQue");
                    testIteration = client.GetNextTestQue();
                }
                catch (TestQueEmptyException)
                {
                    if (log.IsDebugEnabled) 
                        log.Debug("Que is empty...");

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("Empty");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }
                    return results;
                }
                catch (TestingClientException e)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Received TestingClientException ", e);

                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Received Exception ", ex);

                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;
                }
                if (testIteration == null)
                {
                    if (log.IsDebugEnabled)
                        log.Debug("Response was null.");

                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;
                }

                if (testIteration.CollectorsConfig == null)
                {
                    if (log.IsDebugEnabled)
                        log.Debug("Response was null.");
                    
                    if (testIteration != null && testIteration.ResultsID != 0)
                        try
                        {
                            client.MarkFailedTest(testIteration);
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("Error while setting failed test ", e);
                        }
                    
                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;
                }
                
                CollectorsConfig collectorsConfig = testIteration.CollectorsConfig;

                uint resultsID = testIteration.ResultsID;               

                if (resultsID == 0)
                {
                    if (log.IsErrorEnabled)
                        log.Error("No results_id!");

                    if (testIteration != null && testIteration.ResultsID != 0)
                        try
                        {
                            client.MarkFailedTest(testIteration);
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("Error while setting failed test ", e);
                        }

                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;
                }
                String testname = testIteration.TestName;

                if (String.IsNullOrEmpty(testname))
                {
                    if (log.IsErrorEnabled)
                        log.Error("Invalid Test Name");

                    if (testIteration != null && testIteration.ResultsID != 0)
                        try
                        {
                            client.MarkFailedTest(testIteration);
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("Error while setting failed test ", e);
                        }

                    results.Failed++;

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    return results;

                }

                String testType = collectorsConfig.GetArgumentValue("test_type");

                ITestRunner itr = null;

                if (String.IsNullOrEmpty(testType) == false)
                {
                    runners.TryGetValue(testType.Trim().ToLower(), out itr);
                }

                if (itr == null)
                    itr = defaultTestRunner;

                if(itr == null)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Invalid Test Type - Received " + testType.Trim().ToLower());

                    if (testIteration != null && testIteration.ResultsID != 0)
                        try
                        {
                            client.MarkFailedTest(testIteration);
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("Error while setting failed test ", e);
                        }

                    if (PrintToConsole)
                    {
                        Console.ResetColor();
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("\r\nGoing to sleep...");
                    }

                    results.Failed++;
                    return results;
                }

                if (log.IsDebugEnabled)
                    log.Debug("Calling ITestRunner for " + testname);

                if (PrintToConsole)
                {
                    Console.ResetColor();
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK");
                    Console.ResetColor();
                    Console.Write("]\r\nRunning Test...\t\t[");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(testname);
                    Console.ResetColor();
                    Console.Write("][");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write(testIteration.ResultsID);
                    Console.ResetColor();
                    Console.Write("]");
                }
                
                if (itr.RunTest(testIteration))
                {
                    if (PrintToConsole)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("OK");
                        Console.ResetColor(); 
                        Console.Write("]");
                    }

                    if (log.IsDebugEnabled)
                        log.Debug("Test succeeded! " + testname);
                    try
                    {
                       client.SaveSuccessfulTest(testIteration);
                    }
                    catch (Exception e)
                    {
                        if (log.IsErrorEnabled)
                            log.Error("Error while saving successful test ", e);
                    }
                    results.Succeeded++;
                }
                else
                {
                    if (PrintToConsole)
                    {
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Failed");
                        Console.ResetColor();
                        Console.Write("]");
                    }

                    if (log.IsDebugEnabled)
                        log.Debug("Test runner returned FALSE for " + testname);

                    if (testIteration != null && testIteration.ResultsID != 0)
                        try
                        {
                            client.MarkFailedTest(testIteration);
                        }
                        catch (Exception e)
                        {
                            if (log.IsErrorEnabled)
                                log.Error("Error while setting failed test ", e);
                        }

                    results.Failed--;
                }
            }
        }
    }
}
