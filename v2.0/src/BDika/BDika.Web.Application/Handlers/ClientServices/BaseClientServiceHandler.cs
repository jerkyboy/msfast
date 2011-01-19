using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using EYF.Web.Handlers.Common;
using EYF.Web.Common;
using BDika.Client.API.Comm;
using BDika.Entities.Tests;
using BDika.Providers.Collectors;

namespace BDika.Web.Application.Handlers.ClientServices
{
    public abstract class BaseClientServiceHandler<T> : BaseHttpHandler<T>
    {
        [RequestFieldAttributes("client_key", true)]
        public ClientKey ClientKey;
        
        [RequestFieldAttributes("client_id", false)]
        public ClientID ClientID;

        public override bool IsReusable{get { return true; } }

        public override void ProcessRequest(HttpContext context)
        {
            //Check clientid and clientkey
            ServerResponse response = null;

            TesterType testerType = null;

            if (ClientID.IsValidClientID(ClientID) && ClientKey.IsValidClientKey(ClientKey))
            {
                testerType = CollectorsProvider.GetTesterType(this.ClientID, this.ClientKey);
            }

            if (testerType == null)
            {
                response = new ServerResponse();
                response.IsSucceeded = false;
                response.ErrorCode = (uint)ErrorCodes.InvalidCredentials;
            }
            else
            {
                try
                {
                    response = ProcessClientRequest(context, testerType);
                }
                catch
                {
                }
            }

            if (response == null)
            {
                response = new ServerResponse();
                response.IsSucceeded = false;
                response.ErrorCode = (uint)ErrorCodes.UnexpectedError;
            }

            context.Response.Write(response.Serialize());
        }

        public abstract ServerResponse ProcessClientRequest(HttpContext context, TesterType testerType);
    }
}
