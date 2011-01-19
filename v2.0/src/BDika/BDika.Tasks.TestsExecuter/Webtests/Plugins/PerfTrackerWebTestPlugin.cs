using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Net;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using System.Windows.Forms;

namespace BDika.Tasks.TestsExecuter.Webtests.Plugins
{
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
             MessageBox.Show("X");

             String collectorsConfig = "{\"Arguments\":[{\"Key\":\"login_username\",\"Value\":\"e_yadid@hotmail.com\"},{\"Key\":\"login_password\",\"Value\":\"QSZxaw1!\"},{\"Key\":\"test_type\",\"Value\":\"msfast\"},{\"Key\":\"test_name\",\"Value\":\"eyadid\"},{\"Key\":\"test_protocol\",\"Value\":\"http\"},{\"Key\":\"test_domain\",\"Value\":\"www.myspace.com\"},{\"Key\":\"test_path\",\"Value\":\"/eyadid\"},{\"Key\":\"test_query_string\",\"Value\":\"\"},{\"Key\":\"require_login\",\"Value\":\"true\"}],\"ScriptGroups\":null}";//System.Environment.GetEnvironmentVariable("collectorsConfig");
             String resultsID = "1";// System.Environment.GetEnvironmentVariable("resultsId"); 
             
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
    }
}
