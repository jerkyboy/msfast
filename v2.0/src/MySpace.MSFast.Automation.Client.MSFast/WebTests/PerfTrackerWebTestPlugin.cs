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
using System.Text;
//using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Net;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using System.Windows.Forms;

namespace MySpace.MSFast.Automation.Client.MSFast.Webtests.Plugins
{
    /*
    public class PerfTrackerWebTestPlugin : WebTestPlugin
    {
		 public CollectorsConfig CollectorsConfig;
         public uint ResultsID;

		 internal void Log(WebTest testRef, string message)
		 {
			 testRef.AddCommentToResult(message);
			 Console.Out.WriteLine(message);
		 }

         public override void PreWebTest(object sender, PreWebTestEventArgs e)
         {
#if DEBUG
             MessageBox.Show("Attatch Webtest");
#endif
             String collectorsConfig = System.Environment.GetEnvironmentVariable("collectorsConfig");
             String resultsID = System.Environment.GetEnvironmentVariable("resultsId"); 
             
             if(String.IsNullOrEmpty(collectorsConfig) || String.IsNullOrEmpty(resultsID)){
                 Log(e.WebTest, "INVALID CONFIG!");
                 return;
             }
             
             this.ResultsID = uint.Parse(resultsID);
             
             this.CollectorsConfig = new CollectorsConfig();
             this.CollectorsConfig.AppendConfig(new JSONCollectorsConfigLoader(collectorsConfig));

             String testProtocol = CollectorsConfig.GetArgumentValue("test_protocol");
             String testDomain = CollectorsConfig.GetArgumentValue("test_domain");
             String testPath = CollectorsConfig.GetArgumentValue("test_path");
             String testQueryString = CollectorsConfig.GetArgumentValue("test_query_string");

             Uri testUri = new Uri(String.Concat(testProtocol, "://", testDomain, testPath, testQueryString));

             e.WebTest.Context["DefaultUser_Email"] = this.CollectorsConfig.GetArgumentValue("login_username");
             e.WebTest.Context["DefaultUser_Password"] = this.CollectorsConfig.GetArgumentValue("login_password");
             e.WebTest.Context["DefaultUser_UserID"] = "1";
             e.WebTest.Context["TestURL"] = testUri.ToString();
             
             base.PreWebTest(sender, e);
         }
    }*/
}
