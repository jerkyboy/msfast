using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace BDika.Client.API.Comm
{
    public abstract class TestingClientCall<T> where T : ServerResponse
    {
        private String requestBody = String.Empty;

        public virtual T ExecuteCall(String clientID, String clientKey, int timeout)
        {
            requestBody = String.Empty;

            AppendParam("client_id", clientID);
            AppendParam("client_key", clientKey);

            PrepareArguments();

            Uri uri = GetURL();

            WebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            Stream requestStream = null;
            Stream responseStream = null;
            T responseDic = null;

            try
            {
                request = WebRequest.Create(uri);
                request.Timeout = timeout;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                byte[] bytes = Encoding.ASCII.GetBytes(requestBody);

                request.ContentLength = bytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();

                response = (HttpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                String s = reader.ReadToEnd();
                responseDic = JsonConvert.DeserializeObject<T>(s);

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

                if (requestStream != null)
                    requestStream.Close();

                if (responseStream != null)
                    responseStream.Close();                                
            }
            
            return responseDic;
        }

        public virtual void PrepareArguments(){}
        
        public abstract Uri GetURL();

        protected void AppendParam(string paramName, object val)
        {
            requestBody = String.Concat(requestBody, "&" , paramName, "=", HttpUtility.UrlEncode(val.ToString()));
        }
    }
}


















