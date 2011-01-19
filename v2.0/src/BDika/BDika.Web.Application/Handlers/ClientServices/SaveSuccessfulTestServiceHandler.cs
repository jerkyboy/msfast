using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Common;
using BDika.Web.Core.Common;
using BDika.Client.API.Comm;
using BDika.Entities.Tests;
using BDika.Providers.Results;
using BDika.Entities.Results;
using System.IO;
using MySpace.MSFast.ImportExportsMgrs;
using MySpace.MSFast.DataProcessors;
using MySpace.MSFast.DataProcessors.Download;
using MySpace.MSFast.DataProcessors.Performance;


namespace BDika.Web.Application.Handlers.ClientServices
{
    [Linkable(Target = "/Handlers/ClientServices/SaveSuccessfulTestServiceHandler.axd")]
    [BDikaPageAttributes]
    public class SaveSuccessfulTestServiceHandler : BaseClientServiceHandler<SaveSuccessfulTestServiceHandler>
    {
        [RequestFieldAttributes("r", false)]
        public ResultsID ResultsID;

        [RequestFieldAttributes("d", false)]
        public String Based64Data;

        public override ServerResponse ProcessClientRequest(HttpContext context, TesterType testerType)
        {
            ServerResponse serv = new ServerResponse();
            serv.IsSucceeded = false;

            if (ResultsID.IsValidResultsID(this.ResultsID) == false || String.IsNullOrEmpty(Based64Data))
                return serv;

            byte[] data = null;
            MemoryStream ms = null;

            try
            {
                data = Convert.FromBase64String(Based64Data);
                ms = new MemoryStream(data);
                MSFImportExportsManager mem = new MSFImportExportsManager();
                
                ProcessedDataPackage pdp = mem.LoadProcessedDataPackage(ms);

                if (pdp != null){
                    serv.IsSucceeded = ResultsProvider.SaveSuccessfulTest(this.ResultsID, pdp);
                }
            }
            catch
            {
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();

                if (data != null)
                    data = null;
            }


            return serv;
        }
    }
}
