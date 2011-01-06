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
        public static Regex CollectQueryParsers_Pre = new Regex("__PRE_COLLECTION=(.*)$");
        public static Regex CollectQueryParsers_Normal = new Regex("(__MSFAST_PAGEHASH=([abcdef0123456789]*)&)*__MSFAST_COLLECT_GROUP=([0-9]*)$");

        public String URLEncoded = String.Empty;
        public String URL = String.Empty;
        public uint CurrentCollectionGroup = 0;
        public String CollectHash = String.Empty;

        public CollectionInfoParser(HttpPipesChainState chainState)
        {       
            if (chainState != null && chainState.ContainsKey("REQUEST_URI"))
            {
                String uriStr = (String)chainState["REQUEST_URI"];
                
                Match m = CollectQueryParsers_Pre.Match(uriStr);

                try
                {
                    if (m.Success)
                    {
                        this.URLEncoded = m.Groups[1].Value;
                        this.URL = Encoding.UTF8.GetString(Convert.FromBase64String(this.URLEncoded));
                    }
                }
                catch 
                {
                }

                try { this.CurrentCollectionGroup = uint.Parse(m.Groups[2].Value); }catch { }

                m = CollectQueryParsers_Normal.Match(uriStr);

                try { this.CurrentCollectionGroup = uint.Parse(m.Groups[3].Value); }catch { }
                try { this.CollectHash = m.Groups[2].Value; }catch { }

                if (String.IsNullOrEmpty(this.CollectHash))
                {
                    this.CollectHash = Guid.NewGuid().ToString().ToLower().Replace("-", "");
                }
            }            
        }
    }
}
