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
using log4net;
using EYF.Tasks.Context;
using MySpace.MSFast.Automation.Client.API;
using log4net.Config;
using EYF.Core;
using MySpace.MSFast.Automation.Client.MSFast.Utils;
using System.ComponentModel;
using System.ServiceProcess;
using EYF.Core.Configuration;
using System.Net.Sockets;
using System.Net;

namespace MySpace.MSFast.Automation.Client.MSFast
{
    [RunInstaller(true)]
    public class MSFastAutomationClientInstaller : System.Configuration.Install.Installer
    {
        public MSFastAutomationClientInstaller()
        {
            ServiceProcessInstaller process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;

            ServiceInstaller serviceAdmin = new ServiceInstaller();

            serviceAdmin.StartType = ServiceStartMode.Manual;
            serviceAdmin.ServiceName = "MSFastAutomationClient";
            serviceAdmin.DisplayName = "MSFastAutomation Client Service";

            Installers.Add(process);
            Installers.Add(serviceAdmin);
        }
    }

    public class MSFastAutomationMSFastClient : AbsClient
    {
        public override bool Start()
        {
            base.Init();
            base.AddTestRunner("msfast", new DirectLinkTestRunner());
            return base.Start();
        }
    }

    public class Program : ServiceBase
    {
        private static readonly ILog log = EYF.Core.Logger.EYFLogManager.GetLogger();

        private MSFastAutomationMSFastClient MSFastAutomationMSFastClient;

        public static void Main(String[] args)
        {
            XmlConfigurator.Configure();
            MSFastDefaultStartInfo.Init();

            if (log.IsDebugEnabled)
                log.Debug("Starting MSFastAutomation for MSFast...");



            MSFastAutomationMSFastClient client = new MSFastAutomationMSFastClient();
            client.PrintToConsole = true;
            client.Start();

            //ServiceBase.Run(new ServiceBase[] { new Program() });

        }        

        public Program()
        {
            this.ServiceName = "MSFastAutomationClient";
            this.AutoLog = true;
            this.CanPauseAndContinue = false;
            this.CanShutdown = false;
            this.CanStop = true;

            this.MSFastAutomationMSFastClient = new MSFastAutomationMSFastClient();
        }
        protected override void OnStart(string[] args)
        {
            this.MSFastAutomationMSFastClient.Start();
        }
        protected override void OnStop()
        {
            this.MSFastAutomationMSFastClient.Stop();
        }
    }

}