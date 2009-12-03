using System;
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Core.Utils;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors;
using System.IO;
using MySpace.MSFast.Core.Configuration.Common;

namespace MySpace.MSFast.ImportExportsMgrs.HARObjects
{
    [JSONObject("response")]
    public class Response
    {
        [JSONField("status")]
        public int Status;

        [JSONField("statusText")]
        public String StatusText;

        [JSONField("httpVersion")]
        public String HttpVersion;

        [JSONField("cookies")]
        public Cookie[] Cookies;
        
        [JSONField("headers")]
        public Header[] Headers;

        [JSONField("redirectURL")]
        public String RedirectURL;

        [JSONField("headersSize")]
        public long HeadersSize;

        [JSONField("bodySize")]
        public long BodySize;

        [JSONField("content")]
        public Content Content;

        public Response(DownloadState ds, ProcessedDataPackage package)
        {
            ResponseHeaderDumpFilesInfo responseHeaderFileInfo = new ResponseHeaderDumpFilesInfo(package);
            ResponseBodyDumpFilesInfo responseBodyFileInfo = new ResponseBodyDumpFilesInfo(package);


            StatusText = "OK";                  /*TODO - Parse Header*/
            HttpVersion = "HTTP/1.1";           /*TODO - Parse Header*/
            Cookies = new Cookie[0];            /*TODO - Parse Header*/
            Headers = new Header[0];            /*TODO - Parse Header*/
            RedirectURL = "";                   /*TODO - Parse Header*/
            HeadersSize = -1;
            BodySize = -1;
            Status = 200;



            try
            {
                FileInfo fi = new FileInfo(responseBodyFileInfo.GetFullPath(ds.FileGUID));

                if (fi != null && fi.Exists)
                    this.BodySize = fi.Length;

                MemoryStream ms = new MemoryStream();
                Stream s = responseBodyFileInfo.Open(FileAccess.Read, ds.FileGUID);

                byte[] buffer = new byte[2000];
                int len;

                while ((len = s.Read(buffer, 0, 2000)) > 0)
                {
                    ms.Write(buffer, 0, len);
                }
                ms.Flush();
                s.Close();

                String cont = Convert.ToBase64String(ms.ToArray());

                this.Content = new Content()
                {
                    Size = this.BodySize,
                    Text = cont,
                    MimeType = "text/html"      /*TODO - Parse Header*/
                };
            }
            catch
            {
            }
        }
    }
}
