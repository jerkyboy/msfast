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
            
            return null;
        }
        
    }
}
