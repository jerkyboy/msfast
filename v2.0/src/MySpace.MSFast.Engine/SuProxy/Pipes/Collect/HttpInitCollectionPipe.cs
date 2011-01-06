using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Pipes.Utils;
using MySpace.MSFast.Engine.SuProxy.Utils;
using System.Text.RegularExpressions;
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.Engine.SuProxy.Proxy;
using MySpace.MSFast.Engine.CollectorsConfiguration;

namespace MySpace.MSFast.Engine.SuProxy.Pipes.Collect
{
    public class HttpInitCollectionPipe : HttpPipe
    {
        private static String responseHeader = "HTTP/1.1 200 OK\r\n" +
                                               "Server: SuProxy\r\n" +
                                               "Accept-Ranges: bytes\r\n" +
                                               "Vary: Accept-Encoding\r\n" +
                                               "Content-Length: {0}\r\n\r\n";

        public override void SendData(byte[] buffer, int offset, int length){}

        public override void Flush()
        {
            CollectionInfoParser collectionInfoParser = new CollectionInfoParser(this.PipesChain.ChainState);

            StringBuilder scripts = new StringBuilder();

            if (this.Configuration is EngineSuProxyConfiguration)
            {
                scripts.Append(CollectorScriptsConfig.Instance.PageDataCollector);
                scripts.AppendFormat(CollectorScriptsConfig.Instance.Constructor, ((EngineSuProxyConfiguration)this.Configuration).CollectionID,
                                                                                  0,
                                                                                  collectionInfoParser.URL,
                                                                                  collectionInfoParser.URLEncoded,
                                                                                  collectionInfoParser.NextURL);

                foreach (CollectorScript cs in CollectorScriptsConfig.Instance.Values)
                {
                    scripts.Append(cs.Script);
                }
            }

            String page = String.Format(CollectorScriptsConfig.Instance.EmptyHTML, scripts.ToString() , CollectorScriptsConfig.Instance.Event_OnStartingTest);

            byte[] h = Encoding.UTF8.GetBytes(String.Format(responseHeader, b.Length));
            byte[] b = Encoding.UTF8.GetBytes(page);

            base.SendData(h,0,h.Length);
            base.SendData(b,0,b.Length);
            
            base.Flush();
        }
    }
}