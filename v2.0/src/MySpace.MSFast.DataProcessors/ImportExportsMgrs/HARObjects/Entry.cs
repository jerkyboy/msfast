using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.Core.Configuration.Common;
using System.IO;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("entry")]
    public class Entry
    {
        [JSONField("pageref")]
        public String PageRef;

        [JSONField("startedDateTime")]
        public DateTime StartedDateTime;

        [JSONField("time")]
        public long Time;

        [JSONField("request")]
        public Request Request;
        
        [JSONField("response")]
        public Response Response;

        [JSONField("cache")]
        public Cache Cache;

        [JSONField("timings")]
        public Timings Timings;


        public Entry(DownloadState ds, ProcessedDataPackage package)
        {
            this.PageRef = "page_" + package.CollectionID;

            this.StartedDateTime = Converter.EpochToDateTime(ds.ConnectionStartTime);

            this.Cache = new Cache(ds, package);
            this.Request = new Request(ds, package);
            this.Response = new Response(ds, package);
            this.Timings = new Timings(ds, package);
            
            this.Time = this.Timings.Connect +
                        this.Timings.Blocked +
                        this.Timings.DNS +
                        this.Timings.Send +
                        this.Timings.Receive +
                        this.Timings.Wait;
        }
    }
}
