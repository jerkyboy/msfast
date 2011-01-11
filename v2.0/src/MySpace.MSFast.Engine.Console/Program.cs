//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine.Console)
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
using System.Threading;
using System.Reflection;
using MySpace.MSFast.SuProxy.Proxy;
using System.IO;
using System.Windows.Forms;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Engine.BrowserWrapper;
using MySpace.MSFast.Engine.DataCollector;

using MySpace.MSFast.Core.UserExperience;

#if InternetExplorer
using MySpace.MSFast.SysImpl.Win32.InternetExplorer.TestBrowser;
using MySpace.MSFast.Engine.SuProxy.Proxy;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
#endif

namespace MySpace.MSFast.Engine.Console
{
	class Program
	{
        private static String[] commandArgs;

		[STAThread]
		static void Main(string[] args)
        {
            MessageBox.Show("X");
            commandArgs = args;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
			
            PageDataCollectorStartInfo pdcsi = new PageDataCollectorStartInfo(args);
            
			if (pdcsi.IsValid() == false)
			{
				PageDataCollectorStartInfo.PrintUsage(System.Console.Error);
				System.Environment.ExitCode = (int)(PageDataCollectorErrors.InvalidOrMissingArguments);
                return;
			}

            CollectorsConfig.Instance.AppendConfig(new XMLCollectorsConfigLoader());

            if (String.IsNullOrEmpty(pdcsi.CollectorScripts))
            {
                CollectorsConfig.Instance.AppendConfig(new XMLCollectorsConfigLoader(Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream("MySpace.MSFast.Engine.CollectorsConfig_DefaultScripts.config")));
            }
            else
            {
                CollectorsConfig.Instance.AppendConfig(new XMLCollectorsConfigLoader(pdcsi.CollectorScripts));
            }

            TestEvents.IsVerbose = pdcsi.IsVerbose;
            TestEvents.FireProgressEvent(TestEventType.TestStarted);
            
            pdcsi.IsDebug = true;

            _Collector c = new _Collector(pdcsi);

			try
			{
				System.Environment.ExitCode = c.Run();
			}
			catch {
				System.Environment.ExitCode = (int)PageDataCollectorErrors.Unknown;
			}
			finally
			{
				c.Release();
			}

            TestEvents.FireProgressEvent(TestEventType.TestEnded);
		}
     
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try{
                StringBuilder sb = new StringBuilder();
                sb.Append("Engine Failed. args: ");
                
                foreach(String s in commandArgs)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }

                ExceptionsHandler.HandleException(new Exception(sb.ToString(),(Exception)e.ExceptionObject));

            }catch
            {
            }
            System.Environment.ExitCode = (int)PageDataCollectorErrors.Unknown;
            System.Environment.Exit((int)PageDataCollectorErrors.Unknown);
        }

		private class _Collector{
			
			private String generatedTempFilename = null;
			private Mutex _processSync;
			private bool _owned = false;

			private static SuProxyServer proxy = null;
			private PageDataCollectorStartInfo startInfo = null;

			public _Collector(PageDataCollectorStartInfo startInfo)
			{
				this.startInfo = startInfo;
			}

			public int Run()
			{
				if (startInfo.IsValid() == false)
				{
					return (int)(PageDataCollectorErrors.InvalidOrMissingArguments);
				}

				_processSync = new Mutex(true, Assembly.GetAssembly(typeof(PageDataCollector)).GetName().Name, out _owned);

				if (_owned == false)
				{
					return (int)(PageDataCollectorErrors.TestAlreadyRunning);
				}

				//Start Proxy
				if (startInfo.IsStartProxy)
				{
                    EngineSuProxyConfiguration spc = EngineSuProxyConfiguration.Default;

                    if(startInfo.ConfigFiles == null)
                        return (int)PageDataCollectorErrors.InvalidConfiguration;

                    foreach (String s in startInfo.ConfigFiles)
                    {
                        if (s == null || File.Exists(s) == false)
                        {
                            return (int)PageDataCollectorErrors.InvalidConfiguration;
                        }
                    }

					spc.ProxyPort = startInfo.ProxyPort;
                    spc.DumpFolder = startInfo.DumpFolder;
                    spc.URL = startInfo.URL;
                    spc.CollectionID = startInfo.CollectionID;

                    spc.ConfigurationFiles = startInfo.ConfigFiles;

					try
					{
						proxy = new SuProxyServer(spc);
						proxy.Start();
					}
					catch
					{
						Release();
						return (int)(PageDataCollectorErrors.CantStartProxy);
					}

					startInfo.ProxyAddress = "http://localhost:" + spc.ProxyPort;

				}

                TestBrowser bw = null;

#if InternetExplorer
				try
				{
                    bw = new BrowserWrapperIEImpl();
					return (bw.StartTest(startInfo));
				}
				finally{
					if(bw != null)
						bw.Dispose();
				}
#else
                return 0;
#endif
			}

            private TestBrowser GetBrowserWrapper()
            {
                return null;
            }

			public void Release()
			{
				try
				{
					if(_processSync != null)
						_processSync.ReleaseMutex();

					_processSync = null;
				}
				catch 
				{
				}
				if (_owned)
				{
					
					_owned = false;
				}
				try
				{
					if(generatedTempFilename != null)
						File.Delete(generatedTempFilename);
				}
				catch { }
				if (proxy != null)
				{
					try
					{
						proxy.Stop();
					}
					catch 
					{ }
					try{
						proxy.Dispose();
					}
					catch 
					{ }
				}
				proxy = null;

			}			
		}			
    }
}
