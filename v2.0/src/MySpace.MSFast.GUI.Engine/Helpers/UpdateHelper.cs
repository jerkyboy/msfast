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
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using System.Net;
using System.IO;
using System.Threading;
using MySpace.MSFast.Core.Configuration.CommonDataTypes;

namespace MySpace.MSFast.GUI.Engine.Helpers
{
    public class UpdateHelper
    {
        public delegate void GetLatestVersionDetailsCallback(String ver, String desc, String get_url, DateTime versionDate);
        
        private static String latestVersion = null;
        private static String latestVersionDetails = null;
        private static String latestVersionURL = null;
        private static DateTime latestVersionDate = DateTime.MinValue;

        private static DateTime nextCheck = DateTime.Now;

        public static void GetLatestVersionDetails(GetLatestVersionDetailsCallback callback)
        {
            new Thread(_GetLatestVersionDetails).Start(callback);
        }


        private static void _GetLatestVersionDetails(object callback)
        {
            if (callback is GetLatestVersionDetailsCallback)
            {
                if (latestVersionDetails == null || latestVersion == null || DateTime.Now > nextCheck)
                {
                    RefreshVesion();
                }
                if (callback != null)
                {
                    ((GetLatestVersionDetailsCallback)callback)(latestVersion, latestVersionDetails, latestVersionURL, latestVersionDate);
                }
            }
        }

        private static void RefreshVesion()
        {
            IConfigGetter getter = ConfigProvider.Instance.GetConfigGetter("MSFast.Global");
            String url_s = getter.GetString(MSFastGlobalConfigKeys.VERSION_UPDATE_URL);

            String version = null;
            DateTime versionDate = DateTime.MinValue;

            String versionDesc = null;
            String versionURL = null;
            if (String.IsNullOrEmpty(url_s) == false)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_s);
                    // execute the request
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    // we will read data via the response stream
                    StreamReader resStream = new StreamReader(response.GetResponseStream());

                    try
                    {
                        version = resStream.ReadLine();
                        string versionDateValue = resStream.ReadLine();
                        if (!DateTime.TryParse(versionDateValue, out versionDate))
                        {

                        }

                        versionURL = resStream.ReadLine();
                        versionDesc = resStream.ReadToEnd();
                    }
                    catch { }

                    resStream.Close();
                    response.Close();
                }
                catch
                {
                }
            }
            latestVersionURL = versionURL;
            latestVersionDetails = versionDesc;
            latestVersion = version;
            latestVersionDate = versionDate;
            nextCheck = DateTime.Now.AddHours(2);
        }
    }
}
