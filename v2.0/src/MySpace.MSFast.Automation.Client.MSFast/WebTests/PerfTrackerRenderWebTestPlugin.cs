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
using MySpace.MSFast.Engine.CollectorStartInfo;
using MySpace.MSFast.Engine;
using MySpace.MSFast.Engine.DataCollector;
using MySpace.MSFast.Automation.Client.MSFast.Utils;

namespace MySpace.MSFast.Automation.Client.MSFast.Webtests.Plugins
{
    /*
	public class PerfTrackerRenderWebTestPlugin : PerfTrackerWebTestPlugin
	{
        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            if (e.ResponseExists == false || e.Response.StatusCode != HttpStatusCode.OK || this.ResultsID == 0 || e.Request.RecordResult == false)
                return;

            Log(e.WebTest, "Running test with resultid " + this.ResultsID);

            CustomRequestPageDataCollectorStartInfo chr = new CustomRequestPageDataCollectorStartInfo();

            try
            {
                StringBuilder sb = new StringBuilder();
                if (e.Response != null && e.Response.Cookies != null)
                {
                    foreach (Cookie co in e.Response.Cookies)
                    {
                        sb.Append(co.Name);
                        sb.Append("=");
                        sb.Append(co.Value);
                        sb.Append(";");

                    }
                }
                if (e.Request != null && e.Request.Cookies != null)
                {
                    foreach (Cookie co in e.Request.Cookies)
                    {
                        sb.Append(co.Name);
                        sb.Append("=");
                        sb.Append(co.Value);
                        sb.Append(";");
                    }
                }
                chr.Cookie = sb.ToString();
            }
            catch
            {
            }
            if (e.Request.Method == "GET")
            {
                chr.Method = CustomRequestPageDataCollectorStartInfo.MethodType.GET;
            }
            else if (e.Request.Method == "POST")
            {
                chr.Method = CustomRequestPageDataCollectorStartInfo.MethodType.POST;

                if (e.Request.Body != null && e.Request.Body is FormPostHttpBody)
                {
                    foreach (FormPostParameter fpp in ((FormPostHttpBody)e.Request.Body).FormPostParameters)
                    {
                        chr.AddFormVariable(fpp.Name, fpp.Value);
                    }
                }
            }
            
            if (MSFastDefaultStartInfo.SetDefaultStartupInfo(chr, new Uri(e.Request.UrlWithQueryString), (int)this.ResultsID) == false)
            {
                e.WebTest.AddCommentToResult("Failed on setting default start info for render test");
                e.WebTest.Stop();
                return;
            }
            
            int result = new PageDataCollector().StartTest(chr);

            if (result != 0)
            {
                //For some reason this prop is read only
                //e.WebTest.Outcome = Outcome.Fail;
                String err = "Unknown";

                try
                {
                    err = Enum.ToObject(typeof(PageDataCollectorErrors), result).ToString();
                }
                catch { }
                e.WebTest.AddCommentToResult("Failed on Render Test! Error #" + result + " (" + err + ")");
                e.WebTest.Stop();
            }

            base.PostRequest(sender, e);
        }
	}*/
}
