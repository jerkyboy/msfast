using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.PageSource;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("page")]
    public class Page
    {
        [JSONField("startedDateTime")]
        public DateTime StartedDateTime;

        [JSONField("id")]
        public String ID;

        [JSONField("title")]
        public String Title;

        [JSONField("pageTimings")]
        public PageTimings PageTimings;

        [JSONField("pageSource")]
        public String PageSourceData;

        public Page(ProcessedDataPackage pacakge)
        {
            ID = "page_" + pacakge.CollectionID;
            Title = "MSFast Collection #" + pacakge.CollectionID;
            StartedDateTime = Converter.EpochToDateTime(pacakge.CollectionStartTime);
            
            PageTimings = new PageTimings(pacakge);

            PageSourceData sd = (PageSourceData)pacakge[typeof(PageSourceData)];

            this.PageSourceData = Convert.ToBase64String(Encoding.UTF8.GetBytes(sd.PageSource));

        }
    }
}
