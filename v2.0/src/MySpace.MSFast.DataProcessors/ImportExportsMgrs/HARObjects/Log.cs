using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.Core.Configuration.ConfigProviders;
using MySpace.MSFast.DataProcessors.Download;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("log")]
    public class Log
    {
        [JSONField("version")]
        public String Version;

        [JSONField("creator")]
        public Creator Creator;

        [JSONField("browser")]
        public Browser Browser;

        [JSONField("pages")]
        public Page[] Pages;

        [JSONField("entries")]
        public Entry[] Entries;

        public Log(ProcessedDataPackage pacakge)
        {
            Version = "1.1";
            Creator = new Creator(pacakge);
            Browser = new Browser(pacakge);
            Pages = new Page[]{new Page(pacakge)};
            Entries = Get_Entries(pacakge);
        }

        private Entry[] Get_Entries(ProcessedDataPackage package)
        {
            if (package.ContainsKey(typeof(DownloadData)) == false)
                return null;

            DownloadData r = (DownloadData)package[typeof(DownloadData)];

            if (r == null)
                return null;

            LinkedList<Entry> entry = new LinkedList<Entry>();

            foreach (DownloadState ds in r)
            {
                entry.AddLast(new Entry(ds, package));
            }
            return new List<Entry>(entry).ToArray();
        }       
    }
}
