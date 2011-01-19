using System;
using System.Collections.Generic;
using System.Text;
using EYF.Tasks;
using EYF.Core.Configuration;
using log4net;
using BDika.Client.API;
using BDika.Providers.Triggers;
using BDika.Entities.Triggers;
using EYF.Entities.StandardObjects;
using BDika.Providers.Collectors;
using BDika.Entities.Tests;
using BDika.Providers.Results;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Tasks.TestsExecuter.TestExecuters;
using BDika.Client.API.Tests;

namespace BDika.Tasks.TestsExecuter
{
    public class TestsExecuterTask : BaseLoopTask
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        private Dictionary<String, ITestRunner> runners = null;

        public TestsExecuterTask()
        {
            runners = new Dictionary<string, ITestRunner>();
            runners.Add("msfast", new DirectLinkTestRunner());

            if (log.IsDebugEnabled) log.Debug("Starting checking tests que");
            if (log.IsInfoEnabled) log.Info("Setting time interval to " + AppConfig.Instance["CheckInterval"]);

            base.Interval = long.Parse(AppConfig.Instance["CheckInterval"]);
        }

        public override TaskResults ExecuteTask()
        {
            BDikaTestingClient client = new BDikaTestingClient("clientid", "clientkey");

            TestIteration testIteration = null;
            TaskResults results = new TaskResults();

            while (true)
            {
                try
                {
                    if (log.IsDebugEnabled) log.Debug("Creating client and calling GetNextTestQue");
                    testIteration = client.GetNextTestQue();
                }
                catch (TestQueEmptyException)
                {
                    if (log.IsDebugEnabled) 
                        log.Debug("Que is empty...");
                    
                    return results;
                }
                catch (TestingClientException e)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Received TestingClientException ", e);

                    results.Failed++;

                    return results;
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled)
                        log.Error("Received Exception ", ex);

                    results.Failed++;

                    return results;
                }
                if (testIteration == null)
                {
                    if (log.IsDebugEnabled)
                        log.Debug("Response was null.");

                    results.Failed++;
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
                    return results;
                }
                
                String testType = collectorsConfig.GetArgumentValue("test_type");

                if (String.IsNullOrEmpty(testType))
                {
                    if (log.IsErrorEnabled)
                        log.Error("No test_type!");

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

                    return results;
                }

                ITestRunner itr = null;

                runners.TryGetValue(testType.Trim().ToLower(), out itr);

                if (itr == null)
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

                    results.Failed++;
                    return results;
                }

                String testname = collectorsConfig.GetArgumentValue("test_name");

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

                    return results;

                }

                if (log.IsDebugEnabled)
                    log.Debug("Calling ITestRunner for " + testname);

                if (itr.RunTest(testIteration))
                {
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

            return results;
        }
    }
}
