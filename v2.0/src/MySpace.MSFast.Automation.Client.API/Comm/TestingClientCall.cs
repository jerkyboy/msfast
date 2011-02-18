//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.API)
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
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace MySpace.MSFast.Automation.Client.API.Comm
{
    public abstract class TestingClientCall<T> where T : ServerResponse
    {
        private Dictionary<String, String> formParams = new Dictionary<String, String>();
        public bool IsRequestWithFile = false;
        
        public virtual T ExecuteCall(String baseDomain, String clientID, String clientKey, int timeout)
        {
            AppendParam("client_id", clientID);
            AppendParam("client_key", clientKey);

            PrepareArguments();

            Uri uri = GetURL(baseDomain);

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;            
            Stream responseStream = null;

            T responseDic = null;

            try
            {
                request = (HttpWebRequest) WebRequest.Create(uri);
                request.Timeout = timeout;
                request.Method = "POST";
                request.KeepAlive = true;

                if (IsRequestWithFile == false)
                {
                    MakeRequest(request);
                }
                else
                {
                    MakeFileRequest(request);
                }
                
                response = (HttpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);

                responseDic = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());

                if (responseDic != null)
                    responseDic.Deserialize();

                return responseDic;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (responseStream != null)
                    responseStream.Close();
            }
        }

        private void MakeFileRequest(HttpWebRequest request)
        {
            Stream requestStream = null;

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string headerTemplate = "Content-Disposition: form-data; name=\"clientUpload\"; filename=\"clientUpload\"\r\nContent-Type: {0}\r\n\r\n";

            try
            {
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                requestStream = request.GetRequestStream();

                byte[] buffer = null;
            
                //Write Parameters
                foreach (String k in formParams.Keys)
                {
                    //Write 1st boundry
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                    buffer = System.Text.Encoding.UTF8.GetBytes(String.Format(formdataTemplate, k, formParams[k]));
                    requestStream.Write(buffer, 0, buffer.Length);
                }

                //Write 1st boundry
                requestStream.Write(boundarybytes, 0, boundarybytes.Length);

                //Write file header
                buffer = System.Text.Encoding.UTF8.GetBytes(String.Format(headerTemplate, request.ContentType));
                requestStream.Write(buffer, 0, buffer.Length);

                WriteFile(requestStream);

                buffer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
            }
        }

        private void MakeRequest(HttpWebRequest request)
        {
            Stream requestStream = null;

            try
            {
                request.ContentType = "application/x-www-form-urlencoded";

                StringBuilder sb = new StringBuilder();

                foreach (String k in formParams.Keys)
                {
                    if (sb.Length > 0) sb.Append('&');
                    sb.Append(k).Append("=").Append(HttpUtility.UrlEncode(formParams[k]));
                }

                byte[] bytes = Encoding.ASCII.GetBytes(sb.ToString());

                request.ContentLength = bytes.Length;

                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
            }
        }

        public virtual void PrepareArguments() { }
        public virtual void WriteFile(Stream outp) { }

        public abstract Uri GetURL(String baseDomain);

        protected void AppendParam(String paramName, String val)
        {
            if (formParams.ContainsKey(paramName) == false)
                formParams.Add(paramName, val);
        }
    }
}


















