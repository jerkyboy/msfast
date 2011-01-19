using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using log4net;
using EYF.Core.Logger;
using System.Net.Sockets;
using System.Net;
using EYF.Core;
using EYF.Providers.Gateway;
using EYF.Core.DataTypes;
using EYF.Entities.StandardObjects;
using EYF.Web.Common.Links;
using EYF.Web.Common.StaticFilesManagement;
using EYF.Web.Common.InputValidation;
using System.Reflection;
using log4net.Config;

namespace BDika.Web.Application
{
    public class Global : System.Web.HttpApplication
    {
        public static readonly ILog log = EYFLogManager.GetLogger();

        static bool init = false;
        static object initLock = new object();

        public void Application_End()
        {
            if (log.IsDebugEnabled)
            {
                HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime",
                                                                                                BindingFlags.NonPublic
                                                                                                | BindingFlags.Static
                                                                                                | BindingFlags.GetField,
                                                                                                null,
                                                                                                null,
                                                                                                null);

                if (runtime == null)
                    return;

                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage",
                                                                                 BindingFlags.NonPublic
                                                                                 | BindingFlags.Instance
                                                                                 | BindingFlags.GetField,
                                                                                 null,
                                                                                 runtime,
                                                                                 null);

                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack",
                                                                               BindingFlags.NonPublic
                                                                               | BindingFlags.Instance
                                                                               | BindingFlags.GetField,
                                                                               null,
                                                                               runtime,
                                                                               null);

                log.Debug(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
                                             shutDownMessage,
                                             shutDownStack));
            }
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            lock (initLock)
            {
                if (init == false)
                    init = true;

                else return;
            }

            XmlConfigurator.Configure();

            if (log.IsInfoEnabled) log.Info("Starting Application...");

#if DEBUG
            try //Office computer uses a 8080 proxy
            {
                TcpClient tc = new TcpClient();
                tc.Connect("localhost", 8080);
                tc.Close();
                WebRequest.DefaultWebProxy = new WebProxy("http://127.0.0.1:8080");
            }
            catch
            {
            }

#endif

            EYFContext.Init(Server.MapPath("~/bin/Configuration/"));
            PopObjectsRepository.Init();
            State.Init();
            Country.Init();
            ZipCode.Init();
            LinksManager.Init();
            StaticBundleManager.Init();
            ClientSideValidationManager.Init();

            if (log.IsDebugEnabled) log.Debug("Done Starting Application!");

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {



        }
    }
}