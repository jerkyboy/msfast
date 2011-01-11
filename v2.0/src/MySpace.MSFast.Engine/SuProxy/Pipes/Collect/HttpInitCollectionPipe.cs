using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Pipes.Utils;
using MySpace.MSFast.Engine.SuProxy.Utils;
using System.Text.RegularExpressions;
using MySpace.MSFast.SuProxy.Pipes;
using MySpace.MSFast.Engine.SuProxy.Proxy;
using MySpace.MSFast.Engine.CollectorsConfiguration;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;

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
                scripts.Append(CollectorsConfig.Instance.GetArgumentValue("PageDataCollector"));
                scripts.AppendFormat(CollectorsConfig.Instance.GetArgumentValue("Constructor"), ((EngineSuProxyConfiguration)this.Configuration).CollectionID,
                                                                                  0,
                                                                                  collectionInfoParser.URL,
                                                                                  collectionInfoParser.URLEncoded,
                                                                                  collectionInfoParser.NextURL,
                                                                                  collectionInfoParser.NextURLEncoded);

                foreach (CollectorsScript cs in CollectorsConfig.Instance.GetAllScripts())
                {
                    scripts.Append(CollectorsConfig.Instance.FormatCollectorsScript(cs));
                }
            }

            String page = String.Format(CollectorsConfig.Instance.GetArgumentValue("EmptyHTML"), scripts.ToString(), CollectorsConfig.Instance.GetArgumentValue("Event_OnStartingTest"));

            byte[] b = Encoding.UTF8.GetBytes(page);
            byte[] h = Encoding.UTF8.GetBytes(String.Format(responseHeader, b.Length));

            base.SendData(h,0,h.Length);
            base.SendData(b,0,b.Length);
            
            base.Flush();
        }
    }
}