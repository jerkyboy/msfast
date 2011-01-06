//=======================================================================
/* Project: MSFast (MySpace.MSFast.GUI.Engine)
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
using System.Windows.Forms;
using System.IO;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Performance;
using System.Xml;
using MySpace.MSFast.DataValidators;
using System.Reflection;
using MySpace.MSFast.DataProcessors.PageSource;
using MySpace.MSFast.GUI.Engine.Panels.Status;
using MySpace.MSFast.GUI.Engine.Helpers;
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.Events;
using MySpace.MSFast.Core.UserExperience;
using MySpace.MSFast.Core.Configuration.CommonDataTypes;
using MySpace.MSFast.GUI.Configuration.MSFast;
using MySpace.MSFast.GUI.Engine.DataCollector;
using MySpace.MSFast.Engine.BrowserWrapper;
using MySpace.MSFast.DataProcessors.DataValidators.ValidationResultTypes;
using MySpace.MSFast.Core.Logger;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.ImportExportsMgrs;
using MySpace.MSFast.DataProcessors.Markers;

namespace MySpace.MSFast.GUI.Engine.Panels
{
	public partial class MSFastMainPanel : Panel
	{
        private static readonly MySpace.MSFast.Core.Logger.MSFastLogger log = MSFastLogger.GetLogger(typeof(MSFastMainPanel));

		private AsyncBufferPageDataCollector pageDataCollector = null;
		private Browser browser = null;
		private bool isRunning = false;
		private int proxyRangeOffset = 0;
		private ValidationRunner validationRunner = null;

        private ProcessedDataPackage currentPackage = null;

        public MSFastMainPanel(Browser b)
            : this(b, null)
        {
            
        }

        public MSFastMainPanel(Browser b, String loadData) 
		{
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (b != null)
            {
                this.browser = b;
                this.browser.OnBrowserStateChanged += new BrowserStateChanged(browser_OnBrowserStateChanged);
            }
            
            InitializeComponent();

            SetTestRunning(false);
            ResetUpdate();

            if (String.IsNullOrEmpty(loadData) == false)
            {
                LoadCollection(loadData);
            }
		}

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ExceptionsHandler.HandleException((Exception)e.ExceptionObject);
            }
            catch
            {
            }

            DialogResult dr = MessageBox.Show("An unexpected error has occurred.\r\nWould you like to report this error?\r\n\r\n(private data will NOT be sent!)", "Error Detected!", MessageBoxButtons.YesNo);
            
            if (dr == DialogResult.Yes)
            {
                ExceptionsHandler.FlushExceptions();
            }
        }

        private void ResetUpdate()
        {
            this.updatesReadyBtn.Visible = false;
            this.toolStripSeparator5.Visible = false;
            UpdateHelper.GetLatestVersionDetails(new UpdateHelper.GetLatestVersionDetailsCallback(this.UpdateAvailableCallback));
        }

        private delegate void updateAvailableCallback(String ver, String desc, String getUrl, DateTime versionDate);
        private void UpdateAvailableCallback(String ver, String desc, String getUrl, DateTime versionDate)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new updateAvailableCallback(this.UpdateAvailableCallback), new object[] { ver, desc, getUrl, versionDate });
                return;
            }

            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");
            IConfigSetter setter = ConfigProvider.Instance.GetConfigSetter("MSFast.Global");

            if (String.IsNullOrEmpty(ver) || getter.GetString(MSFastGlobalConfigKeys.CURRENT_PACKAGE_VERSION) == ver)
            {
                this.updatesReadyBtn.Visible = false;
                this.toolStripSeparator5.Visible = false;

            }
            else
            {
                if (getter.GetString(MSFastGlobalConfigKeys.CURRENT_PACKAGE_VERSION_LATEST_ALERT) != ver)
                {
                    setter.SetString(MSFastGlobalConfigKeys.CURRENT_PACKAGE_VERSION_LATEST_ALERT, ver);
                    OpenNewVersionAvailable();
                }

                this.updatesReadyBtn.Visible = true;
                this.toolStripSeparator5.Visible = true;
            }
        }

		void browser_OnBrowserStateChanged(Browser browser, BrowserStatus state)
		{
			SetTestRunning(isRunning);
		}

		protected override void Dispose(bool f)
		{
			base.Dispose(f);
			this.graphViewPanel.SetResults(null,null);
			this.validationResultsViewPanel.SetResults(null);
			this.browser = null;
            this.currentPackage = null;
		}

		void tm_OnTestEnded(AsyncBufferPageDataCollector sender,PageDataCollectorStartInfo settings, bool success, PageDataCollectorErrors errCode, int resultsId)
		{
            if (success == false && errCode != PageDataCollectorErrors.TestAborted)
            {
                ExceptionsHandler.HandleException(new Exception(String.Format("Test Failed. Error #{0}  Command Args \"{1}\"",errCode,((settings != null && settings.CreateCommandLineArgs() != null) ? settings.CreateCommandLineArgs() : ""))));
            }
            if (this.pageDataCollector != null)
			{
				this.pageDataCollector.Dispose();
			}
			this.pageDataCollector = null;

			if (success == false)
			{
				SetTestRunning(false);

                SetTestStatus(TestEventType.TestEnded, success, errCode, resultsId);

                int[] pp = GetProxyPorts();
                if (pp == null)
                {
                    return;
                }

                if (errCode == PageDataCollectorErrors.CantStartProxy && proxyRangeOffset < pp.Length - 1)
                {
                    proxyRangeOffset++;
                    StartTest();
                }
                else if(errCode != PageDataCollectorErrors.TestAborted)
                {
                    if (errCode == PageDataCollectorErrors.InvalidConfiguration)
                    {
                        MessageBox.Show("Invalid configuration detected.\r\nPlease make sure the settings are correct", "Test Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (errCode == PageDataCollectorErrors.TestAlreadyRunning)
                    {
                        MessageBox.Show("Test is already running on a different page.","Test Aborted",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                    else if (errCode == PageDataCollectorErrors.TestTimeout)
                    {
                        MessageBox.Show("Test has timed-out.", "Test Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Unexpected error occured while trying to test the page\r\n(err#" + ((int)errCode) +")", "Test Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
			}
			else
			{
				ProcessResults(resultsId);
			}
		}

		void tm_OnStartingTest(AsyncBufferPageDataCollector sender)
		{
			SetTestRunning(true);

            SetTestStatus(TestEventType.StartingTest);
        }

        #region Popups

        private object openFormLock = new object();

        #region About Us Popup
        
        private void OpenAboutUs()
        {
            lock (openFormLock)
            {
                foreach (Form opnFrm in Application.OpenForms)
                {
                    if (opnFrm.Name == "MSFastAboutUsPanel")
                    {
                        opnFrm.Focus();
                        return;
                    }
                }

                Form m = new AboutUs();
                m.SuspendLayout();
                m.Name = "MSFastAboutUsPanel";
                m.Text = "About Us";
                m.ResumeLayout(false);
                m.PerformLayout();
                m.ShowDialog();
            }
        }

        #endregion

        #region Config Popup

		private void OpenConfigWindow() 
		{
            lock (openFormLock)
            {
                foreach (Form opnFrm in Application.OpenForms)
                {
                    if (opnFrm.Name == "MSFastConfigPanel")
                    {
                        opnFrm.Focus();
                        return;
                    }
                }

                Form m = new MSFastForm();

                MSFastGlobalConfigPanel p = new MSFastGlobalConfigPanel();
                p.Dock = DockStyle.Fill;
                m.FormBorderStyle = FormBorderStyle.FixedDialog;
                m.MaximizeBox = false;
                m.MinimizeBox = false;
                m.SuspendLayout();
                m.Size = new System.Drawing.Size(393,326);
                m.Controls.Add(p);
                m.Name = "MSFastConfigPanel";
                m.Text = "Options";
                m.ResumeLayout(false);
                m.PerformLayout();
                m.ShowDialog();
            }
        }
        #endregion

        #region New Version Popup

        private void OpenNewVersionAvailable()
        {
            lock (openFormLock)
            {
                foreach (Form opnFrm in Application.OpenForms)
                {
                    if (opnFrm.Name == "MSFastNewVersionPanel")
                    {
                        opnFrm.Focus();
                        return;
                    }
                }

                Form m = new UpdateAvailable();
                m.SuspendLayout();
                m.Name = "MSFastNewVersionPanel";
                m.Text = "Available Updates";
                m.ResumeLayout(false);
                m.PerformLayout();
                m.Show();
            }
        }

        #endregion        
   
        #endregion

        #region Test Control

        private void AbortTest() 
		{
            SetTestStatus(TestEventType.AbortingTest);

			if (pageDataCollector != null)
			{
				pageDataCollector.AbortTest();
			}
	
		}

		private void StartTest()
		{
			if (isRunning)
				return;

			if (this.browser == null || this.browser.State != BrowserStatus.Ready)
			{
                throw new Exception("Invalid Browser");
			}

			if (pageDataCollector != null)
			{
				return;
			}

			String url = browser.URL;

            if (String.IsNullOrEmpty(url))
            {
                throw new Exception("Invalid Browser");
            }
            if (url.ToLower().StartsWith("http:") == false)
            {
                MessageBox.Show("Tests are currently available for pages with an \"http://\" address only...","Sorry...",MessageBoxButtons.OK,MessageBoxIcon.Information );
                return;
            }

			pageDataCollector = new AsyncBufferPageDataCollector();
			pageDataCollector.OnStartingTest += new AsyncBufferPageDataCollector.StartingTestEventHandler(tm_OnStartingTest);
			pageDataCollector.OnTestEnded += new AsyncBufferPageDataCollector.TestEndedEventHandler(tm_OnTestEnded);
            pageDataCollector.OnTestEvent +=new OnTestEventHandler(pageDataCollector_OnTestEvent); 
			IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");

			if (getter == null ||
				String.IsNullOrEmpty(getter.GetString(MSFastGlobalConfigKeys.TEMP_FOLDER)))
			{

				tm_OnTestEnded(pageDataCollector, null,false, PageDataCollectorErrors.InvalidConfiguration, -1);
			}

			if (browser.GetBuffer(new Browser.GetBufferCallback(delegate(String buffer)
			{
				if (pageDataCollector != null && String.IsNullOrEmpty(buffer) == false && String.IsNullOrEmpty(url) == false)
				{
                    int[] pp = GetProxyPorts();

                    if (pp == null)
                    {
                        tm_OnTestEnded(pageDataCollector, null, false, PageDataCollectorErrors.InvalidOrMissingArguments, -1);
                        return;
                    }

                    if (proxyRangeOffset >= pp.Length)
					{
						proxyRangeOffset = 0;
					}

					BufferedPageDataCollectorStartInfo b = new BufferedPageDataCollectorStartInfo();
					b.URL = url;
					b.CollectionID = Math.Max(1, getter.GetInt(MSFastGlobalConfigKeys.LAST_COLLECTION_ID));

					IConfigSetter setter = ConfigProvider.Instance.GetConfigSetter("MSFast.Global");
					setter.SetInt(MSFastGlobalConfigKeys.LAST_COLLECTION_ID, b.CollectionID+1);

                    String tempFolder = getter.GetString(MSFastGlobalConfigKeys.TEMP_FOLDER);

                    TryCleanTempFolder(tempFolder);

                    b.Buffer = buffer;
                    b.ClearCache = getter.GetBoolean(MSFastGlobalConfigKeys.CLEAR_CACHE_BEFORE_TEST);
                    b.TempFolder = tempFolder;
                    b.DumpFolder = tempFolder;
                    b.ProxyPort = GetProxyPorts()[proxyRangeOffset];
					b.IsDebug = false;

					pageDataCollector.StartTest(b);
				}
				else
				{
                    tm_OnTestEnded(pageDataCollector, null, false, PageDataCollectorErrors.InvalidOrMissingArguments, -1);
				}

			})) == false)
			{
                tm_OnTestEnded(pageDataCollector, null, false, PageDataCollectorErrors.Unknown, -1);
			}
        }

        #endregion

        void pageDataCollector_OnTestEvent(TestEventType progressEventType, int progress, int total, string url)
        {
            SetTestStatus(progressEventType, progress, total, url);
        }


		private void ProcessResults(int collectionID)
		{
            SetTestStatus(TestEventType.ProcessingResults);
			
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");
			
			if(getter == null)
			{
				SetTestRunning(false);
                SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.InvalidConfiguration, -1);
			}

            String DumpFolder = getter.GetString(MSFastGlobalConfigKeys.DUMP_FOLDER);
            
            ProcessedDataPackage package = ProcessedDataCollector.CollectAll(DumpFolder, collectionID);

			if (package == null || package.Count == 0)
			{
				SetTestRunning(false);
                SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.Unknown, -1);
				return;
            }

            ProcessResults(package);

        }

        private void ProcessResults(ProcessedDataPackage package)
        {
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");

            if (getter == null)
            {
                SetTestRunning(false);
                SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.InvalidConfiguration, -1);
            }

            #region Collected Data
            
            String graphResults = null;

            if (getter.GetBoolean(MSFastGlobalConfigKeys.PAGE_GRAPH))
            {
                

                if (package.ContainsKey(typeof(DownloadData)) != false ||
                    package.ContainsKey(typeof(MarkersData)) != false ||
                    package.ContainsKey(typeof(PerformanceData)) != false)
                {
                    SerializedResultsFilesInfo srfi = new SerializedResultsFilesInfo(package);

                    if (String.IsNullOrEmpty(srfi.GetFolderNameAndCheckIfValid()))
                    {
                        SetTestRunning(false);
                        SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.InvalidConfiguration, -1);
                        return;
                    }

                    package.ThumbnailsRoot = "file://" + srfi.GetFolderNameAndCheckIfValid();

                    XmlDocument x = package.Serialize();

                    if (x == null)
                    {
                        SetTestRunning(false);
                        SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.Unknown, -1);
                        return;
                    }

                    try
                    {
                        x.Save(srfi.GetFullPath());
                        graphResults = srfi.GetFullPath();
                    }
                    catch
                    {
                        SetTestRunning(false);
                        SetTestStatus(TestEventType.TestEnded, false, PageDataCollectorErrors.Unknown, -1);
                        return;
                    }
                }
            }
            #endregion

            #region Validation

            ValidationResultsPackage validationResults = null;

            if (getter.GetBoolean(MSFastGlobalConfigKeys.PAGE_VALIDATION))
            {
                if (validationRunner == null)
                {
                    CreateValidationRunner();
                }

                if (validationRunner != null)
                {
                    validationResults = validationRunner.ValidateBlocking(package);
                }
            }

            #endregion

            SetTestRunning(false);
            SetTestStatus(TestEventType.TestEnded, true);

            ShowOutcome(graphResults, validationResults, package);
		}

		private void CreateValidationRunner()
		{
			this.validationRunner = new ValidationRunner();
            string location=null;
			try
			{
                location = "DefaultPageValidation.xml";
				location = (Path.GetDirectoryName(Assembly.GetAssembly(typeof(MSFastMainPanel)).Location).Replace("\\", "/") + "/conf/" + location);
				this.validationRunner.LoadFromFile(location);
			}
			catch(Exception ex) 
            {
                if (log.IsErrorEnabled)
                {
                    log.ErrorFormat("Exception encountered while processing \"{0}\"", location);
                    log.Error(ex);
                }
			}

            try
            {
                string AssemblyPath = Path.GetDirectoryName(typeof(MSFastMainPanel).Assembly.Location);
                string pluginsFolder = Path.Combine(AssemblyPath, "Plugins");
                this.validationRunner.LoadFromPluginsFolder(pluginsFolder);
            }
            catch(Exception ex)
            {
                if (log.IsErrorEnabled) log.Error("Exception Thrown loading Plugins", ex); 
            }
        }

        #region Panels Switch
        
        private void ShowValidationResultsPanel()
		{
			this.testStatusPanel.Visible = false;
			this.validationResultsViewPanel.Visible = true;
			this.graphViewPanel.Visible = false;
		}

		private void ShowGraphPanel()
		{
			this.testStatusPanel.Visible = false;
			this.validationResultsViewPanel.Visible = false;
			this.graphViewPanel.Visible = true;
		}

		private void ShowTestStatusPanel()
		{
			this.testStatusPanel.Visible = true;
			this.validationResultsViewPanel.Visible = false;
			this.graphViewPanel.Visible = false;
            this.testStatusPanel.Refresh();
        }
        
        #endregion

        private delegate void testRunning(bool p);
        private delegate void setTestStatus(TestEventType status, params object[] args);
        private delegate void showOutcome(String graphResults, ValidationResultsPackage validationResults, ProcessedDataPackage package);

        private void ShowOutcome(String graphResults, ValidationResultsPackage validationResults, ProcessedDataPackage package)
		{
			if (this.InvokeRequired)
			{
                this.BeginInvoke(new showOutcome(this.ShowOutcome), new object[] { graphResults, validationResults,package  });
				return;
			}
            
            this.currentPackage = package;

            if (String.IsNullOrEmpty(graphResults) == false)
            {
                this.saveCollectionBtn.Enabled = true;
                ShowGraphPanel();
            }
            else if (validationResults != null && validationResults.Count > 0)
            {
                this.saveCollectionBtn.Enabled = true;
                ShowValidationResultsPanel();
            }
            else 
            {
                MessageBox.Show("No results found!");
            }

            this.pageGraphBtn.Enabled = String.IsNullOrEmpty(graphResults) == false;
			this.validationResultsBtn.Enabled = (validationResults != null && validationResults.Count > 0);

            this.graphViewPanel.SetResults(graphResults, package );
			this.validationResultsViewPanel.SetResults(validationResults);
		}

		private void SetTestRunning(bool p)
		{
            this.currentPackage = null;

			isRunning = p;
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new testRunning(this.SetTestRunning),new object[] { p });
				return;
			}
			
			ShowTestStatusPanel();

			this.pageGraphBtn.Enabled = false;
			this.validationResultsBtn.Enabled = false;
            this.graphViewPanel.SetResults(null, null);
			this.validationResultsViewPanel.SetResults(null);

			this.abortCollectingDataBtn.Enabled = p;

            if (isRunning)
            {
                this.saveCollectionBtn.Enabled = false;
                this.loadCollectionBtn.Enabled = (this.browser == null);
                this.startCollectingDataBtn.Enabled = false;
                this.configCollectingDataBtn.Enabled = false;
            }
            else if (browser != null && browser.State == BrowserStatus.Ready)
            {
                this.loadCollectionBtn.Enabled = true;
                this.startCollectingDataBtn.Enabled = true;
                this.configCollectingDataBtn.Enabled = true;
            }
            else
            {
                this.saveCollectionBtn.Enabled = false;
                this.loadCollectionBtn.Enabled = (this.browser == null);
                this.startCollectingDataBtn.Enabled = false;
                this.configCollectingDataBtn.Enabled = false;
            }

		}

        private void SetTestStatus(TestEventType status, params object[] args) 
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new setTestStatus(this.SetTestStatus), new object[] { status, args });
				return;
			}
			this.testStatusPanel.SetTestStatus(status, args);
        }

        #region Save/Load

        private void SaveCollection()
        {
            if (this.currentPackage == null)
            {
                MessageBox.Show("An unexpected error has occurred", "Error Detected!");
                return;
            }

            ImportExportManager iem = null;

            Stream myStream;
            
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "MSFast .MSF File (*.msf)|*.msf|XML File (*.xml)|*.xml|HTTP Archive v1.1 .HAR File (*.har)|*.har";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    if (saveFileDialog1.FileName.ToLower().EndsWith("xml"))
                    {
                        iem = new XMLImportExportManager();
                    }
                    else if (saveFileDialog1.FileName.ToLower().EndsWith("har"))
                    {
                        iem = new HARImportExportsManager();

                    }
                    else
                    {
                        iem = new MSFImportExportsManager();
                    }

                    try
                    {
                        iem.SaveProcessedDataPackage(myStream, this.currentPackage);
                    }
                    catch
                    {
                        MessageBox.Show("An unexpected error has occurred", "Error Detected!");
                    }
                    finally{
                        myStream.Flush();
                        myStream.Close();
                        myStream.Dispose();
                    }
                }
            }

        }

        private void LoadCollection(String filename)
        {
            try
            {
                LoadCollection(File.Open(filename, FileMode.Open));
            }
            catch
            {
                MessageBox.Show("Invalid MSF File!", "Error Detected!");
            }
        }
        
        private void LoadCollection()
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "msf files|*.msf";
            openFileDialog1.Title = "Select a saved MSFast File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    LoadCollection(myStream);
                }
            }


        }

        private void LoadCollection(Stream myStream)
        {
            MSFImportExportsManager msfHandler = new MSFImportExportsManager();

            try
            {
                ProcessedDataPackage pdd = msfHandler.LoadProcessedDataPackage(myStream);

                if (pdd == null)
                {
                    throw new FileNotFoundException();
                }

                ProcessResults(pdd);

            }
            catch
            {
                MessageBox.Show("Invalid MSF File!", "Error Detected!");
            }
            finally
            {
                myStream.Close();
                myStream.Dispose();
            }
        }
        #endregion


        private static Regex portsRegex = new Regex("([0-9]*)");
        private int[] GetProxyPorts()
        {
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");
            String ports = getter.GetString(MSFastGlobalConfigKeys.DEFAULT_PROXY_PORT);
            
            if(String.IsNullOrEmpty(ports))
            {
                ports = "8080";
            }

            MatchCollection mc = portsRegex.Matches(ports);

            if (mc.Count > 0)
            {
                LinkedList<int> ll = new LinkedList<int>();

                foreach (Match m in mc)
                {
                    if (String.IsNullOrEmpty(m.Value) == false)
                    {
                        try
                        {
                            ll.AddLast(int.Parse(m.Value));
                        }
                        catch { }
                    }
                }
                int[] l = new int[ll.Count];
                int i = 0;
                foreach (int inn in ll)
                {
                    l[i] = inn;
                    i++;
                }
                if(l.Length>0)
                    return l;
            }


            MessageBox.Show("Invalid configuration detected.\r\nInvalid proxy ports!", "Test Aborted", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }


        private void TryCleanTempFolder(string tempFolder)
        {
            if (String.IsNullOrEmpty(tempFolder) || Directory.Exists(tempFolder) == false)
                return;

            long s = GetDirectorySize(tempFolder);
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");
            int max = Math.Min(1024, (Math.Max(16, getter.GetInt(MSFastGlobalConfigKeys.DUMP_MAX_SIZE))));
            max = max * 1024 * 1024;
            if (s > max)
            {
                string[] a = Directory.GetFiles(tempFolder, "*.*", SearchOption.TopDirectoryOnly);
                try
                {
                    foreach (string name in a)
                    {
                        File.Delete(name);
                    }
                }
                catch
                {
                }
            }

        }
   
        private static long GetDirectorySize(string p)
        {
            string[] a = Directory.GetFiles(p, "*.*", SearchOption.TopDirectoryOnly);
            long b = 0;
            foreach (string name in a)
            {
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            return b;
        }

	}
}
