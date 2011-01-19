using System;
using System.Collections.Generic;
using System.Text;
using EYF.Tasks;
using BDika.Providers.Collectors;
using EYF.Core.Configuration;
using BDika.Entities.Tests;
using BDika.Entities.Collectors;
using BDika.Entities.Triggers;
using log4net;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace BDika.Tasks.EmailsSender
{
    public class EmailSenderTask : BaseLoopTask
    {
        public static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();
        public static String EmailTemplate = "";

        public EmailSenderTask()
        {
            if (log.IsDebugEnabled) log.Debug("Starting checking emails que");
            if (log.IsInfoEnabled) log.Info("Setting time interval to " + AppConfig.Instance["CheckInterval"]);

            base.Interval = long.Parse(AppConfig.Instance["CheckInterval"]);
        }

        public override TaskResults ExecuteTask()
        {
            bool b = false;

            Print();
            b = CollectorsConfigurationProvider.SetArgument(new TesterTypeID(1), "KEY", "TesterTypeID(1)");
            Print();
            b = CollectorsConfigurationProvider.SetArgument(new TestID(1), "KEY", "TestID(1)");
            Print();
            b = CollectorsConfigurationProvider.SetArgument(new TriggerID(1), "KEY", "TriggerID(1)");
            Print();
            b = CollectorsConfigurationProvider.SetArgument(new TestID(1), new TesterTypeID(1), "KEY", "TestID(1) TesterTypeID(1)");
            Print();
            b = CollectorsConfigurationProvider.SetArgument(new TriggerID(1), new TestID(1), new TesterTypeID(1), "KEY", "TriggerID(1) TestID(1) TesterTypeID(1)");
            Print();
            
            Print();
            b = CollectorsConfigurationProvider.RemoveArgument(new TriggerID(1), new TestID(1), new TesterTypeID(1), "KEY");
            Print();
            b = CollectorsConfigurationProvider.RemoveArgument(new TestID(1), new TesterTypeID(1), "KEY");
            Print();
            b = CollectorsConfigurationProvider.RemoveArgument(new TesterTypeID(1), "KEY");
            Print();
            b = CollectorsConfigurationProvider.RemoveArgument(new TestID(1), "KEY");
            Print();
            b = CollectorsConfigurationProvider.RemoveArgument(new TriggerID(1), "KEY");
            Print();
            Print();
            Print();
            return null;
        }
        private void Print()
        {
            ExtCollectorsConfig ec = CollectorsConfigurationProvider.GetExtCollectorsConfig(new TriggerID(1), new TestID(1), new TesterTypeID(1));

            
            if (ec != null)
            {
                CollectorsArgument[] cs = ec.GetAllArguments();

                if (cs != null)
                {
                    foreach (CollectorsArgument v in cs)
                    {
                        Console.WriteLine(v.Key + " - " + v.Value);// + " - " + v.IsOverride + " - " + v.OriginalValue);
                    }
                }
            }
        }
    }
}
