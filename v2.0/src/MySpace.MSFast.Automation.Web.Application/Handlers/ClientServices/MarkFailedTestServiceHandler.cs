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
using EYF.Web.Common;
using MySpace.MSFast.Automation.Web.Core.Common;
using MySpace.MSFast.Automation.Client.API.Comm;
using MySpace.MSFast.Automation.Entities.Tests;
using MySpace.MSFast.Automation.Providers.Results;
using MySpace.MSFast.Automation.Entities.Results;

namespace MySpace.MSFast.Automation.Web.Application.Handlers.ClientServices
{
    [Linkable(Target = "/Handlers/ClientServices/MarkFailedTestServiceHandler.axd")]
    [MSFAPageAttributes]
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
