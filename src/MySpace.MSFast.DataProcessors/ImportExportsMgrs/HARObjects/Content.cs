using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("content")]
    public class Content
    {
        [JSONField("size")]
        public long Size;

        [JSONField("mimeType")]
        public String MimeType;

        [JSONField("text")]
        public String Text;
    }
}