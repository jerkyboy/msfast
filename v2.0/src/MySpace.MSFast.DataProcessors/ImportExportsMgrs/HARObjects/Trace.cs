using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject]
    public class Trace
    {
        [JSONField("log")]
        public Log Log;

        public Trace(ProcessedDataPackage pacakge)
        {
            Log = new Log(pacakge);
        }
    }
}
