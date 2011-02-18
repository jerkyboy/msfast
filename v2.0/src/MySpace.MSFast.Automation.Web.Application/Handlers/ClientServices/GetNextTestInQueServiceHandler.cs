//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Web.Application)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
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
using MySpace.MSFast.Automation.Web.Core.Common;
using EYF.Web.Common;
using MySpace.MSFast.Automation.Client.API.Comm;
using MySpace.MSFast.Core.Configuration.CollectorsConfig;
using MySpace.MSFast.Automation.Providers.Collectors;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Results;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.ClientServices
{
    [Linkable(Target = "/Handlers/ClientServices/GetNextTestInQueServiceHandler.axd")]
    [MSFAPageAttributes]
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
                serv.TestName = res.Test.TestName;
            }

            return serv;
        }
    }
}
