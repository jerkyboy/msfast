//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Tasks.EmailsSender)
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
using System.ComponentModel;
using System.ServiceProcess;
using log4net;
using EYF.Tasks.Context;
using EYF.Tasks.Context.Listeners;
using EYF.Providers.ThridParty.Twitter;
using EYF.Core.Configuration;
using MySpace.MSFast.Automation.Providers.Tests.Browse;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using System.IO;

namespace MySpace.MSFast.Automation.Tasks.EmailsSender
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
            serviceAdmin.DisplayName = "MSFast Automation Email Service";

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
            NotificationTemplatesRepository.InitRepository(AppConfig.ConfigLocation(AppConfig.Instance["NotificationTemplates.config"]));
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

