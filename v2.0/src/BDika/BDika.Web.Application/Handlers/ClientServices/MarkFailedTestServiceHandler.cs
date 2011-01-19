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

namespace BDika.Web.Application.Handlers.ClientServices
{
    [Linkable(Target = "/Handlers/ClientServices/MarkFailedTestServiceHandler.axd")]
    [BDikaPageAttributes]
    public class MarkFailedTestServiceHandler : BaseClientServiceHandler<MarkFailedTestServiceHandler>
    {
        [RequestFieldAttributes("r", false)]
        public ResultsID ResultsID;

        public override ServerResponse ProcessClientRequest(HttpContext context, TesterType testerType)
        {
            ServerResponse serv = new ServerResponse();
            serv.IsSucceeded = false;

            if (ResultsID.IsValidResultsID(this.ResultsID) == false)
                return serv;

            serv.IsSucceeded = ResultsProvider.MarkFailedResults(this.ResultsID);

            return serv;
        }
    }
}
