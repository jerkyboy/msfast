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
        public static Regex MSFAST_START_TEST_NEXT_URL = new Regex("__MSFAST_START_TEST=([^&\\?]*)");
        public static Regex MSFAST_TEST_URL = new Regex("__MSFAST_TEST_URL=([^&\\?]*)");
        public static Regex MSFAST_PAGE_HASH = new Regex("__MSFAST_PAGE_HASH=([abcdef0123456789]*)");
        public static Regex MSFAST_COLLECT_GROUP = new Regex("__MSFAST_COLLECT_GROUP=([0-9]*)");

        
        public static string GetInitURL(string URL, string FirstURL)
        {
            return URL + ((URL.IndexOf("?") == -1) ? "?" : "&") + "__MSFAST_START_TEST=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(FirstURL)) + "&__MSFAST_TEST_URL=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(URL));
        }
        public static String GetFirstURL(String URL)
        {
            return URL + ((URL.IndexOf("?") == -1) ? "?" : "&") + "__MSFAST_TESTING=1&__MSFAST_TEST_URL=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(URL));
        }
        
        public String URLEncoded = String.Empty;
        public String URL = String.Empty;
        public String NextURL = String.Empty;
        public String CollectHash = String.Empty;

        public int CurrentCollectionGroup = -1;

        public CollectionInfoParser(HttpPipesChainState chainState)
        {
            if (chainState == null || chainState.ContainsKey("REQUEST_URI") == false)
                return;

            Parse((String)chainState["REQUEST_URI"]);
        }
        public CollectionInfoParser(String URL)
        {
            Parse(URL);
        }

        private void Parse(String uriStr)
        {
            Match m = MSFAST_TEST_URL.Match(uriStr);

            if (m.Success)
            {
                this.URLEncoded = m.Groups[1].Value;
                this.URL = Encoding.UTF8.GetString(Convert.FromBase64String(this.URLEncoded));
            }
            else
            {
                this.URLEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(uriStr));
                this.URL = uriStr;
            }

            //if first page, do nothing just go to next url
            m = MSFAST_START_TEST_NEXT_URL.Match(uriStr);

            if (m.Success)
            {
                this.NextURL = Convert.ToBase64String(Encoding.UTF8.GetBytes(m.Groups[1].Value));
                return;
            }

            m = MSFAST_PAGE_HASH.Match(uriStr);
            
            if (m.Success)
            {
                this.CollectHash = m.Groups[1].Value; 
            }
            
            if (String.IsNullOrEmpty(this.CollectHash))
            {
                this.CollectHash = Guid.NewGuid().ToString().ToLower().Replace("-", "");
            }

            m = MSFAST_COLLECT_GROUP.Match(uriStr);

            if (m.Success)
            {
                try { this.CurrentCollectionGroup = int.Parse(m.Groups[1].Value); } catch { }
            }

            this.NextURL = this.URL + ((this.URL.IndexOf("?") == -1) ? "?" : "&") + "__MSFAST_TESTING=1" + 
                "&__MSFAST_TEST_URL=" + this.URLEncoded +
                "&__MSFAST_PAGEHASH=" + this.CollectHash + 
                ((this.CurrentCollectionGroup >=0 ) ? "&__MSFAST_COLLECT_GROUP=" + (this.CurrentCollectionGroup + 1) : String.Empty);
        }



    }
}
