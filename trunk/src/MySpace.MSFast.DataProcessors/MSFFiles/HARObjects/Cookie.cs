using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("response")]
    public class Cookie
    {
        [JSONField("Name")]
        public String Name;

        [JSONField("value")]
        public String Value;

        [JSONField("domain")]
        public String Domain;

        [JSONField("expires")]
        public DateTime Expires;

        [JSONField("path")]
        public String Path;
    }
}
