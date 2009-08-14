using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Pipes;
using System.Text.RegularExpressions;
using MySpace.MSFast.Core.Configuration.Common;
using MySpace.MSFast.SuProxy.Proxy;

namespace MySpace.MSFast.Engine.SuProxy.Utils
{
    public class CollectionInfoParser
    {
        public static Regex CollectQueryParsers_Pre = new Regex("PRE_COLLECTION=(.*)&__r=([0-9]*)&__c=([0-9]*)$");
        public static Regex CollectQueryParsers_Normal = new Regex("(__collecthash=([abcdef0123456789]*)&)*(__resultid=([0-9]*)&)*__collect=([0-9]*)$");

        public String URLEncoded = "";
        public String URL = "";
        public int CollectFlags = 0;
        public String CollectHash = "";

        public CollectionInfoParser(HttpPipesChainState chainState)
        {       
            if (chainState != null && chainState.ContainsKey("REQUEST_URI"))
            {
                String uriStr = (String)chainState["REQUEST_URI"];
                Match m = CollectQueryParsers_Pre.Match(uriStr);

                try
                {
                    this.URLEncoded = m.Groups[1].Value;
                    this.URL = Encoding.UTF8.GetString(Convert.FromBase64String(this.URLEncoded));
                }
                catch { }

                //try { this._collectId = int.Parse(m.Groups[2].Value); }catch { }
                try { this.CollectFlags = int.Parse(m.Groups[3].Value); }catch { }

                m = CollectQueryParsers_Normal.Match(uriStr);

                //try { this._collectId = int.Parse(m.Groups[4].Value); }catch { }
                try { this.CollectFlags = int.Parse(m.Groups[5].Value); }catch { }
                try { this.CollectHash = m.Groups[2].Value; }catch { }

                if (String.IsNullOrEmpty(this.CollectHash))
                {
                    this.CollectHash = Guid.NewGuid().ToString().ToLower().Replace("-", "");
                }
            }            
        }
    }
}
