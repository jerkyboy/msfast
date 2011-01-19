using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ServiceProcess;
using EYF.Tasks.Context;
using EYF.Tasks.Context.Listeners;
using log4net;

namespace BDika.Tasks.TestsExecuter
{   
    [RunInstaller(true)]
    public class TestsExecuterTaskInstaller : System.Configuration.Install.Installer
    {
        public TestsExecuterTaskInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            ServiceInstaller serviceAdmin = new ServiceInstaller();

            serviceAdmin.StartType = ServiceStartMode.Manual;
            serviceAdmin.ServiceName = "ITestsExecuterTask";
            serviceAdmin.DisplayName = "BDika Tests Collectors Service";

            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }

    public class Program : ServiceBase
    {
        private static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();

        private TestsExecuterTask TestsExecuterTask;

        public static void Main(String[] args)
        {
            TasksContext.InitContext();

            if (log.IsDebugEnabled) log.Debug("Starting ITestsExecuterTask...");
#if DEBUG
            new TestsExecuterTask().Start();
#else
            ServiceBase.Run(new ServiceBase[] { new Program() });
#endif

        }
        public Program()
        {
            this.ServiceName = "ITestsExecuterTask";
            this.AutoLog = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;

            this.TestsExecuterTask = new TestsExecuterTask();

            new NagiosLogTaskListener(this.TestsExecuterTask);
        }
        protected override void OnStart(string[] args)
        {
            this.TestsExecuterTask.Start();
        }
        protected override void OnStop()
        {
            this.TestsExecuterTask.Stop();
        }
    }
}