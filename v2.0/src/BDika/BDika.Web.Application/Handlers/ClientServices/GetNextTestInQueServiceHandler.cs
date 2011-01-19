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
using BDika.Web.Core.Common;
using EYF.Web.Common;
using BDika.Client.API.Comm;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using BDika.Providers.Collectors;
using BDika.Entities.Tests;
using BDika.Providers.Results;

namespace BDika.Web.Application.Handlers.ClientServices
{
    [Linkable(Target = "/Handlers/ClientServices/GetNextTestInQueServiceHandler.axd")]
    [BDikaPageAttributes]
    public class GetNextTestInQueServiceHandler : BaseClientServiceHandler<GetNextTestInQueServiceHandler>
    {
        public override ServerResponse ProcessClientRequest(HttpContext context, TesterType testerType)
        {
            GetNextTestQueServerResponse serv = new GetNextTestQueServerResponse();
            serv.IsSucceeded = false;

            Entities.Results.Results res = ResultsProvider.GetPendingResults(testerType);

            serv.IsSucceeded = true;
            
            if (res != null)
            {
                serv.RawConfig = res.RawConfig;
                serv.ResultsID = res.ResultsID;
            }

            return serv;
        }
    }
}
