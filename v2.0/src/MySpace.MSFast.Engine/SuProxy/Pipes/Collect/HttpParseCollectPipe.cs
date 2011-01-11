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
using MySpace.MSFast.SuProxy.Pipes.Utils;
using System.IO;
using System.Text.RegularExpressions;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using System.Reflection;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.Engine.SuProxy.Utils;
using MySpace.MSFast.Engine.SuProxy.Proxy;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
	public class HttpParseCollectPipe : HttpBreakerPipe
	{
        private static String PreCollectionResponseHeader = "HTTP/1.1 200 OK\r\n" +
                                                            "Server: SuProxy\r\n" +
                                                            "Accept-Ranges: bytes\r\n" +
                                                            "Vary: Accept-Encoding\r\n" +
                                                            "Content-Length: {0}\r\n\r\n";

        private MemoryStream bodyMemoryStream = null;

		private static Dictionary<String, ChunkedPage> CachedPages = new Dictionary<string,ChunkedPage>();

        private CollectionInfoParser CollectionInfoParser;

		public override void SendHeader(string header)
		{
            this.CollectionInfoParser = new CollectionInfoParser(this.PipesChain.ChainState);

            if (CachedPages.ContainsKey(this.CollectionInfoParser.CollectHash))
                return;
			
            CachedPages.Add(this.CollectionInfoParser.CollectHash, new ChunkedPage(header)); //Response Header
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
            if (this.CollectionInfoParser == null)
				return;

            ChunkedPage page = null;

            CachedPages.TryGetValue(this.CollectionInfoParser.CollectHash, out page);

            if(page == null || page.IsParsed)
                return;
            
            if (bodyMemoryStream == null)
                bodyMemoryStream = new MemoryStream();

            bodyMemoryStream.Write(buffer, offset, length);
		}

		public override void Flush()
		{
            if (this.CollectionInfoParser == null)
                return;

            ChunkedPage page = null;

            CachedPages.TryGetValue(this.CollectionInfoParser.CollectHash, out page);

            if (page == null)
                return;

            if (page.IsParsed == false && bodyMemoryStream != null && this.Configuration is EngineSuProxyConfiguration)
            {
                page.Parse(Encoding.UTF8.GetString(bodyMemoryStream.ToArray()), 60);

                Stream sdfi = (new SourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);
                Stream bsdfi = (new BrokenSourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);

                if (bsdfi != null && sdfi != null)
                    page.SaveToDisc(sdfi, bsdfi);
            }

            byte[] bodyData = null;            

            if (this.CollectionInfoParser.CurrentCollectionGroup == -1)
            {
                bodyData = Encoding.UTF8.GetBytes(GetPreCollectionPage(page));
                base.SendHeader(String.Format(PreCollectionResponseHeader, bodyData.Length));
            }
            else
            {
                base.SendHeader(page.Header);
                bodyData = Encoding.UTF8.GetBytes(InjectJavascript(page));
            }

            base.SendBodyData(bodyData, 0, bodyData.Length);
			base.Flush();
        }

        private String GetPreCollectionPage(ChunkedPage chunkedPage)
        {
            StringBuilder scripts = new StringBuilder();

            if (this.Configuration is EngineSuProxyConfiguration)
            {
                scripts.Append(CollectorsConfig.Instance.GetArgumentValue("PageDataCollector"));
                scripts.AppendFormat(CollectorsConfig.Instance.GetArgumentValue("Constructor"), ((EngineSuProxyConfiguration)this.Configuration).CollectionID,
                                                                                  chunkedPage.Body.Length,
                                                                                  this.CollectionInfoParser.URL,
                                                                                  this.CollectionInfoParser.URLEncoded,
                                                                                  this.CollectionInfoParser.NextURL,
                                                                                  this.CollectionInfoParser.NextURLEncoded);


                foreach (CollectorsScript cs in CollectorsConfig.Instance.GetAllScripts())
                {
                    scripts.Append(CollectorsConfig.Instance.FormatCollectorsScript(cs));
                } 

                scripts.Append(CollectorsConfig.Instance.GetArgumentValue("Event_OnInit"));
            }

            return String.Format(CollectorsConfig.Instance.GetArgumentValue("EmptyHTML"), scripts.ToString(), CollectorsConfig.Instance.GetArgumentValue("Event_OnLoadingFirstCollectionPage"));
        }

        #region While Collecting
        private String InjectJavascript(ChunkedPage chunkedPage)
		{
			StringBuilder modifiedPage = new StringBuilder();
			StringBuilder scripts = new StringBuilder();
            bool appendScripts = false;

            CollectorsScript[] collectorScripts = CollectorsConfig.Instance.GetScriptsGroup((uint)this.CollectionInfoParser.CurrentCollectionGroup);

            if (collectorScripts != null)
            {
                foreach (CollectorsScript cs in collectorScripts)
                {
                    scripts.Append(CollectorsConfig.Instance.FormatCollectorsScript(cs));
                }
            }
            
            if (String.IsNullOrEmpty(this.CollectionInfoParser.NextURL))
                CachedPages.Remove(this.CollectionInfoParser.CollectHash);

            appendScripts = scripts.Length > 0;

			modifiedPage.Append(chunkedPage.StartToHead);

            if (appendScripts && this.Configuration is EngineSuProxyConfiguration)
			{
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("PageDataCollector"));
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Constructor"), ((EngineSuProxyConfiguration)this.Configuration).CollectionID, 
                                                                                            chunkedPage.Body.Length,
                                                                                            this.CollectionInfoParser.URL,
                                                                                            this.CollectionInfoParser.URLEncoded,
                                                                                            this.CollectionInfoParser.NextURL,
                                                                                            this.CollectionInfoParser.NextURLEncoded);
                AppendScript(modifiedPage, scripts.ToString());
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnInit"));
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnStartDocument"));
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnStartHtml"));
                AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnStartHead"));
			}
			
            modifiedPage.Append(chunkedPage.HeadContent);
            if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnEndHead"));

			modifiedPage.Append(chunkedPage.HeadToBody);
            if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnStartBody"));

            int chuckid = 1;
			foreach (BodyChunk bc in chunkedPage.Body)
			{
				modifiedPage.Append(bc.Content);
                if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnSegment"), chuckid++);
			}

            if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnEndBody"));
			
            modifiedPage.Append(chunkedPage.BodyToPostBody);
			modifiedPage.Append(chunkedPage.PostBodyContent);

            if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnEndHtml"));

			modifiedPage.Append(chunkedPage.PostBodyToEnd);

            if (appendScripts) AppendScript(modifiedPage, CollectorsConfig.Instance.GetArgumentValue("Event_OnEndDocument"));

			return modifiedPage.ToString();
		}

        private void AppendScript(StringBuilder modifiedPage, string script)
        {
            AppendScript(modifiedPage, script, null);
        }
        private void AppendScript(StringBuilder modifiedPage, string script, params object[] args)
        {
            modifiedPage.Append("<script>");
            if (args != null)
                modifiedPage.Append(String.Format(script, args));
            else
                modifiedPage.Append(script);
            modifiedPage.Append("</script>");
        }
		private string base64Encode(string data)
		{
			try
			{
				byte[] encData_byte = new byte[data.Length];
				encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
				string encodedData = Convert.ToBase64String(encData_byte);
				return encodedData;
			}
			catch
			{
				return null;
			}
        }
        #endregion

    }
}



























