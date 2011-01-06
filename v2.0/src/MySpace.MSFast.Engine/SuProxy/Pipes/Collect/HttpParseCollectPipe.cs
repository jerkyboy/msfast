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

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
	public class HttpParseCollectPipe : HttpBreakerPipe
	{
        private MemoryStream bodyMemoryStream = null;

		private static Dictionary<String, ChunkedPage> CachedPages = new Dictionary<string,ChunkedPage>();

        private CollectionInfoParser CollectionInfoParser;

		private bool isFirstHit = false;

		public override void SendHeader(string header)
		{
            this.CollectionInfoParser = new CollectionInfoParser(this.PipesChain.ChainState);

            if (CachedPages.ContainsKey(this.CollectionInfoParser.CollectHash))
                return;
			
			this.isFirstHit = true;
            CachedPages.Add(this.CollectionInfoParser.CollectHash, new ChunkedPage(header));
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
            if (this.CollectionInfoParser == null)
				return;

            ChunkedPage page = null;
            CachedPages.TryGetValue(this.CollectionInfoParser.CollectHash,out page);

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

            base.SendHeader(page.Header);

            if (page.IsParsed == false && bodyMemoryStream != null && this.Configuration is EngineSuProxyConfiguration)
			{
                page.Parse(Encoding.UTF8.GetString(bodyMemoryStream.ToArray()), 60);

                Stream sdfi = (new SourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);
                Stream bsdfi = (new BrokenSourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);

                if (bsdfi != null && sdfi != null)
                    page.SaveToDisc(sdfi, bsdfi);
			}

            byte[] bodyData = Encoding.UTF8.GetBytes(InjectJavascript(page));
			base.SendBodyData(bodyData, 0, bodyData.Length);

			base.Flush();
		}

		private String InjectJavascript(ChunkedPage chunkedPage)
		{
			StringBuilder modifiedPage = new StringBuilder();
			StringBuilder scripts = new StringBuilder();
            bool appendScripts = false;

            ICollection<CollectorScript> collectorScripts = CollectorScriptsConfig.Instance.GetGroupScripts(this.CollectionInfoParser.CurrentCollectionGroup);

            if (collectorScripts != null)
                foreach (CollectorScript cs in collectorScripts)
                    scripts.Append(cs.Script);

            String nextURL = String.Empty;

            if (this.CollectionInfoParser.CurrentCollectionGroup + 1 < CollectorScriptsConfig.Instance.GroupsCount)
            {
                if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI") && this.Configuration is EngineSuProxyConfiguration)
                {
                    nextURL = CollectionInfoParser.CollectQueryParsers_Normal.Replace((String)this.PipesChain.ChainState["REQUEST_URI"], "");
                    nextURL += ((nextURL.IndexOf("?") == -1) ? "?" : "&") + "__MSFAST_PAGEHASH=" + this.CollectionInfoParser.CollectHash + "&__MSFAST_COLLECT_GROUP=" + (this.CollectionInfoParser.CurrentCollectionGroup + 1);
                }
            }
            else
            {
                CachedPages.Remove(this.CollectionInfoParser.CollectHash);
            }

            appendScripts = scripts.Length > 0;

			modifiedPage.Append(chunkedPage.StartToHead);

            if (appendScripts && this.Configuration is EngineSuProxyConfiguration)
			{
                AppendScript(modifiedPage, CollectorScriptsConfig.Instance.PageDataCollector);
                AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Constructor,     ((EngineSuProxyConfiguration)this.Configuration).CollectionID, 
                                                                                            chunkedPage.Body.Length, 
                                                                                            (String)this.PipesChain.ChainState["REQUEST_URI"],
                                                                                            base64Encode((String)this.PipesChain.ChainState["REQUEST_URI"]), 
                                                                                            nextURL);
                AppendScript(modifiedPage, scripts.ToString());
                AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnInit);
                AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnStartDocument);
                AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnStartHead);
			}
			
            modifiedPage.Append(chunkedPage.HeadContent);
            if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnEndHead);

			modifiedPage.Append(chunkedPage.HeadToBody);
            if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnStartBody);

			foreach (BodyChunk bc in chunkedPage.Body)
			{
				modifiedPage.Append(bc.Content);
                if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnSegment);
			}
            
            if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnEndBody);
			
            modifiedPage.Append(chunkedPage.BodyToPostBody);
			modifiedPage.Append(chunkedPage.PostBodyContent);

            if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnEndHtml);

			modifiedPage.Append(chunkedPage.PostBodyToEnd);
            
            if (appendScripts) AppendScript(modifiedPage, CollectorScriptsConfig.Instance.Event_OnEndDocument);

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
	}
}



























