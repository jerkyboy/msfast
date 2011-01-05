using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.SuProxy.Proxy;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.Engine.SuProxy.Proxy
{
    public class EngineSuProxyConfiguration : SuProxyConfiguration, CollectionMetaInfo
    {
        public int _collectionID;
        public string _dumpFolder;
        public String URL;

        public EngineSuProxyConfiguration(int ProxyPort, int MaxConnectionsQue, int ProxletsPoolSize, int ProxletsWaitTimeout, int ProxletsWaitTimeoutRetries)
            :base( ProxyPort,  MaxConnectionsQue,  ProxletsPoolSize,  ProxletsWaitTimeout,  ProxletsWaitTimeoutRetries)
        {}
        
        public static new EngineSuProxyConfiguration Default
        {
            get
            {
                return new EngineSuProxyConfiguration(8080, 100, 20, 2 * 60 * 1000, 2);
            }
        }

        #region CollectionMetaInfo Members

        public int CollectionID
        {
            get { return this._collectionID; }
            set { this._collectionID = value; }
        }

        public string DumpFolder
        {
            get { return this._dumpFolder; }
            set { this._dumpFolder = value; }
        }

        #endregion
    }
}
