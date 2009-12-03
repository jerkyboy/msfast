using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("request")]
    public class Request
    {
        [JSONField("method")]
        public String Method;

        [JSONField("url")]
        public String URL;

        [JSONField("httpVersion")]
        public String HttpVersion;

        [JSONField("cookies")]
        public Cookie[] Cookies;

        [JSONField("headers")]
        public Header[] Headers;

        [JSONField("queryString")]
        public QueryString[] QueryStrings;

        [JSONField("headersSize")]
        public long HeadersSize;

        [JSONField("bodySize")]
        public long BodySize;


        public Request(DownloadState ds, ProcessedDataPackage package)
        {
            BodySize = -1;
            URL = ds.URL;
            Method = "GET";                     /*TODO - Parse Header*/
            HttpVersion = "HTTP/1.1";           /*TODO - Parse Header*/
            Headers = new Header[0];            /*TODO - Parse Header*/
            Cookies = new Cookie[0];            /*TODO - Parse Header*/
            HeadersSize = -1;                   /*TODO - Parse Header*/
            QueryStrings = new QueryString[0];  /*TODO - Parse Header*/
        }
    }
}