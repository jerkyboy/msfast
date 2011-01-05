using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("cache")]
    public class Cache
    {
        [JSONField("afterRequest")]
        public AfterRequest AfterRequest;

        public Cache(DownloadState ds, ProcessedDataPackage package)
        {
        }
    }

    [JSONObject("afterRequest")]
    public class AfterRequest
    {
        [JSONField("expires")]
        public DateTime Expires;

        [JSONField("lastAccess")]
        public DateTime LastAccess;

        [JSONField("eTag")]
        public String eTag = "";

        [JSONField("hitCount")]
        public int HitCount = 0;
        
        
    }
}
