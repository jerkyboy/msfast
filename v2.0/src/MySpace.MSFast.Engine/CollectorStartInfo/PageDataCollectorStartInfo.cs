//=======================================================================
/* Project: MSFast (MySpace.MSFast.Engine)
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
using System.IO;
using System.Reflection;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using System.Windows.Forms;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.Engine.CollectorStartInfo
{
    public class PageDataCollectorStartInfo : CollectionMetaInfo
	{
        /// <summary>
        /// URL we are testing (url in the browser's navigation bar - no extra parameters)
        /// </summary>
        private String _url;
        public String URL
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != null && value.IndexOf("#") != -1) value = value.Substring(value.IndexOf("#"));
                this._url = value.Replace("\\", "/").Trim();
            }
        }

        /// <summary>
        /// URL of the page we are testing + the collection args attached to it
        /// (including the extra parameters for the collection + local response if any)
        /// </summary>
        private String _testURL;
        public String TestURL
        {
            get
            {
                if (String.IsNullOrEmpty(_testURL) == false)
                    return _testURL;

                return AppendCollectionArgs(this.URL);
            }
            set
            {
                this._testURL = value;
            }
        }

        /// <summary>
        /// URL we are launching in the browser with, 
        /// when we start the test
        /// </summary>
        public String FirstURL
        {
            get
            {
                String host = null;

                try
                {
                    Uri i = new Uri(URL);
                    host = i.Host;
                }
                catch
                {
                }

                if (String.IsNullOrEmpty(host))
                    host = "msfast.myspace.com";

                return String.Format("http://{0}?__PRE_COLLECTION={1}", host, Convert.ToBase64String(Encoding.UTF8.GetBytes(new Uri(TestURL).AbsoluteUri.ToString())));
            }
        }

        /// <summary>
        /// Test timeout (minutes)
        /// </summary>
        public int Timeout = 25;

        /// <summary>
        /// Proxy Port
        /// </summary>
        public int ProxyPort = 8080;
        
        /// <summary>
        /// Address of proxy
        /// </summary>
        public String ProxyAddress = null;
        
        /// <summary>
        /// Location of temp folder
        /// </summary>
        public String TempFolder = Directory.GetCurrentDirectory();

        /// <summary>
        /// Show make the browser visible when true
        /// </summary>
		public bool IsDebug = false;

        /// <summary>
        /// Print log to browser window
        /// </summary>
        public bool IsVerbose = false;		

        /// <summary>
        /// Location of proxy config files
        /// </summary>
        public String[] ConfigFiles = new String[] { Path.GetDirectoryName(Assembly.GetAssembly(typeof(PageDataCollector)).Location) + "\\conf\\SuProxy.default.config", 
                                                     Path.GetDirectoryName(Assembly.GetAssembly(typeof(PageDataCollector)).Location) + "\\conf\\SuProxy.msfast.config" };
        
        /// <summary>
        /// Clear browser cache when instantiate
        /// </summary>
		public bool ClearCache = false;

		
        public bool IsStartProxy { get { return (this.ProxyAddress == null); } }
		
		public virtual bool IsValid() { 
			return (	this.URL != null && 
						Timeout >= 1 && 
						(ProxyPort != -1 || String.IsNullOrEmpty(ProxyAddress) == false) &&
						String.IsNullOrEmpty(TempFolder) == false &&
						Directory.Exists(TempFolder));
		}
        
        protected String AppendCollectionArgs(String url)
        {
            return String.Concat(url, ((url.IndexOf("?") == -1) ? "?" : "&"), "__MSFASTTEST=1");
        }

		public PageDataCollectorStartInfo(String[] commandLineArguments){
			this.ParseCommandLineArgs(commandLineArguments);
		}

		public PageDataCollectorStartInfo(){

		}

		public virtual void Dispose() { }

		#region Command Line Parsing

		private static Dictionary<String, ArgumentsParser> ArgsParsers;
		delegate void ArgumentsParser(String name, String value, PageDataCollectorStartInfo si);

		static PageDataCollectorStartInfo()
		{
			ArgsParsers = new Dictionary<string, ArgumentsParser>();

			ArgsParsers.Add("/pp:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { try { si.ProxyPort = int.Parse(value); } catch { } }));
			ArgsParsers.Add("/t:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { try { si.Timeout = int.Parse(value); } catch { } }));
            ArgsParsers.Add("/u:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.URL = value.Replace("\\", "/"); }));
            ArgsParsers.Add("/uw:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si._testURL = value.Replace("\\", "/"); }));
            ArgsParsers.Add("/pa:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.ProxyAddress = value; }));
			ArgsParsers.Add("/debug", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.IsDebug = true; }));
			ArgsParsers.Add("/ci:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { try { si._collectionId = int.Parse(value); } catch { } }));
			ArgsParsers.Add("/clear-cache", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.ClearCache = true; }));
			ArgsParsers.Add("/verbose", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.IsVerbose = true; }));
            ArgsParsers.Add("/dump:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si._dumpFolder = value.Replace("\\", "/"); if (si._dumpFolder.EndsWith("/") == false) si._dumpFolder = String.Concat(si._dumpFolder, "/"); }));
            ArgsParsers.Add("/temp:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) { si.TempFolder = value.Replace("\\", "/"); if (si.TempFolder.EndsWith("/") == false) si.TempFolder = String.Concat(si.TempFolder, "/"); }));
            ArgsParsers.Add("/config:", new ArgumentsParser(delegate(String name, String value, PageDataCollectorStartInfo si) {si.ConfigFiles = value.Split(new String[]{"|"}, StringSplitOptions.RemoveEmptyEntries);}));
			
		}

		public virtual PageDataCollectorErrors PrepareStartInfo()
		{
			return PageDataCollectorErrors.NoError;
		}
		public virtual PageDataCollectorErrors CleanUp()
		{
			return PageDataCollectorErrors.NoError;
		}

		public string CreateCommandLineArgs()
		{
			StringBuilder sb = new StringBuilder();

            sb.Append(" /u:\"");
            sb.Append(this.URL);
            sb.Append("\" ");



            if (String.IsNullOrEmpty(this._testURL) == false)
            {
                sb.Append(" /uw:\"");
                sb.Append(this._testURL);
                sb.Append("\" ");
            }

			sb.Append(" /t:");
			sb.Append(this.Timeout);

			if (IsStartProxy)
			{
				sb.Append(" /pp:");
				sb.Append(this.ProxyPort);
			}
			else
			{
				sb.Append(" /pa:\"");
				sb.Append(this.ProxyAddress);
				sb.Append("\" ");
			}

			if (this.CollectionID != -1)
			{
				sb.Append(" /ci:");
				sb.Append(this.CollectionID);
			}

			if (this.IsDebug)
				sb.Append(" /debug ");

            if (this.IsVerbose)
                sb.Append(" /verbose ");

            if (this.DumpFolder != null)
			{
				sb.Append(" /dump:\"");
                sb.Append(this.DumpFolder.Replace("\\", "/"));
				sb.Append("\" ");
			}

			if (this.TempFolder != null)
			{
				sb.Append(" /temp:\"");
				sb.Append(this.TempFolder.Replace("\\","/"));
				sb.Append("\" ");
			}

			if (this.ClearCache)
			{
				sb.Append(" /clear-cache ");
			}

            if (this.ConfigFiles != null)
            {
                sb.Append(" /config:\"");
                foreach (String s in this.ConfigFiles)
                {
                    sb.Append(s);
                    sb.Append("|");
                }
                sb.Append("\"");
            }

			return sb.ToString();
		}

		private void ParseCommandLineArgs(String[] commandLineArguments)
		{
			if (commandLineArguments != null)
				foreach (String s in commandLineArguments)
				{
					foreach (String ak in ArgsParsers.Keys)
					{
						if (CheckAndPlaceValue(s, ak, ArgsParsers[ak]))
						{
							break;
						}
					}
				}
		}

		private bool CheckAndPlaceValue(string s, string ky, ArgumentsParser ap)
		{
			if (String.IsNullOrEmpty(s) || String.IsNullOrEmpty(ky))
				return false;

			if (s.Trim().ToLower().StartsWith(ky))
			{
				s = s.Substring(ky.Length);
				ap(ky, s, this);
				return true;
			}
			return false;
		}

		public static void PrintUsage(TextWriter tw)
		{
            tw.WriteLine("--------------------------------------------------------------------\r\n");
            tw.WriteLine(" MSFast Test Runner, MySpace.com (c) 2009\r\n");
            tw.WriteLine("--------------------------------------------------------------------\r\n");
			tw.Write(" Use:  engine.exe");
            tw.WriteLine(" /u:\"<URL>\"\r\n");
            tw.WriteLine("--------------------------------------------------------------------\r\n");
			tw.WriteLine(" Optional Parameters:");
			tw.WriteLine("");
			tw.WriteLine("    /pp:<Port>           Start internal proxy on port #<Port>");
			tw.WriteLine("                    	 (Default 8080)");
			tw.WriteLine("");
			tw.WriteLine("    /t:<Timeout>         Wait for <Timeout> minutes before exiting");
			tw.WriteLine("                         (Default 15 minutes)");
			tw.WriteLine("");
			tw.WriteLine("    /debug               Launch in DEBUG mode (Visible browser window)");
			tw.WriteLine("");
            tw.WriteLine("    /clear-cache         Clear browser cache\r\n");
            tw.WriteLine("--------------------------------------------------------------------");
        }

		#endregion

        #region CollectionMetaInfo Members
        
        private int _collectionId = 1;
        
        public int CollectionID
        {
            get { 
                return _collectionId; }
            set
            {
                _collectionId = value;
            }
        }
        
        private String _dumpFolder = Directory.GetCurrentDirectory();

        public string DumpFolder
        {
            get { 
                return _dumpFolder; }
            set
            {
                _dumpFolder = value;
            }
        }

        #endregion
    }
}
