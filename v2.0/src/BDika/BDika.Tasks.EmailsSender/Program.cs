using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using log4net;
using EYF.Tasks.Context;
using EYF.Tasks.Context.Listeners;

namespace BDika.Tasks.EmailsSender
{
    [RunInstaller(true)]
    public class EmailSenderTaskInstaller : System.Configuration.Install.Installer
    {
        public EmailSenderTaskInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            ServiceInstaller serviceAdmin = new ServiceInstaller();

            serviceAdmin.StartType = ServiceStartMode.Manual;
            serviceAdmin.ServiceName = "IIEmailSenderTask";
            serviceAdmin.DisplayName = "BDika Email Service";

            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }

    public class Program : ServiceBase
    {
        private static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();

        private EmailSenderTask EmailSenderTask;

        public static void Main(String[] args)
        {
            TasksContext.InitContext();

            if (log.IsDebugEnabled) log.Debug("Starting EmailSender...");
#if DEBUG
            new EmailSenderTask().Start();
#else
            ServiceBase.Run(new ServiceBase[] { new Program() });
#endif

        }
        public Program()
        {
            this.ServiceName = "IIEmailSenderTask";
            this.AutoLog = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;

            this.EmailSenderTask = new EmailSenderTask();

            new NagiosLogTaskListener(this.EmailSenderTask);
        }
        protected override void OnStart(string[] args)
        {
            this.EmailSenderTask.Start();
        }
        protected override void OnStop()
        {
            this.EmailSenderTask.Stop();
        }
    }
}