using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Markers;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("pageTimings")]
    public class PageTimings
    {
        [JSONField("onContentLoad")]
        public long OnContentLoad;

        [JSONField("onLoad")]
        public long OnLoad;

        public PageTimings(ProcessedDataPackage pacakge)
        {
            MarkersData r = (MarkersData)pacakge[typeof(MarkersData)];

            foreach (Marker m in r)
            {
                if ("Ready State Interactive".Equals(m.Name) || "Ready State Complete".Equals(m.Name) && m.Timestamp > 0)
                {
                    OnContentLoad = m.Timestamp;
                }
                else if ("onload".Equals(m.Name) && m.Timestamp > 0)
                {
                    OnLoad = m.Timestamp;
                }
            }
        }
    }
}