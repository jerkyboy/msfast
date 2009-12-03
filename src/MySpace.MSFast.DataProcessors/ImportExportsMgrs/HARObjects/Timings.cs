using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("timings")]
    public class Timings
    {
        [JSONField("dns")]
        public long DNS;

        [JSONField("connect")]
        public long Connect;

        [JSONField("send")]
        public long Send;

        [JSONField("wait")]
        public long Wait;

        [JSONField("receive")]
        public long Receive;

        [JSONField("blocked")]
        public long Blocked;

        public Timings(DownloadState ds, ProcessedDataPackage package)
        {
            Connect = ds.SendingRequestStartTime - ds.ConnectionStartTime;
            Blocked = ds.ReceivingResponseStartTime - ds.SendingRequestEndTime;
            DNS = ds.SendingRequestStartTime - ds.ConnectionStartTime;
            Send = ds.SendingRequestEndTime - ds.SendingRequestStartTime;
            Receive = ds.ReceivingResponseEndTime - ds.ReceivingResponseStartTime;
            Wait = ds.ReceivingResponseStartTime - ds.SendingRequestEndTime;
        }

    }
}
