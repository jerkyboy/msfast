using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Render;

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
            RenderData r = (RenderData)pacakge[typeof(RenderData)];
            
            OnContentLoad = ((r.ReadyStateInteractive <= 0) ? -1 : (r.ReadyStateInteractive-pacakge.CollectionStartTime));
            OnLoad = ((r.ReadyStateComplete <= 0) ? -1 : (r.ReadyStateComplete-pacakge.CollectionStartTime));
        }
    }
}