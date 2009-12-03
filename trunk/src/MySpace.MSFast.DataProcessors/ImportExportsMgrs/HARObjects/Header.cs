using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("header")]
    public class Header
    {
        [JSONField("name")]
        public String Name;

        [JSONField("value")]
        public String Value;
    }
}
