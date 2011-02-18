//=======================================================================
/* Project: MSFast (MySpace.MSFast.Automation.Client.API)
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
using System.Collections.Generic;
using System.Text;
using MySpace.MSFast.Automation.Client.API.Tests;

namespace MySpace.MSFast.Automation.Client.API.Comm
{
    public class MarkFailedTestCall : TestingClientCall<ServerResponse>
    {
        public TestIteration TestIteration;

        public override ServerResponse ExecuteCall(String baseDomain, string clientID, string clientKey, int timeout)
        {
            if (this.TestIteration == null || this.TestIteration.ResultsID == 0)
                throw new Exception("Invalid Test Iteration");

            return base.ExecuteCall(baseDomain,clientID, clientKey, timeout);
        }

        public override void PrepareArguments()
        {
            base.PrepareArguments();

            AppendParam("r", this.TestIteration.ResultsID.ToString());
        }

        public override Uri GetURL(String baseDomain)
        {
            return new Uri("http://" + baseDomain + "/Handlers/ClientServices/MarkFailedTestServiceHandler.axd"); 
        }
    }
}
