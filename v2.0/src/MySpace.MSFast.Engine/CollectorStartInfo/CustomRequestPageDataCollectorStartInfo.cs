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
using MySpace.MSFast.Engine.DataCollector;

namespace MySpace.MSFast.Engine.CollectorStartInfo
{
	public class CustomRequestPageDataCollectorStartInfo : PageDataCollectorStartInfo 
	{

		private String generatedTempFilename = null;

		public override PageDataCollectorErrors PrepareStartInfo()
		{
			if (this.IsValid() == false)
			{
				return PageDataCollectorErrors.InvalidOrMissingArguments;
			}

            generatedTempFilename = base.TempFolder.Replace("\\", "/");
			
			if (generatedTempFilename.EndsWith("/") == false)
				generatedTempFilename += "/";

			generatedTempFilename += Guid.NewGuid().ToString().Replace("-", "") + ".html";

			if (CreateCustomRequest(generatedTempFilename) == false)
			{
				return PageDataCollectorErrors.CantSaveTempFile;
			}

            base.URLWithCollectionArgs = String.Concat("file://" + generatedTempFilename);

			return base.PrepareStartInfo();
		}

		public override PageDataCollectorErrors CleanUp()
		{
			if (String.IsNullOrEmpty(this.generatedTempFilename) == false && File.Exists(this.generatedTempFilename))
			{
				File.Delete(this.generatedTempFilename);
			}
			return base.CleanUp();
		}

		private const String CUSTOM_REQUEST_START =
						"<html><body>" +
						"<h1 style=\"font-family:arial;color:#1556a5\">SuProxy Request Forward</h1><h2 style=\"font-family:arial;color:#789ac3\">Please wait while your request is processed...</h2>" +
						"<form action=\"http://";

		private const String CUSTOM_REQUEST_CONTINUED = "/?CUSTOM_REQUEST=1\" method=\"post\" enctype=\"multipart/form-data\"><textarea name=\"customRequestBody\"  style=\"width:900px;height:400px;\">";

		private const String CUSTOM_REQUEST_END = "</textarea><br/><input type=\"submit\" value=\"Click here if nothing happend...\" /></form><script>setTimeout(\"document.forms[0].submit();\",100);</script></body></html>";


		private bool CreateCustomRequest(string filename)
		{
			// Save temp html*/
			try
			{
				StreamWriter sw = new StreamWriter(filename);
				sw.Write(CUSTOM_REQUEST_START);

				Uri ur = new Uri(this.URL);
				sw.Write(ur.Host);
				sw.Write(CUSTOM_REQUEST_CONTINUED);
				sw.Write(this.Serialize());
				sw.Write(CUSTOM_REQUEST_END);
				sw.Flush();
				sw.Close();
			}
			catch
			{
				return false;
			}

			return true;
		}

		#region Request Properties
		#region Method
		public enum MethodType
		{
			GET,
			POST
		}
		public MethodType Method = MethodType.GET;
		#endregion

        #region HttpVersion
        public enum HttpVersions
		{
			OnePointOne,
			OnePointZero
		}
        public HttpVersions HttpVersion = HttpVersions.OnePointOne;
		#endregion

		#region Accept
		public String Accept = "*/*";
		#endregion

		#region AcceptLanguage
		public String AcceptLanguage = "en-us";
		#endregion

		#region Accept-Encoding
		public String AcceptEncoding = "gzip, deflate";
		#endregion

		#region User-Agent
		public String UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1;)";
		#endregion

		#region Connection
		public enum ConnectionType
		{
			KeepAlive,
			Closed
		}
		public ConnectionType Connection = ConnectionType.Closed;
		#endregion

		#region Cache-Control
		public enum CacheControlType
		{
			NoCache
		}
		public CacheControlType CacheControl = CacheControlType.NoCache;
		#endregion

		#region Cookies
		public String Cookie = null;
		#endregion

		#region Form Variables
		private Dictionary<String, String> formVars = null;
		public void AddFormVariable(String name, String value)
		{
			if (Method != MethodType.POST)
				throw new NotImplementedException("Can't add parameters to a \"GET\" request. Please set Method to \"POST\"");
			if (formVars == null)
				formVars = new Dictionary<string, string>();

			if (formVars.ContainsKey(name)) return;

			formVars.Add(name, value);
		}
		#endregion

		#region Request Body
		public String RequestBody = null;
		#endregion

		public String Serialize()
		{
			StringBuilder sb = new StringBuilder();

			if (Method == MethodType.POST && formVars != null && formVars.Count > 0)
			{
				StringBuilder requestBodySB = new StringBuilder();
				bool isFirst = true;
				foreach (String key in formVars.Keys)
				{
					if (!isFirst)
						requestBodySB.Append("&");
					requestBodySB.Append(key);
					requestBodySB.Append("=");
					requestBodySB.Append(formVars[key]);

					if (isFirst)
						isFirst = false;
				}
				RequestBody = requestBodySB.ToString();
			}

			#region Method
			if (Method == MethodType.GET)
			{
				sb.Append("GET ");
			}
			else if (Method == MethodType.POST)
			{
				sb.Append("POST ");
			}
			#endregion

			#region URL
			if (String.IsNullOrEmpty(base.URLWithCollectionArgs))
			{
				throw new UriFormatException();
			}
			Uri uri = new Uri(base.URLWithCollectionArgs);
			sb.Append(base.URLWithCollectionArgs);
			#endregion

            #region HttpVersions
            if (HttpVersion == HttpVersions.OnePointOne)
			{
				sb.Append(" HTTP/1.1\r\n");
			}
            else if (HttpVersion == HttpVersions.OnePointZero)
			{
				sb.Append(" HTTP/1.0\r\n");
			}
			#endregion

			#region Accept
			if (String.IsNullOrEmpty(Accept) == false)
			{
				sb.Append("Accept: ");
				sb.Append(Accept);
				sb.Append("\r\n");
			}
			#endregion

			#region Accept-Language
			if (String.IsNullOrEmpty(AcceptLanguage) == false)
			{
				sb.Append("Accept-Language: ");
				sb.Append(AcceptLanguage);
				sb.Append("\r\n");
			}
			#endregion

			if (Method == MethodType.POST && formVars != null && formVars.Count > 0)
			{
				sb.Append("Content-Type: application/x-www-form-urlencoded\r\n");
			}

			#region Accept-Encoding
			if (String.IsNullOrEmpty(AcceptEncoding) == false)
			{
				sb.Append("Accept-Encoding: ");
				sb.Append(AcceptEncoding);
				sb.Append("\r\n");
			}
			#endregion

			#region User-Agent
			if (String.IsNullOrEmpty(UserAgent) == false)
			{
				sb.Append("User-Agent: ");
				sb.Append(UserAgent);
				sb.Append("\r\n");
			}
			#endregion

			#region Host
			sb.Append("Host: ");
			sb.Append(uri.Host);
			sb.Append("\r\n");
			#endregion

			#region Content Length
			sb.Append("Content-Length: ");
			sb.Append(String.IsNullOrEmpty(RequestBody) ? 0 : RequestBody.Length);
			sb.Append("\r\n");
			#endregion

			#region Connection
			sb.Append("Connection: ");
			if (Connection == ConnectionType.KeepAlive)
			{
				sb.Append("Keep-Alive");
			}
			else if (Connection == ConnectionType.Closed)
			{
				sb.Append("closed");
			}
			sb.Append("\r\n");
			#endregion

			#region Cache-Control
			sb.Append("Cache-Control: ");
			if (CacheControl == CacheControlType.NoCache)
			{
				sb.Append("no-cache");
			}
			sb.Append("\r\n");
			#endregion

			#region Cookies
			if (String.IsNullOrEmpty(Cookie) == false)
			{
				sb.Append("Cookie: ");
				sb.Append(Cookie);
				sb.Append("\r\n");
			}
			#endregion

			#region Requst Body
			if (String.IsNullOrEmpty(RequestBody) == false)
			{
				sb.Append("\r\n");
				sb.Append(RequestBody);
				sb.Append("\r\n");
			}
			#endregion

			sb.Append("\r\n");

			return sb.ToString();
		}		
		#endregion
	}
}
