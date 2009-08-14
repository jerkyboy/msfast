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
		private MemoryStream collectedBody = null;

		private static Dictionary<String, ChunkedPage> CachedPages = new Dictionary<string,ChunkedPage>();
		private static CollectorsConfig collectorsConfig = null;

		private static object initLock = new object();

        private CollectionInfoParser CollectionInfoParser;

		private bool isFirstHit = false;

		public override void Init(System.Collections.Generic.Dictionary<object, object> dictionary)
		{
			base.Init(dictionary);

			lock (initLock)
			{
				if (collectorsConfig == null)
				{
                    Stream configStream = null;
                    try
                    {
                        configStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MySpace.MSFast.Engine.DataCollectors.config");
                        collectorsConfig = new CollectorsConfig(configStream);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        try
                        {
                            if (configStream != null)
                                configStream.Close();
                        }
                        catch 
                        {
                        }
                        configStream = null;
                    }


				}
			}
		}

		public override void SendHeader(string header)
		{
            this.CollectionInfoParser = new CollectionInfoParser(this.PipesChain.ChainState);

            if (CachedPages.ContainsKey(this.CollectionInfoParser.CollectHash) == false)
			{
				this.isFirstHit = true;
                CachedPages.Add(this.CollectionInfoParser.CollectHash, new ChunkedPage(header));
			}
		}

		public override void SendBodyData(byte[] buffer, int offset, int length)
		{
            if (this.CollectionInfoParser == null || CachedPages.ContainsKey(this.CollectionInfoParser.CollectHash) == false)
			{
				return;
			}
            if (CachedPages[this.CollectionInfoParser.CollectHash].IsParsed == false)
            {
				if (collectedBody == null)
					collectedBody = new MemoryStream();

				collectedBody.Write(buffer, offset, length);
			}
		}

		public override void Flush()
		{
            if (this.CollectionInfoParser == null || CachedPages.ContainsKey(this.CollectionInfoParser.CollectHash) == false)
			{
				return;
			}

            base.SendHeader(CachedPages[this.CollectionInfoParser.CollectHash].Header);

            if (CachedPages[this.CollectionInfoParser.CollectHash].IsParsed == false && collectedBody != null && this.Configuration is EngineSuProxyConfiguration)
			{
                CachedPages[this.CollectionInfoParser.CollectHash].Parse(Encoding.UTF8.GetString(collectedBody.ToArray()), 60);

                Stream sdfi = (new SourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);
                Stream bsdfi = (new BrokenSourceDumpFilesInfo((EngineSuProxyConfiguration)this.Configuration)).Open(FileAccess.Write);

                if (bsdfi != null && sdfi != null)
                {
                    CachedPages[this.CollectionInfoParser.CollectHash].SaveToDisc(sdfi, bsdfi);
                }
			}

            byte[] bodyData = Encoding.UTF8.GetBytes(InjectJavascript(CachedPages[this.CollectionInfoParser.CollectHash]));
			base.SendBodyData(bodyData, 0, bodyData.Length);

			base.Flush();
		}

		private String InjectJavascript(ChunkedPage chunkedPage)
		{
			StringBuilder modifiedPage = new StringBuilder();
			StringBuilder scripts = new StringBuilder();
			
			bool allAreStandAlone = true;
			int removeFlags = 0;
            int currentTest = 0;

			foreach (CollectorsConfig.Collector cs in collectorsConfig)
			{
                if (((this.CollectionInfoParser.CollectFlags & (int)cs.CollectType) == (int)cs.CollectType) && cs.IsStandalone == false)
				{
					allAreStandAlone = false;
					break;
				}
			}

			if (allAreStandAlone == false)
			{
				foreach (CollectorsConfig.Collector cs in collectorsConfig)
				{
                    if ((this.CollectionInfoParser.CollectFlags & (int)cs.CollectType) == (int)cs.CollectType && cs.IsStandalone == false)
					{
						removeFlags |= (int)cs.CollectType;
                        currentTest |= (int)cs.CollectType;
						scripts.Append(cs.Collection);
					}
				}
			}else
			{
				foreach (CollectorsConfig.Collector cs in collectorsConfig)
				{
                    if ((this.CollectionInfoParser.CollectFlags & (int)cs.CollectType) == (int)cs.CollectType && cs.IsStandalone)
					{
						removeFlags |= (int)cs.CollectType;
                        currentTest |= (int)cs.CollectType;
						scripts.Append(cs.Collection);
						break;
					}
				}
			}

            int nextCollect = this.CollectionInfoParser.CollectFlags & (~removeFlags);
			String nextURL = "";

			if (nextCollect == 0)
			{
                CachedPages.Remove(this.CollectionInfoParser.CollectHash);
			}
            else if (this.PipesChain.ChainState.ContainsKey("REQUEST_URI") && this.Configuration is EngineSuProxyConfiguration)
			{
                nextURL = CollectionInfoParser.CollectQueryParsers_Normal.Replace((String)this.PipesChain.ChainState["REQUEST_URI"], "");
                nextURL += "__collecthash=" + this.CollectionInfoParser.CollectHash + "&__resultid=" + ((EngineSuProxyConfiguration)this.Configuration).CollectionID + "&__collect=" + nextCollect;
			}

			modifiedPage.Append(chunkedPage.StartToHead);

            if (scripts.Length > 0 && this.Configuration is EngineSuProxyConfiguration)
			{
				modifiedPage.Append(GetScript(collectorsConfig.Collection.JSMain));
                modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSetTestID, ((EngineSuProxyConfiguration)this.Configuration).CollectionID, chunkedPage.Body.Length + 4, currentTest, (String)this.PipesChain.ChainState["REQUEST_URI"], base64Encode((String)this.PipesChain.ChainState["REQUEST_URI"])));
				modifiedPage.Append(String.Format("<script>{0}</script>", scripts.ToString()));
				modifiedPage.Append(GetScript(collectorsConfig.Collection.JSInit));
				if(isFirstHit)
					modifiedPage.Append(GetScript(collectorsConfig.Collection.JSCollectStarted));
			}
			
			int brek = 0;
			modifiedPage.Append(chunkedPage.HeadContent);
			if (scripts.Length > 0) modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "HC"));

			modifiedPage.Append(chunkedPage.HeadToBody);
			if (scripts.Length > 0) modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "HB"));

			foreach (BodyChunk bc in chunkedPage.Body)
			{
				modifiedPage.Append(bc.Content);
				if (scripts.Length > 0) modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "BC"));
			}

			modifiedPage.Append(chunkedPage.BodyToPostBody);
			if (scripts.Length > 0) modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "BP"));

			modifiedPage.Append(chunkedPage.PostBodyContent);
			if (scripts.Length > 0) modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "PB"));

			modifiedPage.Append(chunkedPage.PostBodyToEnd);
			modifiedPage.Append(GetScript(collectorsConfig.Collection.JSSection, brek++, "PE"));

			if (scripts.Length > 0)
			{
				modifiedPage.Append(GetScript(collectorsConfig.Collection.JSDone));

				if (nextCollect != 0)
				{
					modifiedPage.Append(GetScript(collectorsConfig.Collection.JSNextCollect, nextURL));
				}
				else
				{
					modifiedPage.Append(GetScript(collectorsConfig.Collection.JSCollectEnded));
				}
			}

			return modifiedPage.ToString();
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

		private String GetScript(String script)
		{
			return GetScript(script, null);
		}

		private String GetScript(String script, params object[] args)
		{
			if(args != null)
				script = String.Format(script.ToString(), args);

			return String.Format("<script>{0}</script>",script);
		}
	}
}



























